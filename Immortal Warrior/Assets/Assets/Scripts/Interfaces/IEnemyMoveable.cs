using UnityEngine;

public interface IEnemyMoveable
{
    public Rigidbody2D RB { get; set; }
    public void MoveEnemy();
    public void Idle();
}
