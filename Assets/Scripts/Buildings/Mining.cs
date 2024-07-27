using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining : Building
{
    void OnEnable()
    {
        //Set energy consumption    
    }
    public override void Consume()
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

            PlayerStock.instance.ConsumeResource(entry.Key, entry.Value);
        }

        Produce();
    }
    public override void Produce()
    {
        //add data production resources to playerstock
        foreach (var entry in data.buildingProductionPerTick)
        {
            if (entry.Value == 0) continue;

            PlayerStock.instance.AddResource(entry.Key, entry.Value);
        }
    }

    void OnDisable()
    {
        system.ConsumeEvent -= Consume;
    }
}
