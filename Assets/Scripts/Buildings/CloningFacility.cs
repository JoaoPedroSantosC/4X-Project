using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloningFacility : Building
{
    new void Awake()
    {
        base.Awake();
        SetData(WorldController.instance.GetBuildingData(BuildingType.CloningFacility));
    }
    void OnEnable()
    {
        //Set energy consumption
        //energyConsumption = 0;
    }
}
