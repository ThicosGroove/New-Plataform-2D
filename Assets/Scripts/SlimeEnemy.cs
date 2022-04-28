using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : AEnemy
{
    protected override void Die()
    {
        throw new System.NotImplementedException();
    }

    protected override void LostHealth()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void FreeMovement(float minX, float maxX)
    {
        base.FreeMovement(minX, maxX);
    }
}
