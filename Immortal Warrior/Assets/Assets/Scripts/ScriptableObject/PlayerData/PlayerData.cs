using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float PlayerMovement = 4f;
    public float PlayerJumpForce = 5f;
}
