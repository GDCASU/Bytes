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
using UnityEditor.Rendering.LookDev;
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
        Player _input = GetComponent<Player>();

        if (_input.IsJumpPressed) //jumping
        { 
            SoundManager.PlaySound(SoundManager.Sound.PlayerJump);
            UnityEngine.Debug.Log("Jump");
            jumpTime = Time.time;
            jumping = true;
        }
        else if (jumping == true && Time.time > jumpTime + 0.6) //landed jump
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerLandedJump);
            UnityEngine.Debug.Log("Landed Jump" + jumping);
            jumping = false;
        }
        else if (_input.MoveVector.magnitude > 0) //walking
        { 
            SoundManager.PlaySound(SoundManager.Sound.PlayerWalk);
            UnityEngine.Debug.Log("Walk");
        }

        else if (_input.IsSprintPressed) //running
        { 
            SoundManager.PlaySound(SoundManager.Sound.PlayerRun);
            UnityEngine.Debug.Log("Run");
        }
    }

    private Vector3 GetPosition()
    {
        throw new NotImplementedException();
    }
}
