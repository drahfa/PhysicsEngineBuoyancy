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
                buoyancyComponent.entityDensity = 0.917f;

                float submergedHeight;
                float area;

                //We calculate bouyancy force
                if (world.bouyancyBounds.y + world.bouyancyBounds.height > entities.positions[i].y + collisionComponent.radius) //Applying force if an object is fully submerged
                {
                    area = Mathf.PI * collisionComponent.radius * collisionComponent.radius;
                    forceComponent.force += new Vector2(0, world.fluidDensity * Mathf.Abs(world.gravity.y) * area); // F = p * V * g
                }
                else if(world.bouyancyBounds.y + world.bouyancyBounds.height > entities.positions[i].y) //Apllying force if an object is more than a half submerged
                {
                    submergedHeight = Mathf.Abs(entities.positions[i].y) - Mathf.Abs(world.bouyancyBounds.y + world.bouyancyBounds.height) + collisionComponent.radius;
                    area = CircularSegmentArea(submergedHeight, collisionComponent.radius);
                    forceComponent.force += new Vector2(0, world.fluidDensity * Mathf.Abs(world.gravity.y) * area); // F = p * V * g

                }
                else if(world.bouyancyBounds.y + world.bouyancyBounds.height > entities.positions[i].y - collisionComponent.radius) //Applying force if an object is less than half submerged
                {
                    submergedHeight = 1f + Mathf.Abs(entities.positions[i].y) - Mathf.Abs(world.bouyancyBounds.y + world.bouyancyBounds.height) - collisionComponent.radius;
                    area = CircularSegmentArea(submergedHeight, collisionComponent.radius);
                    forceComponent.force += new Vector2(0, world.fluidDensity * Mathf.Abs(world.gravity.y) * area); // F = p * V * g
                }

                entities.forceComponents[i] = forceComponent;
            }
        }
    }

    float CircularSegmentArea(float segmentHeight, float radius)
    {
        float area;

        //area = radius * radius * Mathf.Acos((radius - segmentHeight) / radius) - (radius - segmentHeight) * Mathf.Sqrt(2 * radius * segmentHeight - (segmentHeight * segmentHeight)); // Area of a Circular Segment given its height
        //Leaving the above calculation commented. It outputs the correct result, but I guess it's too slow for the game to handle, "AAB" errors are outputed to the console, sprites stop rendering

        //Assigning area value of 1 instead
        area = 1f;

        return area;
    }
}
