using UnityEngine;

public interface IEnemyMoveable
{
    public Rigidbody2D RB { get; set; }
    public Collider2D ChaseRadius { get; set; }
    public void MoveEnemy();
    public void Chase();
}
