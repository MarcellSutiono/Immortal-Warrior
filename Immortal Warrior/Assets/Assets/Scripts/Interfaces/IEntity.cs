using UnityEngine;

public interface IEntity
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    public void TakeDamage(float dmg);
    public void BasicAttack(float dmg);
    public void death();
}
