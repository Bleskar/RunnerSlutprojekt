using UnityEngine;
using System.Collections;

//this script requires the Rigidbody2D and PlayerAnimation component
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{
    //STATIC VARIABLES
    public static PlayerMovement Instance { get; private set; } //Singleton of the player class

    //VARIABLES
    [Header("Movement")]
    [SerializeField] float topSpeed = 5f; //players max speed allowed
    [SerializeField] float acceleration = 50f; //acceleration when the player is inputting
    [SerializeField] float deacceleration = 80f; //deacceleration when the player isn't inputting

    Vector2 velocity;

    [Header("Grounding")]
    [SerializeField] LayerMask ground; //Layermask for the ground, used for effective collision-checking
    [SerializeField] Vector2 groundBoxSize; //size of the collision with ground check-box
    [SerializeField] Vector2 groundBoxOffset; //offset of the collision with ground check-box

    [Header("Jumping")]
    [SerializeField] float cayoteTime = .1f; //how long time after the player has left the ground, can they still jump
    [SerializeField] float jumpStrength = 15f; //how strong the player's jump is
    [SerializeField] float jumpHold = .2f; //how long can the player hold the jumpbutton (i.e. jump variation; jump height is dependant upon how long the player holds the jump button)
    [SerializeField] float airResistance = 10f; //air resistance, applied to y-velocity

    bool grounded;
    float cayoteJumpTimer;
    bool CanJump => cayoteJumpTimer > 0f && rb.velocity.y <= 0f && !jumping && !Frozen && !Combat.Dead;
    bool jumping;
    float holdTimer;

    public bool Frozen => StartTimer.Counting || WinTrigger.HasWon || Combat.Dead; //can't move when bool is true

    //REFRENCES
    public PlayerCombat Combat { get; private set; } //The combat script attached to this object
    PlayerAnimation anim; //The playeranimation component of this game object
    Rigidbody2D rb; //reference to the Rigidbody2D component on the player game object
    CircleCollider2D col; //collider of the player

    //Called right before start
    private void Awake()
    {
        Instance = this; //set the singleton to this instance
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //getting the rigidbody2D on the player
        anim = GetComponent<PlayerAnimation>(); //getting the player animation script
        col = GetComponent<CircleCollider2D>(); //gets the circle collider of the player
        Combat = GetComponent<PlayerCombat>(); //get the player combat component of this object
    }

    //Like Update, but called on a set interval, and is useful for calculating physics
    private void FixedUpdate()
    {
        CheckWalls(); //check for walls in the directin of the players velocity
        grounded = Grounded(); //is player grounded?
    }

    // Update is called once per frame
    void Update()
    {
        if (Combat.Dead)
            return;

        if (grounded) //if player is grounded
        {
            cayoteJumpTimer = cayoteTime; //cayoteJumpTimer is reset
            if (velocity.y < 0f)
                velocity.y = -2f; //if y velocity is less than zero then set y-vel to 0
        }
        else
        {
            velocity.y += Physics2D.gravity.y * Time.deltaTime; //apply gravity to velocity when the player isn't grounded

            if (cayoteJumpTimer > 0f) //cayote timer will tick down
                cayoteJumpTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && CanJump)
            StartJump(); //if the player is pressing the jump-button and can jump, the player will start jumping


        float xInput = Input.GetAxis("Horizontal"); //get the x-input
        if (Frozen) xInput = 0f; //player is not allowed to move if the frozen variable is true

        velocity.y -= airResistance * Mathf.Pow(velocity.y, 2) * Mathf.Sign(velocity.y) * Time.deltaTime; //apply air resistance
        Movement(xInput); //move the player with x-input


        anim.Animate(xInput, grounded); //animate the player
        rb.velocity = velocity; //set velocity in the rigidbody
    }

    //returns true if the player is grounded
    bool Grounded()
        => Physics2D.OverlapBox(transform.position + (Vector3)groundBoxOffset, groundBoxSize, 0f, ground);

    //Makes the player move
    void Movement(float input)
    {
        //if the player is making an input
        if (input != 0f && Mathf.Abs(rb.velocity.x) < topSpeed)
        {
            //accelerate the player in the direction of the input
            velocity.x += acceleration * input * Time.deltaTime;

            //cap the players velocity at topSpeed if x-velocity is greater than the topSpeed
            if (Mathf.Abs(rb.velocity.x) > topSpeed)
                velocity.x = Mathf.Sign(rb.velocity.x) * topSpeed;

            return;
        }

        //Get the sign of the x-velocity before deaccelerating
        float sign = Mathf.Sign(velocity.x);

        //make player deaacelerate if they're not inputting
        velocity.x -= deacceleration * Time.deltaTime * sign;

        //if the new sign of the players x-velocity is different from the old sign, then stop the player
        if (Mathf.Sign(rb.velocity.x) != sign)
            velocity.x = 0f;
    }

    //starts a jump
    void StartJump()
    {
        jumping = true;
        holdTimer = jumpHold;

        StartCoroutine(Jump());
    }

    //Makes the player jump while holding the jump button
    IEnumerator Jump()
    {
        while (holdTimer > 0)
        {
            if (Input.GetButtonUp("Jump")) //stop jumping if the player releases the jump button
                break;

            velocity.y = jumpStrength;
            holdTimer -= Time.deltaTime;

            yield return null;
        }
        jumping = false;
    }

    //Applies velocity to player
    public void ApplyVelocity(Vector2 addedVelocity)
    {
        velocity += addedVelocity;
    }

    //Checks for wall collisions
    void CheckWalls()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position + (Vector3)col.offset, col.radius - .05f, velocity.x * Vector2.right, Mathf.Abs(velocity.x) * Time.deltaTime, ground);

        if (!hit)
            return;

        velocity.x = 0f;
    }

    //Gizmos helps visualize/debug your code by for example drawing boxes in unity
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)groundBoxOffset, groundBoxSize);
    }
}
