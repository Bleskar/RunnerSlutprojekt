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

        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    [Header("Cursors")]
    public Sprite[] mouseSprites = new Sprite[0];

    //resets the level
    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
