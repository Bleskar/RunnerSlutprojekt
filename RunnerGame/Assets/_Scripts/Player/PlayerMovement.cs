using UnityEngine;

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

    [Header("Grounding")]
    [SerializeField] LayerMask ground; //Layermask for the ground, used for effective collision-checking
    [SerializeField] Vector2 groundBoxSize; //size of the collision with ground check-box
    [SerializeField] Vector2 groundBoxOffset; //offset of the collision with ground check-box

    [Header("Jumping")]
    [SerializeField] float cayoteTime = .1f; //how long time after the player has left the ground, can they still jump
    [SerializeField] float jumpStrength = 15f; //how strong the player's jump is

    bool grounded;
    float cayoteJumpTimer;
    bool CanJump => cayoteJumpTimer > 0f && rb.velocity.y <= 0f;

    //REFRENCES
    PlayerAnimation anim; //The playeranimation component of this game object
    Rigidbody2D rb; //reference to the Rigidbody2D component on the player game object

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
    }

    //Like Update, but called on a set interval, and is useful for calculating physics
    private void FixedUpdate()
    {
        grounded = Grounded(); //is player grounded?
    }

    // Update is called once per frame
    void Update()
    {
        float xInput = Input.GetAxis("Horizontal"); //get the x-input
        Movement(xInput); //move the player with x-input

        if (grounded) //if player is grounded then the cayoteJumpTimer is reset
            cayoteJumpTimer = cayoteTime;
        else if (cayoteJumpTimer > 0f) //else the timer will tick down
            cayoteJumpTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump") && CanJump)
            Jump(); //if the player is pressing the jump-button and can jump, the player will jump

        anim.Animate(xInput, grounded); //animate the player
    }

    //returns true if the player is grounded
    bool Grounded()
        => Physics2D.OverlapBox(transform.position + (Vector3)groundBoxOffset, groundBoxSize, 0f, ground);

    //Makes the player move
    void Movement(float input)
    {
        //if the player is making an input
        if (input != 0f)
        {
            //accelerate the player in the direction of the input
            rb.velocity += new Vector2(acceleration * input * Time.deltaTime, 0f);

            //cap the players velocity at topSpeed if x-velocity is greater than the topSpeed
            if (Mathf.Abs(rb.velocity.x) > topSpeed)
                rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * topSpeed, rb.velocity.y);

            return;
        }

        //Get the sign of the x-velocity before deaccelerating
        float sign = Mathf.Sign(rb.velocity.x);

        //make player deaacelerate if they're not inputting
        rb.velocity -= new Vector2(deacceleration * Time.deltaTime * sign, 0f);

        //if the new sign of the players x-velocity is different from the old sign, then stop the player
        if (Mathf.Sign(rb.velocity.x) != sign)
            rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    //Makes the player Jump
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
    }

    //Gizmos helps visualize/debug your code by for example drawing boxes in unity
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + (Vector3)groundBoxOffset, groundBoxSize);
    }
}
