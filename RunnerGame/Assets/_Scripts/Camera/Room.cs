using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Player Layer")]
    [SerializeField] LayerMask player; //Reference to the player layer

    Vector2 CamBorders => new Vector2(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize); //borders of the camera

    public Vector2 RoomSize => new Vector2(transform.localScale.x, transform.localScale.y); //size of the room
    Vector2 RoomSizeHalf => new Vector2(RoomSize.x, RoomSize.y) * .5f; //half of the room size

    public Vector4 RoomBorders => //the limits of how far the camera can go, taking into account the size of the camera so the camera won't render anything outside the room
        new Vector4(transform.position.x - RoomSizeHalf.x + CamBorders.x, transform.position.x + RoomSizeHalf.x - CamBorders.x, //x portion
            transform.position.y - RoomSizeHalf.y + CamBorders.y, transform.position.y + RoomSizeHalf.y - CamBorders.y); //y portion

    //checks if the player is inside of the room
    public bool CheckForPlayer()
        => Physics2D.OverlapBoxAll(transform.position, RoomSize, 0, player).Length > 0; //returns true if the player is inside the RoomSize

    //When enabling 
    public void SetEnabled(bool e)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(e);
        }
    }

    //Draw the boundaries of the room
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    //When the room is selected, draw fill the inside of the room
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, .1f, 1, .05f);
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
