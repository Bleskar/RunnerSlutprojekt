using UnityEngine;

public class UICursor : MonoBehaviour
{
    [SerializeField] RectTransform rt; //the cursors rect transform

    void Update()
    {
        Cursor.visible = false; //disable the normal cursor

        //set the position to the pixel position of the main camera
        rt.anchoredPosition = Camera.main.ViewportToScreenPoint(CameraController.DisplayCamera.ScreenToViewportPoint(Input.mousePosition) - Vector3.one * .5f); 
    }
}
