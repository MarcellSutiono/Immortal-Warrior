using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    [SerializeField] public PlayerData pd;
    [SerializeField] public Slider playerHealthBar;

    void Start()
    {
        pd.PlayerHealth = pd.PlayerMaxHealth;
    }

    void Update()
    {
        playerHealthBar.value = pd.PlayerHealth;
    }
}
