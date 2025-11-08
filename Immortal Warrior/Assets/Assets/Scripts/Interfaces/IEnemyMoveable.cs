using UnityEngine;

public interface IEnemyMoveable
{
    public Rigidbody2D RB { get; set; }
    public void MoveEnemy(Vector2 velocity);
}
