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

public class Room : MonoBehaviour
{
    public RoomShape shape;
    public List<GameObject> entrancewayList;
    public List<GameObject> entrancewayList2;

    private bool[] activeEntranceways;

    public Vector3 roomPostion;

    private void Start()
    {
        switch(shape)
        {
            case RoomShape.General:
                activeEntranceways = new bool[6];
                break;
            case RoomShape.Tall:
                activeEntranceways = new bool[12];
                break;
            case RoomShape.Hall:
                activeEntranceways = new bool[12];
                break;
            case RoomShape.Big:
                activeEntranceways = new bool[24];
                break;
            case RoomShape.Boss:
                activeEntranceways = new bool[8];
                break;
        }
    }

    public void ActivateEntrance(int entranceNum)
    {
        if (!entrancewayList[entranceNum].activeInHierarchy)
        {
             entrancewayList[entranceNum].SetActive(true);
        }
    }

    public void ActivateAltEntrance(int entranceNum)
    {
        if (!entrancewayList2[entranceNum].activeInHierarchy)
        {
            entrancewayList2[entranceNum].SetActive(true);
        }
    }
}
