using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public static EntityController instance;

    EntityData playerEntity;
    EntityData aiEntity;

    void Awake()
    {
        instance = this;
    }

    public void SetPlayerEntity(EntityData entity)
    {
        playerEntity = entity;
    }
    public void SetAIEntity(EntityData entity)
    {
        aiEntity = entity;
    }

    public EntityData GetPlayerEntity()
    {
        return playerEntity;
    }
    public EntityData GetAIEntity()
    {
        return aiEntity;
    }
}
