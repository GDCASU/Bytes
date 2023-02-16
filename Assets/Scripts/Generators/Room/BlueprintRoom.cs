using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.WSA;

public enum RoomShape
{
    General,
    Tall,
    Hall,
    Big,
    Boss
}

public class BlueprintRoom // Psudo Room used for room generation algorithm
{
    public Vector3 position;
    public RoomShape shape;
    public bool[] activeEntranceways;

    public BlueprintRoom(Vector3 postion)
    {
        position = postion;
        activeEntranceways = new bool[24];
    }
}
