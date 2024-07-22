using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;

    [SerializeField] int systemAmount = 50;
    [SerializeField] int playerCount = 2;
    [SerializeField] Vector3 galaxyDimensions = Vector3.one;

    [SerializeField] LayerMask systemLayerMask;

    //Brainstorm
    //Universe age

    private void Awake()
    {
        instance = this;
    }

    [Button("Generate Universe", EButtonEnableMode.Playmode)]
    public void StartUniverseGeneration()
    {
        GetComponent<SystemGenerator>().GenerateSystems(systemAmount, playerCount, galaxyDimensions);
    }

    [Button("Regenerate Universe", EButtonEnableMode.Playmode)]
    public void RegenerateUniverse()
    {
        GetComponent<SystemGenerator>().RegenerateSystems(systemAmount, playerCount, galaxyDimensions);
    }

    [Button("Clear Universe", EButtonEnableMode.Playmode)]
    public void ClearUniverse()
    {
        GetComponent<SystemGenerator>().ResetSystems();
    }

    public int GetSystemLayerMask()
    {
        return systemLayerMask;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, galaxyDimensions);
    }
}
