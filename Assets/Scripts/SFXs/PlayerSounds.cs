/*
 * Author: Cami Lee
 * Date: 14 April 2023
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerSounds : MonoBehaviour {
    bool jumping = false;
    float jumpTime = 0f;

    // Update is called once per frame
    private void Awake() {
        SoundManager.Initialize();
    }
    void Update() {
        PlayerInput _playerinput = GetComponent<PlayerInput>();

        /*if (_playerinput.AugmentationOne) //augmentation one (aka dash)
        {
            SoundManager.PlaySound(SoundManager.Sound.Dash);
        }
        else if (_playerinput.AugmentationTwo) //augmentation two (aka rocket boost)
        {
            SoundManager.PlaySound(SoundManager.Sound.RocketBlast);
        }
        else if (_playerinput.AugmentationThree) //augmentation three (aka stomp)
        {
            SoundManager.PlaySound(SoundManager.Sound.StompMotion);
        }*/
        if (_playerinput.IsJumpPressed) //jumping
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
        else if (_playerinput.MoveVector.magnitude > 0 && !jumping) //walking
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerWalk);
        }

        else if (_playerinput.IsSprintPressed) //running
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerRun);
        }
    }

    private Vector3 GetPosition()
    {
        throw new NotImplementedException();
    }
}
