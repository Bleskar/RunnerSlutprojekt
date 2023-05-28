using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    //used to know if the player should be able to walk yet or not
    public static bool Counting {get; private set;}

    [Header("Refrences")]
    [SerializeField] RectTransform number;
    Image im;

    [Header("Animation options")]
    [SerializeField] Sprite[] sprites = new Sprite[0];
    [SerializeField] float scaleTime = .1f;
    [SerializeField] float stayTime = .5f;
    [SerializeField] Sprite[] goSprites = new Sprite[0];

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f; // change the timescale so nothing moves during the countdown
        im = number.GetComponent<Image>();

        StartCoroutine(Count()); //start counting down
    }

    IEnumerator Count()
    {
        Counting = true;
        im.enabled = false;

        yield return new WaitForSecondsRealtime(.5f); //delay before countdown

        //countdown loop
        im.enabled = true;
        float timer;
        for (int i = 0; i < sprites.Length; i++)
        {
            AudioManager.Play("Tick");
            im.sprite = sprites[i];

            timer = 0f;
            while (timer < scaleTime)
            {
                //cool image effects :)
                number.sizeDelta = new Vector3(112f * timer / scaleTime, 64f);
                number.anchoredPosition = 200f * Vector2.right * (1f - timer / scaleTime);
                timer += Time.unscaledDeltaTime;
                yield return null;
            }

            number.anchoredPosition = Vector2.zero;
            number.sizeDelta = new Vector3(112f, 64f);

            //make the number stay for a little while before changing it to the next
            yield return new WaitForSecondsRealtime(stayTime);

            timer = 0f;
            while (timer < scaleTime)
            {
                //remove the image with cool image effects :)
                number.anchoredPosition = 200f * Vector2.left * timer / scaleTime;
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        //flash the "GO" sprites
        Counting = false;
        Time.timeScale = 1f;
        AudioManager.Play("Go");
        number.anchoredPosition = Vector2.zero;

        for (int i = 0; i < 12; i++)
        {
            im.sprite = goSprites[i % goSprites.Length];
            yield return new WaitForSeconds(.05f);
        }

        timer = 0f;
        while (timer < scaleTime)
        {
            //remove the "GO" image with cool image effects :)
            number.anchoredPosition = 200f * Vector2.left * timer / scaleTime;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        im.enabled = false;
    }
}
