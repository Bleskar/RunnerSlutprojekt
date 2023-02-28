using System;
using UnityEngine;
[Serializable]
public class Sound
{
    public string name;

    [HideInInspector] public AudioSource source;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume = 1f;

    public float pitch = 1f;
    public float pitchRandom;

    public void Play(float _volume)
    {
        source.volume = _volume;
        source.pitch = pitch + ((UnityEngine.Random.value - .5f) * pitchRandom);

        source.Play();
    }
}
