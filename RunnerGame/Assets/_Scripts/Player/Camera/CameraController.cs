using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; } //singleton of the camera controller
    static public Camera DisplayCamera => Instance.displayCamera; //public property for the display camera
    static public Vector2 CursorPosition => //cursor position, gets the pixel position of the cursor and translates it into a world position in the Main Camera
        Camera.main.ViewportToWorldPoint(DisplayCamera.ScreenToViewportPoint(Input.mousePosition));

    public PlayerMovement PlayerMovement
    {
        get => PlayerMovement.Instance;
    }

    [SerializeField] Camera displayCamera; //display camera
    [SerializeField] float speed = 8f; // the speed of the camera

    Room[] rooms; //all the rooms in the scene
    Room currentRoom; //the room that the player is currently in

    float zorig; //original z position of the camera

    private void Awake()
    {
        Instance = this; //set the singleton to this instance
        zorig = transform.position.z; //set the original z position of the camera
    }

    private void Start()
    {
        GetRooms(); //get all rooms in the scene
    }

    // Update is called once per frame
    void Update()
    {
        CheckRooms(); //check which room the player is in

        //getting the target position within the confines of the current room
        Vector3 targetPosition = TryGoToPosition(PlayerMovement.transform.position);
        //lerping the camera position to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
    }

    //returns the new position within the limits of the current room
    Vector3 TryGoToPosition(Vector3 pos)
    {
        pos.z = zorig; //setting the z position to the original z position

        if (!currentRoom) //if the camera doesn't have a room that the player is in then return the unchanged position
            return pos;

        //clamp the x-position
        pos.x = Mathf.Clamp(pos.x, currentRoom.RoomBorders.x, currentRoom.RoomBorders.y);

        //clamp the y-position
        pos.y = Mathf.Clamp(pos.y, currentRoom.RoomBorders.z, currentRoom.RoomBorders.w);

        //return the new position
        return pos;
    }

    //get all the rooms in the scene
    public void GetRooms() => rooms = FindObjectsOfType<Room>();

    //check which room the player is in
    void CheckRooms()
    {
        for (int i = 0; i < rooms.Length; i++) //check for the player in each room
            if (rooms[i].CheckForPlayer())
            {
                //finds player
                if (currentRoom != rooms[i])
                {
                    //change the room to this room
                    RoomChange(rooms[i]);
                }
                return;
            }
    }

    //change the current room to the room "r"
    void RoomChange(Room r)
    {
        currentRoom = r;

        for (int i = 0; i < rooms.Length; i++) //disable every room except for the one the player is in
        {
            rooms[i].SetEnabled(rooms[i] == r);
        }
    }
}
