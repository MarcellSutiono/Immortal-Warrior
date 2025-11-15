using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float PlayerMaxHealth = 100f;
    public float PlayerHealth = 100f;

    public float PlayerMovement = 4f;
    public float PlayerJumpForce = 5f;

    public float PlayerRawPower = 5f;
    public float PlayerPower = 5f;

    //----ATTACK----
    public bool attackLeft = false;
    public bool attackRight = false;

    //----ROLLING----
    public float PlayerRollSpeed = 5f;
    public float PlayerRollDuration = 0.5f;
    public float PlayerRollCooldown = 3f;
    public bool IsRolling = false;

}
