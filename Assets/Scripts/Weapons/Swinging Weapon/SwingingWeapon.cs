using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingWeapon : MeleeWeapon
{
    public override void Block(bool isStarting) { }

    public override void Strike()
    {
        throw new System.NotImplementedException();
    }
}
