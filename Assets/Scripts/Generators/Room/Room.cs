using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.WSA;

public enum RoomShape
{
    General,
    Tall,
    Hall
}

public class Room : MonoBehaviour
{
    public RoomShape shape;
    public List<GameObject> entrancewayList;
    public List<GameObject> entrancewayList2;



    public GameObject prevRoom;
    public void SetPrev(GameObject room)
    {
        prevRoom = room;
    }
    public GameObject GetPrev()
    {
        return prevRoom;
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
