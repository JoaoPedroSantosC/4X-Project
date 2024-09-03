using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mining : Building
{
    new void Awake()
    {
        base.Awake();
        SetData(WorldController.instance.GetBuildingData(BuildingType.Mining));
    }
    void OnEnable()
    {
        //Set energy consumption
        //energyConsumption = 0;
    }
}
