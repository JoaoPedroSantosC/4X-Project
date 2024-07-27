using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class PlayerStock : MonoBehaviour
{
    public static PlayerStock instance;

    //units
    [SerializedDictionary("Unit Type", "Amount Available")]
    public SerializedDictionary<UnitTypes, uint> units;

    //processed resources
    [SerializedDictionary("Processed Resource Type", "Amount Available")]
    public SerializedDictionary<ProcessedResourceTypes, uint> resources;

    void Awake()
    {
        instance = this;
    }

    public void ConsumeResource(ProcessedResourceTypes resource, uint amount)
    {
        if (resources[resource] < amount) return;

        resources[resource] -= amount;
        
        //updade ui
    }

    public void AddResource(ProcessedResourceTypes resource, uint amount)
    {
        resources[resource] += amount;

        //update ui
    }

    public uint GetAvailableResourceAmount(ProcessedResourceTypes resource)
    {
        return resources[resource];
    }
}

public enum UnitTypes
{
    Total,
    Offensive,
    Defensive,
    Production
}
public enum RawResourceTypes
{
    Oxygen,
    Hydrogen,
    Helium,
    Copper,
    Nickel,
    Tin,
    Lithium,
    Titanium,
    Silicon,
    Nitrogen
}
public enum ProcessedResourceTypes
{
    Fuel,
        //FUEL_LiquidHydrogen,
        //FUEL_Hydrazine,
        //FUEL_Kerosene,

    ShipHull,

    HeavyWeapons,

    Metals,


}
