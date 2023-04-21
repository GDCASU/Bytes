using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUISFXs : MonoBehaviour
{
    bool jumping;
    float jumpTime;

    // Update is called once per frame
    private void Awake()
    {
        SoundManager.Initialize();
    }
    void Update()
    {
        Player _playerinput = GetComponent<Player>();
        DashAugmentation _dashinput = GetComponent<DashAugmentation>();

        if (_dashinput.IsDashing) //dash move
        {
            SoundManager.PlaySound(SoundManager.Sound.Dash);
        }
        else if (_playerinput.IsJumpPressed) //jumping
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerJump);
            jumpTime = Time.time;
            jumping = true;
        }
        else if (jumping == true && Time.time > jumpTime + 0.6) //landed jump
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerLandedJump);
            jumping = false;
        }
        else if (_playerinput.MoveVector.magnitude > 0) //walking
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerWalk);
        }

        else if (_playerinput.IsSprintPressed) //running
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerRun);
        }
    }

}

