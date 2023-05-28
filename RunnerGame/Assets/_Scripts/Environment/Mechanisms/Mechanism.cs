using UnityEngine;
/// <summary>
/// Mechanisms can be triggered through external scripts to 
/// change the players environment, like for examaple to open doors or build bridges
/// </summary>
public abstract class Mechanism : MonoBehaviour
{
    public abstract void Trigger();
}
