using UnityEngine;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{
    [SerializeField] RectTransform rt; //the cursors rect transform
    Image im; //the image of the cursor
    public CursorType cursorType; //current cursorType

    private void Start()
    {
        im = GetComponent<Image>();
    }

    void Update()
    {
        im.sprite = GameManager.Instance.mouseSprites[(int)cursorType];
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
