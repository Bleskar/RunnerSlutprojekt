using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    bool transitioning;
    RectTransform currentMenu; //the current menu the user is in

    [Header("Transition time")]
    [SerializeField] float transitionTime = .5f;

    float logoEndYPosition; //the deafault logo y position

    [Header("Start menu")]
    [SerializeField] RectTransform mainMenu;
    [SerializeField] Image backgroundCover;
    [SerializeField] RectTransform logo;
    [SerializeField] RectTransform[] buttons = new RectTransform[0];

    [Header("Sliders")]
    [SerializeField] ViewportSlider masterSlider;
    [SerializeField] ViewportSlider musicSlider;
    [SerializeField] ViewportSlider sfxSlider;

    [Header("Level select")]
    [SerializeField] LevelDetails levelDetails;
    [SerializeField] RectTransform levelSelectMenu;
    string levelSelected;

    Coroutine introAnimation;

    // Start is called before the first frame update
    void Start()
    {
        currentMenu = mainMenu;
        introAnimation = StartCoroutine(IntroAnimation());

        logoEndYPosition = logo.anchoredPosition.y;

        masterSlider.Value = AudioManager.Instance.masterVolume;
        musicSlider.Value = AudioManager.Instance.musicVolume;
        sfxSlider.Value = AudioManager.Instance.effectsVolume;
    }

    IEnumerator IntroAnimation()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        backgroundCover.color = Color.black;

        logo.gameObject.SetActive(false);
        yield return new WaitForSeconds(.5f);

        //logo falls onto screen
        float logoYPosition = logo.sizeDelta.y * .5f;
        float logoVelocity = -5f;
        logo.gameObject.SetActive(true);

        while (Mathf.Abs(logoVelocity) > 8f || logoYPosition > logoEndYPosition)
        {
            logoVelocity -= 9.82f * Time.deltaTime * 16f; //apply acceleration
            logoYPosition += Time.deltaTime * logoVelocity; //apply velocity

            if (logoYPosition <= logoEndYPosition) //bounce if logo reaches bottom{
            {
                logoYPosition = logoEndYPosition;
                logoVelocity *= -.5f;
                AudioManager.Play("Land");
            }

            logo.anchoredPosition = Vector2.up * logoYPosition;

            if (Input.GetMouseButtonDown(0))
            {
                SkipIntro(); //skip the intro by clicking
            }
            yield return null;
        }

        logo.anchoredPosition = Vector2.up * logoEndYPosition;

        //start music
        AudioManager.ForceMusic("Coolduck");

        //fade in background
        float timer = 0f;
        while (timer < .5f)
        {
            backgroundCover.color = Color.Lerp(Color.black, Color.clear, timer * 2f);

            timer += Time.deltaTime;
            yield return null;
        }
        backgroundCover.color = Color.clear;

        //slide in buttons
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            timer = 0f;
            while (timer < .5f)
            {
                float f = timer / .5f;
                buttons[i].anchoredPosition = new Vector2(Screen.currentResolution.width * .5f * (1f - f), -24f * i);
                timer += Time.deltaTime;
                yield return null;
            }
            AudioManager.Play("Swoosh");
            buttons[i].anchoredPosition = new Vector2(0f, -24f * i);
        }
    }

    //skips the intro animation
    public void SkipIntro()
    {
        if (introAnimation == null) return;

        StopCoroutine(introAnimation);

        logo.gameObject.SetActive(true);
        logo.anchoredPosition = Vector2.up * logoEndYPosition;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].anchoredPosition = new Vector2(0f, -24f * i);
        }

        AudioManager.ForceMusic("Coolduck");
        backgroundCover.color = Color.clear;
    }

    //start transtion between menus
    public void TransitionMenus(RectTransform menu)
    {
        if (transitioning) return;

        AudioManager.Play("Select");
        StartCoroutine(Transition(menu));
    }

    IEnumerator Transition(RectTransform menu)
    {
        menu.gameObject.SetActive(true);
        transitioning = true;

        float timer = 0f;
        float f = 0f;
        while (timer < transitionTime)
        {
            f = timer / transitionTime;

            currentMenu.offsetMin = Vector2.left * Screen.currentResolution.width * f; //old menu
            currentMenu.offsetMax = Vector2.left * Screen.currentResolution.width * f;

            menu.offsetMin = Vector2.right * Screen.currentResolution.width * (1f - f); //new menu
            menu.offsetMax = Vector2.right * Screen.currentResolution.width * (1f - f);

            timer += Time.deltaTime;
            yield return null;
        }

        currentMenu.gameObject.SetActive(false);
        currentMenu = menu;
        menu.offsetMin = Vector2.zero;
        menu.offsetMax = Vector2.zero;

        transitioning = false;
    }

    //quit the game
    public void Quit()
    {
        Application.Quit();
    }

    //change master volume
    public void ChangeMasterVolume(float volume)
    {
        AudioManager.Instance.masterVolume = volume;
    }

    //change music volume
    public void ChangeMusicVolume(float volume)
    {
        AudioManager.Instance.musicVolume = volume;
    }

    //change sfx volume
    public void ChangeSFXVolume(float volume)
    {
        AudioManager.Play("Blast");
        AudioManager.Instance.effectsVolume = volume;
    }

    //open level details
    public void SelectLevel(string level)
    {
        levelSelected = level;
        levelDetails.SelectLevel(level);
        TransitionMenus(levelSelectMenu);
    }

    //start level
    public void StartLevel()
    {
        AudioManager.ForceStopMusic();
        SceneManager.LoadScene(levelSelected);
    }
}
