using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ViewportSlider : ViewportUI
{
    [SerializeField] Text percent;
    [SerializeField] RectTransform bar;
    float startSize;

    bool valueChanging;

    float value;
    public float Value
    {
        get => value;
        set => this.value = Mathf.Clamp01(value);
    }

    [SerializeField] UnityEvent<float> onSliderChange;

    // Start is called before the first frame update
    void Start()
    {
        startSize = bar.sizeDelta.x;
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!valueChanging && Input.GetMouseButtonDown(0) && MouseInside)
        {
            valueChanging = true;
        }

        if (MouseInside && valueChanging)
        {
            Value = Mathf.InverseLerp(BottomLeftCorner.x, TopRightCorner.x, MousePosition.x);
        }

        if (valueChanging && Input.GetMouseButtonUp(0))
        {
            valueChanging = false;
            onSliderChange.Invoke(Value);
        }

        percent.text = $"{value * 100f:N0}%";

        bar.sizeDelta = new Vector2(startSize * Value, bar.sizeDelta.y);
        bar.anchoredPosition = new Vector2(startSize * Value * .5f, 0f);
    }
}
