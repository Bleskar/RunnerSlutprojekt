using UnityEngine;
using System;

public class EffectManager : MonoBehaviour
{
    //Effect manager keeps track of particle effects so they easily can be called upon when necessary

    //static variable of the singleton
    public static EffectManager Instance { get; private set; }

    //create a singleton of this
    private void Awake()
    {
        Instance = this;
    }

    //The different effects
    [SerializeField] private ParticleSystem[] particles = new ParticleSystem[0];

    //Play particle effect
    public static void Play(string name, int count, Vector2 position)
        => Instance.PlayLocal(name, count, position);

    void PlayLocal(string name, int count, Vector2 position)
    {
        //find the particle effect
        ParticleSystem ps = Array.Find(particles, i => i.gameObject.name == name);

        //play the particles with the specified parameters
        ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams()
        {
            applyShapeToPosition = true,
            position = position
        };

        ps.Emit(ep, count);
    }
}
