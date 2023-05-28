using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ViewportButton : ViewportUI
{
    Color defaultColor;
    Sprite defaultSprite;
    [SerializeField] Color hoverColor;
    [SerializeField] Sprite hoverSprite;
    [SerializeField] UnityEvent clickEvent;

    private void Start()
    {
        Initialize();
        defaultColor = targetGraphic.color;
        defaultSprite = targetGraphic.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (MouseInside)
        {
            targetGraphic.sprite = hoverSprite;
            targetGraphic.color = hoverColor;

            if (Input.GetMouseButtonDown(0))
                clickEvent.Invoke();
        }
        else
        {
            targetGraphic.color = defaultColor;
            targetGraphic.sprite = defaultSprite;
        }
    }
}
