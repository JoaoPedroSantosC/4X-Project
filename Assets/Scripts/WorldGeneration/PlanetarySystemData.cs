using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "New System Data", menuName = "Data/New System Data")]
public class PlanetarySystemData : ScriptableObject
{
    [SerializedDictionary("System Type", "Resources")]
    public AYellowpaper.SerializedCollections.SerializedDictionary<SystemType, AYellowpaper.SerializedCollections.SerializedDictionary<RawResourceTypes, uint>> data;
}
public enum SystemType
{

}