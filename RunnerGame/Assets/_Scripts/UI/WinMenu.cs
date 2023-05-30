using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    public static WinMenu Instance { get; private set; }

    [SerializeField] GameObject inGameUI;
    [SerializeField] Text inGameTimer;
    [SerializeField] GameObject winMenu;
    [SerializeField] Text winTimer;
    [SerializeField] GameObject nameInputParent;
    [SerializeField] Text nameInputField;
    [SerializeField] GameObject buttons; //restart and menu buttons

    bool Counting => !PlayerMovement.Instance.Frozen; //is the timer counting?
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        Instance = this;

        //all win menu ui items should start inactive
        winMenu.SetActive(false);
        nameInputParent.SetActive(false);
        buttons.SetActive(false);
        winTimer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        inGameTimer.enabled = Counting || PauseMenu.Paused; //only show the ingame timer if the time is ticking or when in the pause menu

        if (Counting)
        {
            timer += Time.deltaTime; //if counting is true, then start to tick up the seconds of the timer
            inGameTimer.text = GameManager.TimeToString(timer);
        }
    }

    public void Win()
    {
        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine()
    {
        UICursor.CurrentCursor = UICursor.CursorType.Pointer; //switch to pointer
        float finalTime = timer; //how much time it took to complete the level'
        AudioManager.PlayMusic("Coolduck"); //play chill music
        inGameTimer.enabled = false;
        winMenu.SetActive(true); //activate the win menu
        inGameUI.SetActive(false); //disable the ingame ui

        yield return new WaitForSeconds(.5f);

        //show the win timer
        winTimer.enabled = true;
        winTimer.text = GameManager.TimeToString(finalTime);
        AudioManager.Play("Land"); //play sound effect when showing the time

        //ask the user to input a three letter name to go with their time (ABC or OSK)
        nameInputParent.SetActive(true);
        string name = "";
        for (int i = 0; i < 3; i++)
        {
            nameInputField.text = name + "~"; //indicate that the player can input their name
            char c = ' ';
            while (c == ' ')
            {
                c = LookForCharacterInput();
                yield return null;
            }
            name += c; //append the character to the string
        }

        nameInputField.text = name;

        //add and save score
        GameManager.Instance.AddScore(new Score(name, finalTime, SceneManager.GetActiveScene().name));

        yield return new WaitForSeconds(.5f);

        //show buttons
        AudioManager.Play("Land"); //play sound effect when showing the time
        buttons.SetActive(true);
    }

    public void Quit()
    {
        GameManager.Instance.MainMenu();
    }

    char LookForCharacterInput()
    {
        for (int i = 0; i < 'Z' - 'A'; i++)
        {
            if (Input.GetKeyDown(KeyCode.A + i))
            {
                //user inputs a valid character
                return (char)('A' + i);
            }
        }

        return ' '; //return space if no letter could be found
    }
}
