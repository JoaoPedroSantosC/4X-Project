using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetarySystem : MonoBehaviour
{
    List<PlanetarySystem> nearbySystems;

    //Production materials here

    GameObject selectionLines;
    private void Start()
    {
        selectionLines = transform.GetChild(0).gameObject;
    }
    public void FindNearbySystems(float radius)
    {
        nearbySystems = new List<PlanetarySystem>();

        foreach (Collider c in Physics.OverlapSphere(transform.position, radius, WorldController.instance.GetSystemLayerMask()))
        {
            if (c.gameObject == gameObject) continue;

            nearbySystems.Add(c.GetComponent<PlanetarySystem>());
        }
    }
    public void SetSystemType()
    {

    }
    public bool CheckIfNearby(PlanetarySystem system)
    {
        return nearbySystems.Contains(system);
    }




    #region Debugging
    private void OnMouseDown()
    {
        selectionLines.SetActive(true);
    }
    [Button("Debug System Detection")]
    public void DebugCastSphere()
    {
        print(nearbySystems.Count);
    }
    #endregion
}
