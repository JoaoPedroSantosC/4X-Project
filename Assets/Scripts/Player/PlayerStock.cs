using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class PlayerStock : EntityStock
{
    public static PlayerStock instance;

    void Awake()
    {
        instance = this;
    }
}
