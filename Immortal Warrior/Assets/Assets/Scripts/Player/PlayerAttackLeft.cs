using UnityEngine;

public class PlayerAttackLeft : MonoBehaviour
{
    public PlayerData pd;
    private void OnTriggerStay2D(Collider2D col)
    {
        if(pd.attackLeft && col.gameObject.CompareTag("BossHitbox"))
        {
            col.GetComponentInParent<Boss>().TakeDamage(pd.PlayerPower);
        }
    }
}
