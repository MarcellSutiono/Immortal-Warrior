using UnityEngine;

public class PlayerAttackLeft : MonoBehaviour
{
    public PlayerData pd;
    private void OnTriggerStay2D(Collider2D col)
    {
        if(pd.attackLeft)
        {
            Debug.Log("HIT! LEFT");
        }
    }
}
