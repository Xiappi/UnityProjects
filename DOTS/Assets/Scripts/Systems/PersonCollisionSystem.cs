using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Rendering;

public class PersonCollisionSystem : SystemBase
{

    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    struct CollisionJob : ITriggerEventsJob
    {
        [ReadOnly]
        public ComponentDataFromEntity<PersonTag> PersonGroup;
        public ComponentDataFromEntity<URPMaterialPropertyBaseColor> ColourGroup;

        public void Execute(TriggerEvent triggerEvent)
        {
            var isPersonA = PersonGroup.HasComponent(triggerEvent.EntityA);
            var isPersonB = PersonGroup.HasComponent(triggerEvent.EntityB);

            if (!isPersonA || !isPersonB)
                return;

            var random = new Random((uint)((triggerEvent.BodyIndexA * triggerEvent.BodyIndexB) + System.DateTime.UtcNow.Millisecond));
            random = ChangeColour(random, triggerEvent.EntityA);
            random = ChangeColour(random, triggerEvent.EntityB);

        }

        private Random ChangeColour(Random random, Entity entity)
        {
            if (ColourGroup.HasComponent(entity))
            {
                var colorComponent = ColourGroup[entity];
                colorComponent.Value.x = random.NextFloat(0, 1);
                colorComponent.Value.y = random.NextFloat(0, 1);
                colorComponent.Value.z = random.NextFloat(0, 1);
                ColourGroup[entity] = colorComponent;
            }
            return random;

        }
    }

    protected override void OnUpdate()
    {
        Dependency = new CollisionJob
        {
            PersonGroup = GetComponentDataFromEntity<PersonTag>(true),
            ColourGroup = GetComponentDataFromEntity<URPMaterialPropertyBaseColor>(),
        }.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, Dependency);
    }
}
