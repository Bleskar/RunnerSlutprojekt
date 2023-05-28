using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this class is used to replace unity's built in ui button system.
/// This is due to the fact that two cameras are used in order achieve the pixel art effect.
/// When two cameras are used, unity gets the cursor position on only the first camera, but
/// since the buttons are on the seconds camera the cursor positions are wrong and the buttons cant
/// be press so I needed to make my own button system
/// </summary>
public abstract class ViewportUI : MonoBehaviour
{
    protected Image targetGraphic;
    protected RectTransform tr;

    protected Vector2 BottomLeftCorner => (Vector2)Camera.main.ScreenToViewportPoint(tr.anchoredPosition - tr.sizeDelta * .5f) + tr.anchorMin;
    protected Vector2 TopRightCorner => (Vector2)Camera.main.ScreenToViewportPoint(tr.anchoredPosition + tr.sizeDelta * .5f) + tr.anchorMin;

    protected Vector2 MousePosition => CameraController.ViewportCursorPosition;

    public bool MouseInside
    {
        get
        {
            //if (Time.unscaledTime < lastEnabled + .5f)
            //    return false;

            return MousePosition.x > BottomLeftCorner.x && MousePosition.y > BottomLeftCorner.y && MousePosition.x < TopRightCorner.x && MousePosition.y < TopRightCorner.y;
        }
    }

    float lastEnabled;
    private void OnEnable()
    {
        lastEnabled = Time.unscaledTime;
    }

    protected void Initialize()
    {
        targetGraphic = GetComponent<Image>();
        tr = targetGraphic.rectTransform;
    }
}
