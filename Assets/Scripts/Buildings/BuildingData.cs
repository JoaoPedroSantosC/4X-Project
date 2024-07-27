using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "New Building Data", menuName = "Data/New Building Data")]
public class BuildingData : ScriptableObject
{
    [SerializedDictionary("Raw Resource", "Amount consumed")]
    public SerializedDictionary<RawResourceTypes, uint> buildingRawConsumptionPerTick;

    [SerializedDictionary("Processed Resource", "Amount consumed")]
    public SerializedDictionary<ProcessedResourceTypes, uint> buildingProcessedConsumptionPerTick;

    [SerializedDictionary("Processed Resource", "Amount produced")]
    public SerializedDictionary<ProcessedResourceTypes, uint> buildingProductionPerTick;

}
