using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMusic : MonoBehaviour
{
    int musicNumber = 0;
    float initial;
    float initial2;
    float final;
    bool partOne = true;
    bool partTwo = false;
    bool partThree = false;
    bool isPlaying = false;
    private static Dictionary<AudioClip, int> musicNumberDictionary;

    private void Awake()
    {
        SoundManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (initial + 52 < Time.time)
        {
            partOne = false;
            partTwo = true;
            isPlaying = false;
            SoundManager.StopSound(SoundManager.Sound.Elevator, GetPosition());
            initial2 = Time.time;
        }
        else if (initial2 + 39 < Time.time) {
            SoundManager.StopSound(SoundManager.Sound.Elevator2, GetPosition());
            initial2 = Time.time;
        }
        else if (final + 13 < Time.time)
        {
            partTwo = false;
            partThree = true;
            isPlaying = false;

        }

        if (partOne && !isPlaying) {
            SoundManager.PlaySound(SoundManager.Sound.Elevator, GetPosition());
            initial = Time.time;
            isPlaying = true;
        }
        if (partTwo && !isPlaying) {
            SoundManager.PlaySound(SoundManager.Sound.Elevator2, GetPosition());
            initial = Time.time;
            isPlaying = true;
        }
        if (partThree && !isPlaying) {
            SoundManager.PlaySound(SoundManager.Sound.Elevator2, GetPosition());
            isPlaying = true;
        }

    }

    private Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }


}
