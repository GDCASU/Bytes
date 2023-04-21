using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        ReloadAbility _reloadinput = GetComponent<ReloadAbility>();

        if (_reloadinput.IsReloading)
        { //reloading
            SoundManager.PlaySound(SoundManager.Sound.GlockReloadCycle);
            if (true)
            { //success
                SoundManager.PlaySound(SoundManager.Sound.GlockActiveReloadSuccess);
                UnityEngine.Debug.Log("Glock Success");
            }
            else
            { //failure
                SoundManager.PlaySound(SoundManager.Sound.GlockActiveReloadFail);
            }
        }
        else if (_weaponinput.IsShooting) { //shooting
            SoundManager.PlaySound(SoundManager.Sound.GlockGunFire);
        }
        else if (_weaponinput.Handler.IsSwitching) //switching weapons
        {
            SoundManager.PlaySound(SoundManager.Sound.WeaponSwap);
        }
    }

}
