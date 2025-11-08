using UnityEngine;

public class Knight : Boss
{
    new void Start()
    {
        base.Start();
        MaxHealth = 150f;
        CurrentHealth = MaxHealth;
        Speed = 5f;
    }

    new void Update()
    {
        base.Update();
    }
}
