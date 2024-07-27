using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;

    //World parameters
    [SerializeField] int systemAmount = 50;
    [SerializeField] int playerCount = 2;
    [SerializeField] Vector3 galaxyDimensions = Vector3.one;

    //Time parameters
    [SerializeField] float tickInterval = 1f;

    //Building parameters
    [SerializeField] int maxBuildingAmount = 3;

    [SerializeField] LayerMask systemLayerMask;

    //Brainstorm
    //Universe age

    public delegate void Tick();
    public event Tick ConsumeTick;
    public event Tick ProduceTick;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(TickCoroutine());
    }
    IEnumerator TickCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickInterval);

            if (ProduceTick !=null) ProduceTick();
            
            if (ConsumeTick != null) ConsumeTick();
        }
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

    public int GetMaxBuildingAmount()
    {
        return maxBuildingAmount;
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
