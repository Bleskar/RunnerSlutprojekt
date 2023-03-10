using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
}
