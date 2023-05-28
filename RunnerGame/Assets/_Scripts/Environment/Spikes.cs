using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCombat pc = collision.GetComponent<PlayerCombat>(); //look if the collided object is the player
        if (pc) pc.Kill(); //kill the player
    }
}
