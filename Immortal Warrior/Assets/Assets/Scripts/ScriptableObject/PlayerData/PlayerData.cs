using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float PlayerMaxHealth = 100f;
    public float PlayerHealth = 100f;

    public float PlayerDefaultMovement = 6f;
    public float PlayerMovement = 6f;
    public float PlayerMovementCharging = 1f;
    public float PlayerJumpForce = 6f;

    public float PlayerRawPower = 5f;
    public float PlayerPower = 5f;

    //----ATTACK----
    public bool attackLeft = false;
    public bool attackRight = false;
    public float chargeTime = 0f;
    public float maxChargeTime = 3f;

    //----ROLLING----
    public float PlayerRollSpeed = 5f;
    public float PlayerRollDuration = 0.5f;
    public float PlayerRollCooldown = 3f;
    public bool IsRolling = false;

    //----PARRY----
    public bool IsParrying = false;

}
