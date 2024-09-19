using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStock : EntityStock
{
    public static AIStock instance;

    void Awake()
    {
        instance = this;
    }
}
