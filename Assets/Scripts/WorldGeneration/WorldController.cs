using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;

    //Entities
    [SerializeField] EntityData[] entities;
    EntityData playerEntity;

    //World parameters
    [SerializeField] int systemAmount = 50;
    [SerializeField] int playerCount = 2;
    [SerializeField] Vector3 galaxyDimensions = Vector3.one;

    //Time parameters
    [SerializeField] float tickInterval = 1f;
    [SerializeField] int monthsPerTick = 1;

    //Building parameters
    [SerializeField] int maxBuildingAmount = 3;

    [SerializeField] LayerMask systemLayerMask;

    //Building data dictionary
    [Space][SerializedDictionary("Processed Resource", "Amount produced")]
    public SerializedDictionary<BuildingType, BuildingData> buildingsDictionary;

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
        if (entities.Length != 2) return;

        if (entities[0] == entities[1]) return;

        EntityController.instance.SetPlayerEntity(entities[0]);
        EntityController.instance.SetAIEntity(entities[1]);

        GetComponent<SystemGenerator>().GenerateSystems(systemAmount, galaxyDimensions, entities);
    }

    [Button("Regenerate Universe", EButtonEnableMode.Playmode)]
    public void RegenerateUniverse()
    {
        if (entities.Length != 2) return;

        if (entities[0] == entities[1]) return;

        EntityController.instance.SetPlayerEntity(entities[0]);
        EntityController.instance.SetAIEntity(entities[1]);

        GetComponent<SystemGenerator>().RegenerateSystems(systemAmount, galaxyDimensions, entities);
    }

    [Button("Clear Universe", EButtonEnableMode.Playmode)]
    public void ClearUniverse()
    {
        GetComponent<SystemGenerator>().ResetSystems();
    }

    public EntityData GetPlayerEntityData()
    {
        return playerEntity;
    }
    public int GetMaxBuildingAmount()
    {
        return maxBuildingAmount;
    }
    public int GetSystemLayerMask()
    {
        return systemLayerMask;
    }
    public BuildingData GetBuildingData(BuildingType type)
    {
        return buildingsDictionary[type];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, galaxyDimensions);
    }
}
