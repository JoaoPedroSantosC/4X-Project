using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class EntityStock : MonoBehaviour
{
    //units
    [SerializedDictionary("Unit Type", "Amount Available")]
    public SerializedDictionary<UnitTypes, uint> units;

    //processed resources
    [SerializedDictionary("Processed Resource Type", "Amount Available")]
    public SerializedDictionary<ProcessedResourceTypes, uint> resources;

    public void ConsumeResource(ProcessedResourceTypes resource, uint amount)
    {
        //return if the amount of processed resources required is less than the amount to consume
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
    Production,
    Offensive,
    Defensive,
}
public enum RawResourceTypes
{
    Oxygen,
    Hydrogen,
    Helium,
    Nitrogen,
    Silicon,
    Sulfur,
    Magnesium,
    Calcium,
    Carbon,
    Copper,
    Iron,
    Gold,
    Lithium,
    Aluminum,
    Titanium,
    Uranium,
    Lead,
    ExoticMatter,
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

    Explosives,
}
