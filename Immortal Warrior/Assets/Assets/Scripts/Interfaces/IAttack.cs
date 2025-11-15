using UnityEngine;

public interface IAttack
{
    public float AttackDelay { get; set; }
    public float AttackCooldown{ get; set; }
    public bool CanAttack { get; set; }
    public Collider2D AttackTriggerRadius { get; set; }
    public Collider2D AttackRadius { get; set; }
    public void BasicAttack(float dmg);
}
