using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public static bool HasWon { get; private set; } //checks if the player has won

    private void Start()
    {
        HasWon = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HasWon)
            return;

        PlayerMovement pm = collision.GetComponent<PlayerMovement>();
        if (pm)
        {
            HasWon = true;
        }
    }
}
