using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
   private void Awake()
    {
        SoundManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Turret _turretinput = GetComponent<Turret>();
        Player _playerinput = GetComponent<Player>();

        if (true)
        {
            SoundManager.PlaySound(SoundManager.Sound.DangerZone);
        }
        else {
            SoundManager.PlaySound(SoundManager.Sound.LevelOneAmbiance);
        }
    }
}
