using System.Collections.Generic;
using System;

//data that is stored in a save
[Serializable]
public class SaveFile
{
    public Score[] scores = new Score[0];

    public SaveFile(List<Score> scores)
    {
        this.scores = scores.ToArray();
    }
}

//a score for a user
[Serializable]
public class Score
{
    public Score(string name, float time, string level)
    {
        this.name = name;
        this.time = time;
        this.level = level;
    }

    public string name;
    public float time;
    public string level;
}
