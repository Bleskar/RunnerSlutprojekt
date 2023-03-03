using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IKillable
{
    //variables
    public int health;

    //refrences
    protected Rigidbody2D rb;

    public virtual void Damage(int dmg, Vector2 knockback)
    {
        health -= dmg;
        if (health <= 0)
            Kill();
    }

    public abstract void Kill();
}
