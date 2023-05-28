using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LevelDetails : MonoBehaviour
{
    [Serializable]
    public class Level
    {
        public string name;
        public Sprite coverSprite;
    }

    public Level[] levels = new Level[0];

    public Image coverImage;
    public Text levelLabel;

    public void SelectLevel(string name)
    {
        Level l = Array.Find(levels, i => i.name == name);

        levelLabel.text = name;
        coverImage.sprite = l.coverSprite;
    }
}
