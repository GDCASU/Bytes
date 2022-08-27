using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    WeaponHandler weaponHandler;
    PlayerUI ui;

    void Awake()
    {
        ui = GetComponent<PlayerUI>();
        weaponHandler = GetComponent<WeaponHandler>();
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
            ui.RestrictCursor();
        else
            ui.FreeCursor();
    }

    protected override void OnEnable() => weaponHandler.Dev_OnEnable();

    void OnDisable() => weaponHandler.Dev_OnDisable();

    public override void ReceiveDamage(float damage)
    {
        
    }

    public override void ReceiveHealth(float health)
    {
        
    }
}
