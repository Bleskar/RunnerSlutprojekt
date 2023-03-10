using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IKillable
{
    //variables
    public int health;

    //refrences
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    //Simple method for getting all the needed refrences in the child class
    protected void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public virtual void Damage(int dmg, Vector2 knockback)
    {
        sr.color = Color.red; //Apply damage effects to the enemy
        health -= dmg; //decrease health
        CameraController.Instance.ScreenShake(1f); //make the screen shake when an enemy is hit
        if (health <= 0) //check if health is zero, if so then run the "Kill" method
            Kill();
    }

    public abstract void Kill();
}
