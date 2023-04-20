/*
 * Author: Cami Lee
 * Date: 14 April 2023
 */
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// add sounds into files with SoundManager.PlaySound(SoundManager.Sound.[sound_name])
// add 3D sounds into files with SoundManager.PlaySound(SoundManager.Sound.[sound_name], GetPosition())

public static class SoundManager {
    public enum Sound {
        // list of sounds needed to be implemented

        // player sounds 
        PlayerWalk,
        PlayerRun,
        PlayerKnifeAttack,
        PlayerJump,
        PlayerLandedJump,
        PlayerKnifeHit,

        // general weapon sounds 
        WeaponSwap,

        // glock sounds
        GlockGunFire,
        GlockReloadCycle,
        GlockActiveReloadSuccess,
        GlockActiveReloadFail,
        GlockCockBackReload,

        // AR sounds
        ARGunFire,
        ARReloadCycle,
        ARActiveReloadSuccess,
        ARActiveReloadFail,
        ARCockBackReload,

        // bullet system sounds

        // secondary fire system sounds

        // rarity system sounds
        DisplayPane,

        // drop / pickup system sounds

        // battery system sounds

        // augmentations sounds
        Dash,
        RocketBlast,
        StompReady,
        StompMotion,
        StompImpact,

        // melee robot sounds
        MeleeDamage,
        MeleeHeadshotDamage,
        MeleeFootsteps,
        MeleeWeapon,
        MeleeDeath,
        MeleeAmbiance,

        // semi-auto robot sounds
        SemiAutoDamage,
        SemiAutoFootsteps,
        SemiAutoGun,
        SemiAutoDeath,
        SemiAutoAmbiance,

        // bomber drone sounds
        BomberDamage,
        BomberFlying,
        BomberExplosion,
        BomberDeath,
        BomberCharge,

        // ballistic drone sounds
        BalliDroneDamage,
        BalliDroneFlying,
        BalliDroneDeath,
        BalliDroneGun,

        // ballistic turret sounds
        BalliTurrDamage,
        BalliTurrGun,
        BalliTurrDeath,
        BalliTurrRotating,

        // map system / map generation sounds 
        Elevator,
        TrialComplete,
        TrialStarted,
        PlayerStats,
        BossDefeat,
        LevelOneAmbiace,

        // environment / interactable objects sounds
        DoorShut,
        DoorShutHigh,
        ChestOpen,
        CrateDamage,
        CrateBreak,
        CanisterExplode,

        // Boss Japanese Mech sounds
    }

    // stores sounds needing a timer
    private static Dictionary<Sound, float> soundTimerDictionary;
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    public static void Initialize() {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerWalk] = 0f;
        soundTimerDictionary[Sound.PlayerRun] = 0f;
        soundTimerDictionary[Sound.GlockGunFire] = 0f;
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        // creates game object sound in 3D space
        if (CanPlaySound(sound)) {
            GameObject soundGameObject = new GameObject("3DSound");
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(sound);
            // audioSource.____ has lots of effects to add to the 3D sounds
            audioSource.Play();

            Object.Destroy(soundGameObject, audioSource.clip.length);
        }
    }

    public static void PlaySound(Sound sound) {
        // creates game object sound
        if (CanPlaySound(sound)) {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }
            
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound sound) {
        switch (sound) {
            default:
                return true;
            case Sound.PlayerWalk:
                if (soundTimerDictionary.ContainsKey(sound)) {
                    float LastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = 0.5f;

                    if (LastTimePlayed + playerMoveTimerMax < Time.time) {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else { return false; }
                }
                else { return true; }
            case Sound.PlayerRun:
                if (soundTimerDictionary.ContainsKey(sound)) {
                    float LastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = 0.3f;

                    if (LastTimePlayed + playerMoveTimerMax < Time.time) {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else { return false; }
                }
                else { return true; }
            case Sound.GlockGunFire:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float LastTimePlayed = soundTimerDictionary[sound];
                    float firingTimerMax = 0.3f;

                    if (LastTimePlayed + firingTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else { return false; }
                }
                else { return true; }
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        // grabs sound file from Sound Assets class
        foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.i.soundAudioClipArray) {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        UnityEngine.Debug.LogError("Sound not found");
        return null;
    }
}
