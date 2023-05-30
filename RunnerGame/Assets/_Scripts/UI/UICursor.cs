using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    [SerializeField] RectTransform rt; //the cursors rect transform
    Image im; //the image of the cursor

    public static CursorType CurrentCursor; //the current cursor type
    public CursorType cursorType; //start cursorType

    private void Start()
    {
        CurrentCursor = cursorType; //set the current cursor type to the one specified in the inspector
        im = GetComponent<Image>();
    }

    void Update()
    {
        im.sprite = GameManager.Instance.mouseSprites[(int)CurrentCursor];
        Cursor.visible = false; //disable the normal cursor

        //set the position to the pixel position of the main camera
        rt.anchoredPosition = Camera.main.ViewportToScreenPoint(CameraController.DisplayCamera.ScreenToViewportPoint(Input.mousePosition) - Vector3.one * .5f); 
    }

    public enum CursorType
    {
        Pointer,
        Target,
    }
}
