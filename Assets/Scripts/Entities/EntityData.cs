using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "New Entity", menuName = "Data/New Entity")]
public class EntityData : ScriptableObject
{
    public float energyConsumption = 1f;

    [Space]

    public float unitProduction = 1f;
    public float unitLifetimeMultiplier = 1f;

    [SerializedDictionary("Resource", "Multiplier")]
    public SerializedDictionary<RawResourceTypes, float> productionMultipliers;

    [SerializedDictionary("Resource", "Multiplier")]
    public SerializedDictionary<RawResourceTypes, float> consumptionMultipliers;
}
