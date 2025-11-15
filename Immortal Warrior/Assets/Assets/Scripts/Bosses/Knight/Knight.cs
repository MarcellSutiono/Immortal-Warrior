using UnityEngine;
using UnityEngine.UI;

public class Knight : Boss
{
    public Slider healthBar;

    new void Start()
    {
        base.Start();

        MaxHealth = 150f;
        CurrentHealth = MaxHealth;
        Power = 20f;
    }

    new void Update()
    {
        base.Update();
        healthBar.maxValue = MaxHealth;
        healthBar.value = CurrentHealth;
    }
}
