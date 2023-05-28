using UnityEngine;

public class Button : MonoBehaviour, IKillable
{
    [SerializeField] Mechanism trigger; //this mechanism will be triggered when the button is shot
    [SerializeField] Sprite changeSprite; //sprite the button will change to when it's been activated

    SpriteRenderer sr;

    bool activated; //keep trck of if the button already has been shot

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Damage(int dmg, Vector2 knock)
    {
        //trigger the mechanism when shot

        if (activated) return; //can't trigger the mechanism twice

        activated = true; //set activated to true and change the sprite
        sr.sprite = changeSprite;

        trigger.Trigger();
    }

    public void Kill()
    {
        //kill won't do anything in this script
    }
}
