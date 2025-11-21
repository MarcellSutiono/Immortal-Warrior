using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public PlayerData pd;
    public Slider playerHealthBar;
    public GameObject loseUI;

    void Start()
    {
        pd.PlayerHealth = pd.PlayerMaxHealth;
    }

    void Update()
    {
        playerHealthBar.value = pd.PlayerHealth;

        if(pd.PlayerHealth <= 0)
        {
            Time.timeScale = 0f;
            loseUI.SetActive(true);
        }
    }

    public void restart()
    {
        StopAllCoroutines();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
