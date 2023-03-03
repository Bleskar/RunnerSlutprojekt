using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Transform weapon; //transform of the weapon game-object
    SpriteRenderer weaponSr; //spriterender of the weapon

    PlayerAnimation anim; //player animation
    PlayerMovement movement; //player movement

    float recoil; //current recoil of the weapon
    Vector3 weaponOffset; //start weapon offset from the player

    public int ammuntion; //how much ammunition is left in the weapon
    [SerializeField] float shootKnockback = 3f; //knockback
    [SerializeField] float shootDelay = .2f; //delay between shots
    float shootCooldown; //cooldown between shots

    bool reloading; //true if the player is reloading

    [SerializeField] Projectile projectilePrefab; //Reference to the projectile prefab that will be shot out

    public PlayerAnimation Animation
    {
        get => anim;
        set => anim = value;
    }

    public PlayerMovement Movement
    {
        get => movement;
        set => movement = value;
    }

    public Projectile ProjectilePrefab
    {
        get => projectilePrefab;
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponSr = weapon.GetComponent<SpriteRenderer>(); //get the spriterenderer of the weapon
        weaponOffset = weapon.localPosition; //set weapon offset

        Animation = GetComponent<PlayerAnimation>(); //get the player animation component
        Movement = GetComponent<PlayerMovement>(); //get the player animation component

        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = CameraController.CursorPosition; //get the world position of the mouse on the screen
        Vector2 aimDirection = (mousePos - (Vector2)transform.position).normalized; //get the difference betwwen the mouse position and the player position

        //set rotation of the weapon to the
        float rotation = Mathf.Atan2(aimDirection.y, aimDirection.x);
        weapon.rotation = Quaternion.Euler(0f, 0f, rotation * 180f / Mathf.PI);

        if (Input.GetButton("Fire") && shootCooldown <= 0f) //if the fire button is pressed, and the the shoot cooldown is zero or less, then the player shoots
        {
            shootCooldown = shootDelay;
            Shoot(aimDirection);
        }
        else if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;

        if (Input.GetButtonDown("Reload") && ammuntion < 2)
            StartReload();

        weapon.localPosition = new Vector3(aimDirection.x, aimDirection.y) * -recoil + weaponOffset; //set the position of the weapon based on the recoil
        recoil = Mathf.Lerp(recoil, 0f, Time.deltaTime * 8f); //lerp the recoil back to 0

        Animation.PlayerRotation(aimDirection); //set the weapon and player flip
    }

    //shoots a projectile in the direction
    public void Shoot(Vector2 direction)
    {
        //must have ammuntion to shoot
        if (ammuntion <= 0)
            return;
        ammuntion--; //ammunition ticks down after a shot

        recoil = .25f; //set recoil
        Movement.ApplyVelocity(-direction * shootKnockback); //apply knockback
        AudioManager.Play("Blast"); //play the shotgun sound effect

        Projectile clone = Instantiate(projectilePrefab, weapon.transform.position, Quaternion.identity); //clone the prefab to the game
        clone.Direction = direction; //set the direction of the projctile

        if (ammuntion <= 0) //automatically reload if ammunition is 0
            StartReload();
    }

    //starts the reload coroutine
    public void StartReload()
    {
        if (reloading)
            return;

        reloading = true;
        StartCoroutine(Reloading());
    }

    //reload coroutine
    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(.7f);
        reloading = false;
        Reload();
    }

    //reloads the weapon
    public void Reload()
    {
        ammuntion = 2;
    }
}
