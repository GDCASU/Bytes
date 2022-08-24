using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Enemy
{
    public override void ReceiveDamage(float damage)
    {
        print(name + " has received " + damage + " damage");
    }

    public override void ReceiveHealth(float health)
    {
        print(name + " has received " + health + "health");
    }
}
