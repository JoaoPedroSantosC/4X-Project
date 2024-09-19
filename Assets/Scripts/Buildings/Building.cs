using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField] protected BuildingData data;

    protected PlanetarySystem system; //system in which is located
    
    protected int energyConsumption = 1; //per tick

    protected EntityStock owningEntityStock;

    public void Awake()
    {
        //get reference to the system in which is located
        system = GetComponent<PlanetarySystem>();
    }
    void Start()
    {
        system.ConsumeEvent += Consume;
    }
    void OnDisable()
    {
        system.ConsumeEvent -= Consume;
    }
     
    public virtual void Consume()
    {
        if (!CanConsume()) return;

        //consume resources from system
        foreach (var entry in data.buildingRawConsumptionPerTick)
        {
            if (entry.Value == 0) continue;

            system.ConsumeResource(entry.Key, entry.Value);
        }

        //consume resources from stock
        foreach (var entry in data.buildingProcessedConsumptionPerTick)
        {
            if (entry.Value == 0) continue;

            owningEntityStock.ConsumeResource(entry.Key, entry.Value);
        }

        Produce();
    }
    public virtual void Produce()
    {
        //add data production resources to playerstock
        foreach (var entry in data.buildingProductionPerTick)
        {
            if (entry.Value == 0) continue;

            owningEntityStock.AddResource(entry.Key, entry.Value);
        }

        //add data production units to system
        foreach (var entry in data.buildingUnitProductionPerTick)
        {
            if (entry.Value == 0) continue;

            system.SetSystemUnits(entry.Key, (int)entry.Value, 60f, true);
        }
    }

    public bool CanConsume()
    {
        if (system.GetEntity() == null) return false;

        if (!system.TrySpendEnergy(energyConsumption)) return false;

        //check system's available resources
        if (data.buildingRawConsumptionPerTick.Count > 0)
        foreach (var entry in data.buildingRawConsumptionPerTick)
        {
            if (system.GetAvailableResourceAmount(entry.Key) < entry.Value) return false;
        }

        //check stock's available resources
        if (data.buildingProcessedConsumptionPerTick.Count > 0)
        foreach (var entry in data.buildingProcessedConsumptionPerTick)
        {
            if (owningEntityStock.GetAvailableResourceAmount(entry.Key) < entry.Value) return false;
        }

        return true;
    }

    public void SetData(BuildingData d)
    {
        data = d;
    }
    public void SetEntityStock(EntityStock stock)
    {
        owningEntityStock = stock;
    }
}
