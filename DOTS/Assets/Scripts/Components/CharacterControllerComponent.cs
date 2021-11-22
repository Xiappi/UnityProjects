using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct CharacterControllerComponent : IComponentData
{
    public float3 currentDirection { get; set; }
    public float3 currentMagnitude { get; set; }
    public bool jump { get; set; }
}
