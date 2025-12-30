using System;
using UnityEngine;
using UnityEngine.UI;

public class Knight2 : Boss
{
    public Slider healthBar;

    new void Start()
    {
        base.Start();
        CurrentHealth = MaxHealth;
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
        if(lastBoss)
        {
            Time.timeScale = 0f;
            winUI.SetActive(true);
        }
        else
        {
            audioManager.playSFX(audioManager.rockDoor);
            door.SetActive(true);
            Destroy(gameObject);
        }

    }
}
