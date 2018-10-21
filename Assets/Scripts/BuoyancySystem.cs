using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancySystem : ISystemInterface
{
    public void Start(World world)
    {
        var entities = world.entities;

        // add randomized velocity to all entities that have positions
        for (var i = 0; i < entities.flags.Count; i++)
        {
            if (entities.flags[i].HasFlag(EntityFlags.kFlagPosition))
            {
                entities.AddComponent(new Entity(i), EntityFlags.kFlagBuoyancy);
            }
        }
    }

    public void Update(World world, float time = 0, float deltaTime = 0)
    {
        var entities = world.entities;
        var gravity = world.gravity;

        for (var i = 0; i < entities.flags.Count; i++)
        {
            if (entities.flags[i].HasFlag(EntityFlags.kFlagBuoyancy) &&
                entities.flags[i].HasFlag(EntityFlags.kFlagForce) &&
                entities.flags[i].HasFlag(EntityFlags.kFlagCollision))
            {
                var forceComponent = entities.forceComponents[i];
                var collisionComponent = entities.collisionComponents[i];
                var buoyancyComponent = entities.bouyancyComponents[i];
                //buoyancyComponent.immersedDiameter = collisionComponent.radius * 2f;
                buoyancyComponent.entityDensity = 917f;
                // F = m * g
                //if (forceComponent.massInverse > 1e-6f)
                //    forceComponent.force += gravity / forceComponent.massInverse;

                if(world.worldBounds.x + world.worldBounds.height > entities.positions[i].y)
                {
                    //Vector2 weight = new Vector2(forceComponent.massInverse * world.gravity.y);
                    //Debug.Log("weight");
                    float area = Mathf.PI * collisionComponent.radius * collisionComponent.radius;
                    forceComponent.force += new Vector2(0, area * Mathf.Abs(world.gravity.y));
                    Debug.Log("forceComponent.force: " + forceComponent.force + ", area: " + area + ", world.gravity.y: " + world.gravity.y);
                }
                else
                {
                    forceComponent.force = Vector2.zero;
                }
                

                entities.forceComponents[i] = forceComponent;
            }
        }
    }
}
