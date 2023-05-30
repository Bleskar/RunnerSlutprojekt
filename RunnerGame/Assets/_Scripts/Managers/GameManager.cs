using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static variable of the singleton
    public static GameManager Instance { get; private set; }

    //create a singleton of this
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        LoadScores();
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public void LoadScores()
    {
        SaveFile sf = SaveSystem.Load();
        if (sf == null) return; //don't load scores if there is no saved data

        for (int i = 0; i < sf.scores.Length; i++)
        {
            scores.Add(sf.scores[i]);
        }

        SortScores();
    }

    [Header("Save file")]
    List<Score> scores = new List<Score>(0); //scores for all players (lowest time is first on the list)

    //use bubble-sort to sort the scores from highest to lowest
    public void SortScores()
    {
        bool sorted = false;
        while(!sorted)
        {
            sorted = true;
            for (int i = 0; i < scores.Count - 1; i++)
            {
                if (scores[i + 1].time < scores[i].time)
                {
                    sorted = false;

                    //switch scores
                    Score temp = scores[i];
                    scores[i] = scores[i + 1];
                    scores[i + 1] = temp;
                }
            }
        }
    }

    //Adds a score, sorts the list and saves the scores
    public void AddScore(Score score)
    {
        scores.Add(score);
        SortScores();
        SaveSystem.Save(new SaveFile(scores));
    }

    // gets scores on the specified level
    public Score[] GetScores(string level)
    {
        List<Score> levelScores = scores.FindAll(i => i.level == level);
        return levelScores.ToArray();
    }

    [Header("Cursors")]
    public Sprite[] mouseSprites = new Sprite[0];

    //resets the level
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //goes back to the main menu
    public void MainMenu()
    {
        AudioManager.ForceStopMusic(); //turn off music
        SceneManager.LoadScene("Menu"); //load the main menu
    }

    //converts a float into minutes and seconds and turns it into a string
    public static string TimeToString(float time)
    {
        float seconds = time % 60;
        int minutes = (int)(time / 60);

        if (minutes > 0) return $"{minutes}:{seconds:N1}";
        else return $"{seconds:N1}";
    }
}
