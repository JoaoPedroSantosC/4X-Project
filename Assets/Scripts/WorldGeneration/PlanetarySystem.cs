using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetarySystem : MonoBehaviour
{
    List<PlanetarySystem> nearbySystems;

    //Owning entity here
    EntityData owningEntity;

    //Units here
    [SerializedDictionary("Unit Type", "Amount Available")]
    public SerializedDictionary<UnitTypes, uint> units;

    //Available resources
    [SerializedDictionary("Raw Resource Type", "Amount Available")]
    public SerializedDictionary<RawResourceTypes, uint> resources;

    //Buildings here
    List<Building> buildings;

    //Stored energy (in MJoules)
    int storedEnergy = 0;

    GameObject selectionLines;

    public event WorldController.Tick ProduceEvent;
    public event WorldController.Tick ConsumeEvent;

    private void Start()
    {
        selectionLines = transform.GetChild(0).gameObject;

        //ProduceEvent += () => { Debug.Log("Produce test"); };
        //ConsumeEvent += () => { Debug.Log("Consume test"); };

        WorldController.instance.ProduceTick += ProduceEvent;
        WorldController.instance.ConsumeTick += ConsumeEvent;
    }
    public void FindNearbySystems(float radius)
    {
        nearbySystems = new List<PlanetarySystem>();

        foreach (Collider c in Physics.OverlapSphere(transform.position, radius, WorldController.instance.GetSystemLayerMask()))
        {
            if (c.gameObject == gameObject) continue;

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
        gameObject.AddComponent(buildingClass.GetType());
        Building building = GetComponent<Building>();

        ConsumeEvent += building.Consume;

        //WorldController.instance.ProduceTick += ProduceEvent;
        WorldController.instance.ConsumeTick += ConsumeEvent;
    }

    public void ConsumeResource(RawResourceTypes resource, uint amount)
    {
        if ((int)(resources[resource] - amount) < 0) return;

        resources[resource] -= amount;
    }

    public void SetSystemType()
    {

    }
    public void SetSystemEntity(EntityData entity)
    {
        owningEntity = entity;

        if (owningEntity == null)
        {
            WorldController.instance.ProduceTick -= ProduceEvent;
            WorldController.instance.ConsumeTick -= ConsumeEvent;
        }
    }
    public uint GetAvailableResourceAmount(RawResourceTypes resource)
    {
        return resources[resource];
    }

    void OnDisable()
    {
        WorldController.instance.ProduceTick -= ProduceEvent;
        WorldController.instance.ConsumeTick -= ConsumeEvent;
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
        Mining m = new Mining();
        AddBuilding(m);
    }

    [Button("Debug System Detection", EButtonEnableMode.Playmode)]
    public void DebugCastSphere()
    {
        print(nearbySystems.Count);
    }
    #endregion
}
