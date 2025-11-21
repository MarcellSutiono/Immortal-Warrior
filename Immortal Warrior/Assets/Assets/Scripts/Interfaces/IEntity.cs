using UnityEngine;

public interface IEntity
{
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public float Speed { get; set; }
    public float ChaseSpeed { get; set; }
    public float Power { get; set; }
    public bool Immunity { get; set; }
    public Animator anim { get; set; }

    public void TakeDamage(float dmg);
    public void death();
}
