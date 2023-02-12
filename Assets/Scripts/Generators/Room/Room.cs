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
    public Room(Vector3 postion)
    {
        roomPos = postion;
    }

    public Vector3 roomPos;

    public RoomShape shape;
    public List<GameObject> entrancewayList;
    public List<GameObject> entrancewayList2;

    private bool[] activeEntranceways;

    public Vector3 roomPostion;

    private void Start()
    {
        activeEntranceways = new bool[24];
    }

    public void ActivateEntrance(int entranceNum)
    {
        if (!entrancewayList[entranceNum].activeInHierarchy)
        {
             entrancewayList[entranceNum].SetActive(true);
        }
    }
}
