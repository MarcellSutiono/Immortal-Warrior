using System;
using UnityEngine;
using UnityEngine.UI;

public class Knight : Boss
{
    public Slider healthBar;
    public GameObject door;

    new void Start()
    {
        base.Start();

        MaxHealth = 150f;
        CurrentHealth = MaxHealth;
        Power = 20f;

        anim = GetComponent<Animator>();
    }

    new void Update()
    {
        base.Update();
        healthBar.maxValue = MaxHealth;
        healthBar.value = CurrentHealth;

        anim.SetFloat("xVelocity", MathF.Abs(RB.linearVelocity.x));
    }

    public override void death()
    {
        audioManager.playSFX(audioManager.rockDoor);
        door.SetActive(true);
        Destroy(gameObject);
    }
}
