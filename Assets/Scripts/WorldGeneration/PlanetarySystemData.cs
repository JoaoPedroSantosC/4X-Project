using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "New System Data", menuName = "Data/New System Data")]
public class PlanetarySystemData : ScriptableObject
{
    [TextArea(1, 5)]
    public string description;

    [SerializedDictionary("System Type", "Resources")]
    public SerializedDictionary<RawResourceTypes, uint> data;
    //public SerializedDictionary<SystemType, SerializedDictionary<RawResourceTypes, uint>> data;
}
public enum SystemType
{
    Balanced,
    NonmetalAbundant,
    MetalAbundant,
    GasAbundant,
}