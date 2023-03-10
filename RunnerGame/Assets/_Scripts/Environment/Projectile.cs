using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement and collision")]

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

            IKillable ik = GetComponent<IKillable>(); //check if the object can be killed
            if (ik != null)
            {
                ik.Damage(damage, direction);
            }

            Kill(); //kill the projectile
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
