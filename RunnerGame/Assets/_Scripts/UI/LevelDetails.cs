using UnityEngine;
using System;
using UnityEngine.UI;

public class LevelDetails : MonoBehaviour
{
    [Serializable]
    public class Level
    {
        public string name;
        public string displayName;
        public Sprite coverSprite;
    }

    public Level[] levels = new Level[0];

    public Image coverImage;
    public Text levelLabel;

    public Text leaderboard;

    public void SelectLevel(string name)
    {
        Level l = Array.Find(levels, i => i.name == name);

        levelLabel.text = l.displayName;
        coverImage.sprite = l.coverSprite;

        Score[] scores = GameManager.Instance.GetScores(name);
        leaderboard.text = ""; //clear the leaderboard
        //show the top ten highest scores
        for (int i = 0; i < 10; i++)
        {
            if (i >= scores.Length) //don't display anymore scores if there are less than ten scores
            {
                break;
            }
            //add it to the leaderboard
            leaderboard.text += $"{scores[i].name} - {GameManager.TimeToString(scores[i].time)}\n";
        }
    }
}
