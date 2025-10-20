using UnityEngine;

public class PlayerAttackRight : MonoBehaviour
{
    public PlayerData pd;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(pd.attackRight)
        {
            Debug.Log("HIT RIGHT!");
        }
    }
}
