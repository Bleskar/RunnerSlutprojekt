using UnityEngine;
using UnityEngine.UI;

public class AmmunitionDisplay : MonoBehaviour
{
    public static AmmunitionDisplay Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [Header("Ammuntion")]
    [SerializeField] Image[] images = new Image[0];

    [SerializeField] Sprite empty;
    [SerializeField] Sprite full;

    [Header("Reload")]
    [SerializeField] GameObject reloadDisplay;
    [SerializeField] RectTransform reloadBar;
    Vector2 startSize;
    public float reloadTime; //value between 0 and 1, how far along is the player with reloading?

    private void Start()
    {
        startSize = reloadBar.sizeDelta;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (i < PlayerCombat.Instance.ammunition)
                images[i].sprite = full;
            else
                images[i].sprite = empty;
        }

        reloadDisplay.SetActive(PlayerCombat.Instance.Reloading);
        if (reloadDisplay.activeSelf)
        {
            reloadBar.sizeDelta = new Vector2(reloadTime * startSize.x, startSize.y);
            reloadBar.anchoredPosition = Vector2.right * (1f + reloadBar.sizeDelta.x * .5f);
        }
    }
}
