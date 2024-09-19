using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlanetarySystem : MonoBehaviour
{
    List<PlanetarySystem> nearbySystems;

    //Owning entity here
    [SerializeField][ReadOnly] EntityData owningEntity = null;

    //Units here
    [SerializedDictionary("Unit Type", "Amount Available")]
    public SerializedDictionary<UnitTypes, uint> units;

    [SerializeField][ReadOnly] uint totalAmountUnits = 0;

    //Available resources
    [SerializedDictionary("Raw Resource Type", "Amount Available")]
    public SerializedDictionary<RawResourceTypes, uint> resources;

    //Buildings here
    List<Building> buildings = new List<Building>();

    //Stored energy (in MJoules)
    int storedEnergy = 9999;

    GameObject selectionLines;

    public event WorldController.Tick ProduceEvent = null;
    public event WorldController.Tick ConsumeEvent = null;

    //Debugging
    bool detectionRangeDebuggingOn = false;

    private void Start()
    {
        SetSelectionLines();

        //ProduceEvent += () => { Debug.Log("Produce test"); };
        //ConsumeEvent += () => { Debug.Log("Consume test"); };

        WorldController.instance.ProduceTick += ProduceEvent;
        WorldController.instance.ConsumeTick += ConsumeEvent;
    }
    void OnDisable()
    {
        WorldController.instance.ProduceTick -= ProduceEvent;
        WorldController.instance.ConsumeTick -= ConsumeEvent;
    }

    public void FindNearbySystems(float radius)
    {
        nearbySystems = new List<PlanetarySystem>();

        foreach (Collider c in Physics.OverlapSphere(transform.position, radius, WorldController.instance.GetSystemLayerMask()))
        {
            if (c.gameObject.transform == gameObject.transform) continue;

            //Debug.Log("Detected system: " + c.gameObject.name, c.gameObject);

            nearbySystems.Add(c.GetComponent<PlanetarySystem>());
        }
    }
    public bool CheckIfNearby(PlanetarySystem system)
    {
        return nearbySystems.Contains(system);
    }

    public void AddBuilding(Building buildingClass)
    {
        if (buildings.Count >= WorldController.instance.GetMaxBuildingAmount()) return;

        //reset events
        //WorldController.instance.ProduceTick -= ProduceEvent;
        WorldController.instance.ConsumeTick -= ConsumeEvent;

        //add building
        Building building = (Building)gameObject.AddComponent(buildingClass.GetType());
        buildings.Add(building);

        SetBuildingStocks();

        ConsumeEvent += building.Consume;

        //WorldController.instance.ProduceTick += ProduceEvent;
        WorldController.instance.ConsumeTick += ConsumeEvent;
    }
    public void SetBuildingStocks()
    {
        if (buildings.Count <= 0) return;

        foreach (Building b in buildings)
        {
            if (owningEntity == EntityController.instance.GetPlayerEntity())
            {
                b.SetEntityStock(PlayerStock.instance);
            }
            else
            {
                b.SetEntityStock(AIStock.instance);
            }
        }
    }

    public void ConsumeResource(RawResourceTypes resource, uint amount)
    {
        if ((int)(resources[resource] - amount) < 0) return;

        resources[resource] -= amount;
    }

    public void SetSystemType(PlanetarySystemData systemData)
    {
        resources = systemData.data;
    }
    public void SetSystemEntity(EntityData entity)
    {
        if (entity == null)
        {
            //resets events in case this system is set to not be owned by any entity
            WorldController.instance.ProduceTick -= ProduceEvent;
            WorldController.instance.ConsumeTick -= ConsumeEvent;

            selectionLines.SetActive(false);

            return;
        }

        owningEntity = entity;

        //for debugging purposes
        if (selectionLines == null) SetSelectionLines();
        selectionLines.SetActive(true);

        SetBuildingStocks();

        WorldController.instance.ProduceTick += ProduceEvent;
        WorldController.instance.ConsumeTick += ConsumeEvent;
    }

    public void MoveSystemUnits(PlanetarySystem s, UnitTypes unit, uint amount, float lifetime)
    {
        units[unit] = (uint)Mathf.Clamp(units[unit] - amount, 0, 9999);
        totalAmountUnits = (uint)Mathf.Clamp(totalAmountUnits - amount, 0, 9999);

        s.SetSystemUnits(unit, (int)amount, lifetime, false);
    }
    public void SetSystemUnits(UnitTypes unit, int amount, float lifetime, bool addToEntityStock)
    {
        if (owningEntity == null) return;

        //add units to dictionary
        units[unit] = (uint)Mathf.Clamp(units[unit] + amount, 0, 999);
        totalAmountUnits = (uint)(totalAmountUnits + amount);

        //add units to entity stock
        EntityStock stock = EntityController.instance.FindStockByEntityData(owningEntity);
        if (addToEntityStock)
        {
            if (stock != null) stock.SetUnits(unit, amount);
        }

        //invoke die method for the units
        StartCoroutine(KillSystemUnits(stock, unit, (uint)amount, lifetime));
    }

    public void InstantlyKillSystemUnits(EntityStock stock, UnitTypes unit, uint amount)
    {
        if (units[unit] <= 0) return;

        units[unit] = (uint)Mathf.Clamp(units[unit] - amount, 0, 9999);
        totalAmountUnits = (uint)Mathf.Clamp(totalAmountUnits - amount, 0, 9999);

        stock.SetUnits(unit, -(int)amount);
    }
    public IEnumerator KillSystemUnits(EntityStock stock, UnitTypes unit, uint amount, float unitLifetime)
    {
        if (units[unit] <= 0) yield return null;

        yield return new WaitForSeconds(unitLifetime);

        units[unit] = (uint)Mathf.Clamp(units[unit] - amount, 0, 9999);
        totalAmountUnits = (uint)Mathf.Clamp(totalAmountUnits - amount, 0, 9999);

        stock.SetUnits(unit, -(int)amount);
    }


    void SetSelectionLines()
    {
        selectionLines = transform.GetChild(0).gameObject;
    }

    public EntityData GetEntity()
    {
        return owningEntity;
    }
    public uint GetAvailableResourceAmount(RawResourceTypes resource)
    {
        return resources[resource];
    }

    

    #region Energy

    public void StoreEnergy(int energy)
    {
        storedEnergy += energy;
    }
    public bool TrySpendEnergy(int energy)
    {
        if (storedEnergy <= 0 || (storedEnergy - energy) < 0) return false;

        storedEnergy -= energy;
        return true;
    }

    #endregion

    #region Debugging
    private void OnMouseDown()
    {
        selectionLines.SetActive(true);
    }

    [Button("Debug Building", EButtonEnableMode.Playmode)]
    public void TestBuilding()
    {
        CloningFacility m = new CloningFacility();
        AddBuilding(m);
    }
    [Button("Debug System Detection", EButtonEnableMode.Playmode)]
    public void DebugCastSphere()
    {
        print(nearbySystems.Count);
    }
    [Button("Debug System Detection Range", EButtonEnableMode.Playmode)]
    public void DebugRangeDetection()
    {
        detectionRangeDebuggingOn = !detectionRangeDebuggingOn;
    }
    [Button("Debug Unit Production", EButtonEnableMode.Playmode)]
    public void DebugUnitProduction()
    {
        SetSystemUnits(UnitTypes.Offensive, 30, 10f, true);
    }

    private void OnDrawGizmos()
    {
        if (!detectionRangeDebuggingOn) return;
        
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
    #endregion

}
