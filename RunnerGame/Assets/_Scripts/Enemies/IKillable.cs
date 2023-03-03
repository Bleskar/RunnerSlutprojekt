using UnityEngine;
public interface IKillable
{
    void Damage(int damage, Vector2 knockback);
    void Kill();
}
