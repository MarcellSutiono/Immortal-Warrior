using UnityEngine;

public class PlayerAttackRight : MonoBehaviour
{
    public PlayerData pd;
    private void OnTriggerStay2D(Collider2D col)
    {
        if(pd.attackRight && col.gameObject.CompareTag("BossHitbox"))
        {
            col.GetComponentInParent<Boss>().TakeDamage(pd.PlayerPower);
        }
    }
}
