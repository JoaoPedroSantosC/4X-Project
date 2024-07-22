using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class SystemGenerator : MonoBehaviour
{
    List<PlanetarySystem> planetarySystems = new List<PlanetarySystem>();

    [SerializeField] GameObject systemPrefab;
    [SerializeField] Transform systemParent;

    [Space]

    [SerializeField] float systemDetectionRadius = 4f;

    [Space]

    [SerializeField] GameObject lineRendererPrefab;
    [SerializeField] Transform lineRendererParent;

    void InitializeEntities(int playerCount /*change parameter for a Entity class array later*/)
    {
        for (int i = 0; i < playerCount; i++)
        {
            //get random system
            //set player start point to system
            //add x base amount of units in system
        }
    }
    public void GenerateSystems(int amount, int playerCount, Vector3 limits)
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

            system.FindNearbySystems(systemDetectionRadius);

            //add system to planetarySystems list
            planetarySystems.Add(system);
        }
    }

    public void RegenerateSystems(int amount, int playerCount, Vector3 limits)
    {
        if (planetarySystems.Count < 1) return;

        ResetSystems();

        GenerateSystems(amount, playerCount, limits);
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
