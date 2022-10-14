/*
 * Author: Cristion Dominguez
 * Date: 6 Oct. 2022
 */

public class OldPlayer : Character
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

    protected override void OnEnable() => weaponHandler.Dev_Enable();

    void OnDisable() => weaponHandler.Dev_Disable();

    public override void ReceiveDamage(float damage)
    {
        
    }

    public override void ReceiveHealth(float health)
    {
        
    }
}
