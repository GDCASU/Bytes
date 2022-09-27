using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> entrancewayList;

    private GameObject nextRoom, prevRoom;
    public void SetNext(GameObject room)
    {
        nextRoom = room;
    }
    public void SetPrev(GameObject room)
    {
        prevRoom = room;
    }
    public GameObject GetNext()
    {
        return nextRoom;
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
