using System;
using UnityEngine;
using UnityEngine.UI;

public class Knight2 : Boss
{
    public Slider healthBar;

    new void Start()
    {
        base.Start();

        MaxHealth = 200f;
        CurrentHealth = MaxHealth;
        Power = 30f;

        anim = GetComponent<Animator>();
    }

    new void Update()
    {
        base.Update();
        healthBar.maxValue = MaxHealth;
        healthBar.value = CurrentHealth;

        anim.SetFloat("xVelocity", MathF.Abs(RB.linearVelocity.x));
    }
}
