using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<GameObject> entrancewayList;
    public void ActivateEntrance(int entranceNum)
    {
        if (!entrancewayList[entranceNum].activeInHierarchy)
        {
            entrancewayList[entranceNum].SetActive(true);
        }
    }
}
