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

    //will return the player object if the player is overlapping with sthe specified box
    protected PlayerCombat CheckForPlayer(Vector2 offset, Vector2 size)
    {
        Collider2D[] cda = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size, 0f);
        for (int i = 0; i < cda.Length; i++)
        {
            //check if the collided object is the player
            PlayerCombat pc = cda[i].GetComponent<PlayerCombat>();
            if (pc) return pc;
        }
        //return null if the player wasn't found
        return null;
    }

    //checks for the player in a box, if the player is found, then hurt the player
    protected void AttackBox(Vector2 offset, Vector2 size)
    {
        PlayerCombat pc = CheckForPlayer(offset, size);
        if (pc) pc.Kill();
    }

    public virtual void Damage(int dmg, Vector2 knockback)
    {
        sr.color = Color.red; //Apply damage effects to the enemy
        health -= dmg; //decrease health
        CameraController.Instance.ScreenShake(.1f); //make the screen shake when an enemy is hit
        if (health <= 0) //check if health is zero, if so then run the "Kill" method
            Kill();
    }

    public abstract void Kill();
}
