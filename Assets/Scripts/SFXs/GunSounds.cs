using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSounds : MonoBehaviour
{
    // Update is called once per frame
    private void Awake()
    {
        SoundManager.Initialize();
    }

    void Update()
    {
        Weapon _weaponinput = GetComponent<Weapon>();
        WeaponHandler _weaponhandlerinput = GetComponent<WeaponHandler>();
        ReloadAbility _reloadinput = GetComponent<ReloadAbility>();

        if (_weaponinput.IsShooting) { //shooting
            SoundManager.PlaySound(SoundManager.Sound.GlockGunFire);
        }

        else if (_reloadinput.IsReloading) { //reloading
            SoundManager.PlaySound(SoundManager.Sound.GlockReloadCycle);
            if (true) { //success
                SoundManager.PlaySound(SoundManager.Sound.GlockActiveReloadSuccess);
                UnityEngine.Debug.Log("Glock Success");
            }
            else { //failure
                SoundManager.PlaySound(SoundManager.Sound.GlockActiveReloadFail);
            }
        }

        if (_weaponhandlerinput.IsSwitching) //switching weapons
        {
            SoundManager.PlaySound(SoundManager.Sound.WeaponSwap);
        }
    }

}
