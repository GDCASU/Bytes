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
        Vector3 vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0,
            Input.GetAxisRaw("Vertical"));

        if (_turretinput.GetPosition() == vector + _playerinput.GetPosition())
        {
            SoundManager.PlaySound(SoundManager.Sound.DangerZone);
        }
        else {
            SoundManager.PlaySound(SoundManager.Sound.LevelOneAmbiance);
        }
    }
}
