using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField][Expandable] protected BuildingData data;

    protected PlanetarySystem system;
    protected int energyConsumption = 1;

    private void Awake()
    {
        system = GetComponent<PlanetarySystem>();
    }
    void Start()
    {
        system.ConsumeEvent += Consume;
    }
    public abstract void Produce();
    public abstract void Consume();
    public bool CanConsume()
    {
        if (!system.TrySpendEnergy(energyConsumption)) return false;

        //check available system resources
        if (data.buildingRawConsumptionPerTick.Count > 0)
        foreach (var entry in data.buildingRawConsumptionPerTick)
        {
            if (system.GetAvailableResourceAmount(entry.Key) < entry.Value) return false;
        }

        //check available stock resources
        if (data.buildingProcessedConsumptionPerTick.Count > 0)
        foreach (var entry in data.buildingProcessedConsumptionPerTick)
        {
            if (PlayerStock.instance.GetAvailableResourceAmount(entry.Key) < entry.Value) return false;
        }

        return true;
    }
}
