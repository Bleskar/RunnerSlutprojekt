using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance { get; private set; } // singleton of this script in the game

    private void Awake()
    {
        Instance = this; //set the singleton
    }

    public bool Dead { get; private set; } //wether or not the player is dead

    [SerializeField] Transform weapon; //transform of the weapon game-object
    SpriteRenderer weaponSr; //spriterender of the weapon

    PlayerAnimation anim; //player animation
    PlayerMovement movement; //player movement

    float recoil; //current recoil of the weapon
    Vector3 weaponOffset; //start weapon offset from the player

    public int ammunition; //how much ammunition is left in the weapon
    [SerializeField] float shootKnockback = 3f; //knockback
    [SerializeField] float shootDelay = .2f; //delay between shots
    float shootCooldown; //cooldown between shots

    public bool reloading; //true if the player is reloading

    [SerializeField] Projectile projectilePrefab; //Reference to the projectile prefab that will be shot out

    // Start is called before the first frame update
    void Start()
    {
        Dead = false;

        weaponSr = weapon.GetComponent<SpriteRenderer>(); //get the spriterenderer of the weapon
        weaponOffset = weapon.localPosition; //set weapon offset

        anim = GetComponent<PlayerAnimation>(); //get the player animation component
        movement = GetComponent<PlayerMovement>(); //get the player animation component

        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = CameraController.CursorPosition; //get the world position of the mouse on the screen
        Vector2 aimDirection = (mousePos - (Vector2)transform.position).normalized; //get the difference betwwen the mouse position and the player position
        if (movement.Frozen) aimDirection = Vector2.right; //don't let the weapon rotate when the player is frozen

        //set rotation of the weapon to the
        float rotation = Mathf.Atan2(aimDirection.y, aimDirection.x);
        weapon.rotation = Quaternion.Euler(0f, 0f, rotation * 180f / Mathf.PI);

        if (Input.GetButton("Fire") && shootCooldown <= 0f && !Reloading && !movement.Frozen) //if the fire button is pressed, and the the shoot cooldown is zero or less, then the player shoots
        {
            shootCooldown = shootDelay;
            Shoot(aimDirection);
        }
        else if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;

        if (Input.GetButtonDown("Reload") && ammunition < 2 && !Reloading && !movement.Frozen)
            StartReload();

        weapon.localPosition = new Vector3(aimDirection.x, aimDirection.y) * -recoil + weaponOffset; //set the position of the weapon based on the recoil
        recoil = Mathf.Lerp(recoil, 0f, Time.deltaTime * 8f); //lerp the recoil back to 0

        anim.PlayerRotation(aimDirection); //set the weapon and player flip
    }

    //shoots a projectile in the direction
    public void Shoot(Vector2 direction)
    {
        //must have ammuntion to shoot
        if (ammunition <= 0)
            return;
        ammunition--; //ammunition ticks down after a shot

        recoil = .25f; //set recoil
        movement.ApplyVelocity(-direction * shootKnockback); //apply knockback
        AudioManager.Play("Blast"); //play the shotgun sound effect

        Projectile clone = Instantiate(projectilePrefab, weapon.transform.position, Quaternion.identity); //clone the prefab to the game
        clone.Direction = direction; //set the direction of the projctile

        if (ammunition <= 0) //automatically reload if ammunition is 0
            StartReload();
    }

    //starts the reload coroutine
    public void StartReload()
    {
        if (reloading)
            return;

        reloading = true;
        StartCoroutine(ReloadRoutine());
    }

    public bool Reloading => reloading;

    //reload coroutine
    IEnumerator ReloadRoutine()
    {
        float timer = 0f;

        while (timer < .7f)
        {
            timer += Time.deltaTime;
            AmmunitionDisplay.Instance.reloadTime = timer / .7f;
            yield return null;
        }

        reloading = false;
        Reload();
    }

    //reloads the weapon
    public void Reload()
    {
        ammunition = 2;
    }

    //this method should be called once the player collides with an enemy
    public void Kill()
    {
        if (Dead) return;

        Dead = true;
        StartCoroutine(DeathAnimation());
    }

    //death animation
    IEnumerator DeathAnimation()
    {
        AudioManager.Play("Death"); //play the death sound effect
        EffectManager.Play("Feathers", 20, transform.position);
        weaponSr.enabled = false;

        yield return new WaitForSeconds(2f);

        GameManager.Instance.ResetLevel(); //reset the level
    }
}
