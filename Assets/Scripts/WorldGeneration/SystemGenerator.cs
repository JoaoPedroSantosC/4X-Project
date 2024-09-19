using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemGenerator : MonoBehaviour
{
    List<PlanetarySystem> planetarySystems = new List<PlanetarySystem>();

    [SerializeField] PlanetarySystemData[] planetarySystemDatas;
    [SerializeField] GameObject systemPrefab;
    [SerializeField] Transform systemParent;

    [Space]

    [SerializeField] float systemDetectionRadius = 4f;

    [Space]

    [SerializeField] GameObject lineRendererPrefab;
    [SerializeField] Transform lineRendererParent;

    void InitializeEntities(EntityData[] entities) //Update this to set the initial position to the farthest systems
    {
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i] == null) continue;

            while (true)
            {
                //get random system & set player start point to system
                PlanetarySystem randomSystem = planetarySystems[Random.Range(0, planetarySystems.Count)];

                if (randomSystem.GetEntity() != null) continue;

                randomSystem.SetSystemEntity(entities[i]);
                
                //Set x amount of starting units
                randomSystem.SetSystemUnits(UnitTypes.Production, 100, 30f, true);
                break;
            }
        }
    }

    public PlanetarySystem[] FindSystemByOwner(EntityData entity)
    {
        List<PlanetarySystem> systems = new List<PlanetarySystem>();

        //find and add system to systems
        foreach (PlanetarySystem s in planetarySystems)
        {
            if (s.GetEntity() == entity) systems.Add(s);
        }

        return systems.ToArray();
    }
    public void GenerateSystems(int amount, Vector3 limits, EntityData[] entities)
    {
        if (planetarySystems.Count > 0) return;

        limits /= 2f;

        //for loop (amount)
        for (int i = 0; i < amount; i++)
        {
            Vector3 systemPosition;
            while (true)
            {
                //generate random position inside limits
                systemPosition = new Vector3(Random.Range(-limits.x, limits.x), 0, Random.Range(-limits.z, limits.z));

                //break if generated position is valid
                if ((systemPosition.x > -limits.x && systemPosition.x < limits.x) && (systemPosition.z > -limits.z && systemPosition.z < limits.z)) break;
            }

            //instantiate system
            PlanetarySystem system = Instantiate(systemPrefab, systemPosition, Quaternion.identity).GetComponent<PlanetarySystem>();
            system.transform.SetParent(systemParent);
            
            //set random system type
            system.SetSystemType(planetarySystemDatas[Random.Range(0, planetarySystemDatas.Length)]);

            //add system to planetarySystems list
            planetarySystems.Add(system);
        }

        FindSystemsNeighbors();

        InitializeEntities(entities);
    }
    void FindSystemsNeighbors()
    {
        foreach (PlanetarySystem system in planetarySystems)
        {
            system.FindNearbySystems(systemDetectionRadius);
        }
    }
    public void RegenerateSystems(int amount, Vector3 limits, EntityData[] entities)
    {
        if (planetarySystems.Count < 1) return;

        ResetSystems();

        GenerateSystems(amount, limits, entities);
    }
    public void ResetSystems()
    {
        foreach(PlanetarySystem system in planetarySystems)
        {
            Destroy(system.gameObject);
        }

        planetarySystems.Clear();
    }
}
