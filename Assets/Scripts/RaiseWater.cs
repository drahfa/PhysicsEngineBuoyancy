using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseWater : MonoBehaviour
{
    private void Start()
    {
        transform.position = new Vector2(0, World.instance.bouyancyBounds.y + World.instance.bouyancyBounds.height);        
    }

    public void RaiseWaterLevel(float newPosValue)
    {
        World.instance.bouyancyBounds.height = newPosValue;
        transform.position = new Vector2(0, World.instance.bouyancyBounds.y + newPosValue);
    }
}
