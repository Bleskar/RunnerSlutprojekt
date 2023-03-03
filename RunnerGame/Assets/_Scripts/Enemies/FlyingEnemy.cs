using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : EnemyBase
{
    public override void Kill()
    {
        Destroy(gameObject);
    }
}
