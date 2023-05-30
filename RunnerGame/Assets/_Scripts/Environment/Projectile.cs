using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement and collision")]
    [SerializeField] int bounces = 0; // how many times the projectile bounces
    Vector2 direction; //direction must always be normalized;

    public Vector2 Direction
    {
        get => direction;
        set => direction = value.normalized;
    }

    [SerializeField] float speed = 20f;
    [SerializeField] float radius = .25f;

    [Header("Settings")]
    [SerializeField] int damage = 1; //how much damage does this projectile do?
    [SerializeField] LayerMask targets; //which layer will this projectile target?
    [SerializeField] float aliveTime = 5f; //how long will this projectile be alive
    bool dead;

    bool Travelling => !dead; //is this object allowed to travel?

    //refrences
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sr.enabled = !dead; //object is visible onlye when it's alive

        if (Travelling)
            Travel(Time.deltaTime); //let the object travel when it can

        aliveTime -= Time.deltaTime; //count down the aliveTime
        if (aliveTime <= 0f) //kill this object if aliveTime is less than 0
            Kill();
    }

    //Moves the projectile according to the deltaTime, direction, speed and radius
    public void Travel(float deltaTime)
    {
        //check if the projectile hits anything in the next frame
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, radius, Direction, deltaTime * speed, targets);
        if (hit)
        {
            //projectiel has hit an object
            transform.position += hit.distance * (Vector3)Direction; //move the projectile to the hit

            IKillable ik = hit.transform.GetComponent<IKillable>(); //check if the object can be killed
            if (ik != null)
            {
                //do damage
                ik.Damage(damage, direction);
            }

            EffectManager.Play("BounceSpark", 20, transform.position);
            //if it has bounces left, then bounce off of the surface
            if (bounces > 0)
            {
                bounces--;
                Bounce(hit.normal);
                //set the position to a position away for the wall,
                //so the projectile doesnt collide multiple times
                transform.position = hit.point + Direction * .25f;
            }
            else
            {
                Kill(); //if there are no bounces left, kill the projectile
            }

            return;
        }

        //if the projectile doesn't hit anything this frame, it moves along as normal
        transform.position += deltaTime * speed * (Vector3)Direction;
    }

    public void Kill()
    {
        if (dead)
        {
            Destroy(gameObject); //destroy if it's dead
            return;
        }

        dead = true;
        aliveTime = 3f; //the projectile must stay alive a few seconds after it's been killed so that the particle effects can finish emitting
    }

    public void Bounce(Vector2 normal)
    {
        if (sr.isVisible) AudioManager.Play("Bounce"); //only play the bounce sound when the projectile is on screen
        Direction = Vector2.Reflect(Direction, normal);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
