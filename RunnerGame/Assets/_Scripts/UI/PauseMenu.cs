using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }
    public static bool Paused { get; private set; }

    bool CanPause => !StartTimer.Counting && !WinTrigger.HasWon; //bool that determines if the user can pause or not

    [Header("Refrences")]
    [SerializeField] Image pauseBackground;
    [SerializeField] GameObject mainMenu;

    private void Start()
    {
        Instance = this;
        PauseLocal(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && CanPause) //pause or unpause when the player hits the pause button (tab or p)
            Pause(!Paused);
    }

    public void Pause(bool pause)
    {
        if (Paused == pause) return;
        PauseLocal(pause);
    }

    void PauseLocal(bool pause)
    {
        Paused = pause;

        pauseBackground.enabled = pause; //activate the background
        mainMenu.SetActive(pause); //activate the menu
        Time.timeScale = pause ? 0f : 1f; //freeze time when the user pauses
        UICursor.CurrentCursor = pause ? UICursor.CursorType.Pointer : UICursor.CursorType.Target; //switch to the appropriate cursor
    }

    public void Restart()
    {
        GameManager.Instance.ResetLevel();
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        GameManager.Instance.MainMenu();
    }
}
