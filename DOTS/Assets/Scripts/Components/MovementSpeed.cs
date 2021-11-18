using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MovementSpeed : IComponentData
{
    public float Value;
}
