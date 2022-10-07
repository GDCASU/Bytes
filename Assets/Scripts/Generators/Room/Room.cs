using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
