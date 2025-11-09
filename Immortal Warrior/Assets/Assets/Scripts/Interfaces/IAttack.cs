using UnityEngine;

public interface IAttack
{
    public float Cooldown { get; set; }
    public Collider2D AttackRadius { get; set; }
    public void BasicAttack(float dmg);
}
