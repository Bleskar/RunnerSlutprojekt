using System.Collections;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } //Singleton of the audio manager

    AudioSource musicSource; //audiosource of the music
    public string CurrentMusic => currentSound.name; //current playing music
    Sound currentSound; //current music information
    bool transitioning; //is the music transitioning?

    [HideInInspector] public bool death; //is the player dead?

    private void Awake()
    {
        if (Instance)
        {
            //if there already exists an audiomanager, then destroy this one and play area music
            PlayMusic(areaMusic);
            Destroy(gameObject);
            return;
        }

        //if there is no other audio manager, then this is the singleton
        Instance = this;
        DontDestroyOnLoad(gameObject); //this object should not be destroyed inbetween scenes

        for (int i = 0; i < soundLibrary.Length; i++) //initialize all of the sounds
        {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            soundLibrary[i].source = s; //create and add a reference to the audiosource in the "Sound" object
            s.clip = soundLibrary[i].clip; // set the audioclip of the audiomanager
            s.spatialBlend = 0f; //make the sound 2D
        }

        musicSource = gameObject.AddComponent<AudioSource>(); //add an audiosource for the music
        musicSource.loop = true; //make it loop

        PlayMusic(areaMusic); //play the scene music
    }

    [Range(0f, 1f)] public float masterVolume = 1f;

    [Header("Sound Effects")]
    [Range(0f, 1f)] public float effectsVolume = 1f;
    public Sound[] soundLibrary = new Sound[0];

    [Header("Music")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    public Sound[] musicLibrary = new Sound[0];
    [SerializeField] float transitionTime = .5f; //how long is the transition between music
    [SerializeField] string areaMusic; //what music will play on this scene

    private void Update()
    {
        if (!transitioning)
        {
            if (currentSound != null)
                musicSource.volume = musicVolume * currentSound.volume * masterVolume; //set the volume of the music
            else
                musicSource.volume = 0f; //if there is now current music then set volume to 0
        }

        musicSource.pitch = Mathf.Lerp(musicSource.pitch, death ? 0f : 1f, Time.deltaTime * 5f); //if the player is dead then the music slows down
    }

    //Play a sound on the instance of the audio manager
    public void PlayLocal(string a)
    {
        Sound s = Array.Find(soundLibrary, i => i.name == a);
        s.Play(s.volume * effectsVolume * masterVolume);
    }

    //Play or change music on the instance of the audio manager
    public void PlayMusicLocal(string a)
    {
        Sound s = Array.Find(musicLibrary, i => i.name == a);

        if (s == currentSound) return;

        StopAllCoroutines();
        StartCoroutine(MusicTransition(currentSound, s));

        currentSound = s;
    }

    //handles the transition between different songs
    IEnumerator MusicTransition(Sound oldSound, Sound s)
    {
        transitioning = true;
        float timer;

        if (oldSound != null)
        {
            timer = transitionTime;
            while (timer > 0f)
            {
                musicSource.volume = (timer / transitionTime) * masterVolume * oldSound.volume * musicVolume;
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        if (s != null)
        {
            musicSource.clip = s.clip;
            musicSource.Play();

            timer = transitionTime;
            while (timer > 0)
            {
                musicSource.volume = (1f - (timer / transitionTime)) * masterVolume * musicVolume * s.volume;
                timer -= Time.deltaTime;
                yield return null;
            }

            musicSource.volume = masterVolume * musicVolume * s.volume;
        }

        transitioning = false;
    }

    //static methods for playing sound effects or music on the audiomanager singleton
    public static void Play(string a) => Instance.PlayLocal(a);
    public static void PlayMusic(string a) => Instance.PlayMusicLocal(a);
}