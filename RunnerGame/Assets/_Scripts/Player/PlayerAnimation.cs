using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public static string CurrentAnimation { get; private set; } //the current animation of the player
    Animator anim; //the Animator of the player GameObject
    SpriteRenderer sr; //spriterenderer of the player GameObject
    [SerializeField] SpriteRenderer weaponSpriteRenderer; //spriterenderer of the player's weapon

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); //Get the animator component
        sr = GetComponent<SpriteRenderer>(); //Get the spriterenderer component
    }

    //sets the x-flip of the player and the weapon
    public void PlayerRotation(Vector2 aimDirection)
    {
        sr.flipX = aimDirection.x < 0f; //flip the player's sprite in the correct direction
        weaponSpriteRenderer.flipY = aimDirection.x < 0f; //flip the weapon's sprite in the correct direction
    }

    //standard animitions that are called from outside the script using player input information
    public void Animate(float xInput, bool grounded)
    {
        if (!grounded)
        {
            Play("Jump"); //if the player isn't grounded then the "Jump" animation should play
            return;
        }

        if (xInput != 0f)
        {
            //play the walk animation when the player is moving
            Play("Walk");
        }
        else
        {
            //Idle animation when the player isn't moving
            Play("Idle");
        }
    }

    //Changes the current animation
    //Can be called multiple times without having the animation reset
    public void Play(string animation)
    {
        if (CurrentAnimation == animation)
            return; //return if this animation is already playing

        anim.Play(animation); //play the new animation
        CurrentAnimation = animation; //change the CurrentAnimation variable
    }
}
