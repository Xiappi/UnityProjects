using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SetupSpawner : MonoBehaviour
{

    [SerializeField] GameObject prefab;
    [SerializeField] int gridSize;
    [SerializeField] int spread;

    private BlobAssetStore blob;

    void Start()
    {
        blob = new BlobAssetStore();

        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob);
        var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                float3 pos = new float3(x * spread, 0, z * spread);
                em.SetComponentData(entity, new Translation { Value = pos });
                em.SetComponentData(entity, new MovementSpeed { Value = UnityEngine.Random.Range(1, 10) });
                // em.SetComponentData(entity, new Destination { Value = pos });

                var instance = em.Instantiate(entity);
            }
        }

    }

    void OnDestroy()
    {
        blob.Dispose();
    }

}
