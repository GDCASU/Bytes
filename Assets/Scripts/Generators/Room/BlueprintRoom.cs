using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.WSA;

public class BlueprintRoom // Psudo Room used for room generation algorithm
{
    public Vector3 position;
    public bool[] activeEntranceways;

    public GameObject RoomGadget;

    public BlueprintRoom(Vector3 postion)
    {
        position = postion;
        activeEntranceways = new bool[6];
    }
}
