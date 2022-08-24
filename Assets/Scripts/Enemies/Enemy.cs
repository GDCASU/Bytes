using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Character
{
    public override void ReceiveDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
            Destroy(gameObject);
    }

    public override void ReceiveHealth(float health)
    {
        Health += health;
        if (Health > MaxHealth)
            Health = MaxHealth;
    }
}
