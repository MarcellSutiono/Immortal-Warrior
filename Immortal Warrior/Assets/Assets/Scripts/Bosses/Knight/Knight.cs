using UnityEngine;

public class Knight : Boss
{
    new void Start()
    {
        base.Start();
        MaxHealth = 150f;
        CurrentHealth = MaxHealth;
    }

    new void Update()
    {
        base.Update();
    }
}
