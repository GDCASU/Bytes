using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RoomShape
{
    GeneralRoom,
    HallRoom,
    TallRoom,
    BigRoom,
};

enum RoomType
{
    General,
    Start,
    Keycard,
    Trail,
    Boss,
    Escape
}

public class Room : MonoBehaviour
{
    public Vector3 position;
    public bool[] activeEntranceways;
    public List<GameObject> entranceways;

    [SerializeField] private RoomShape roomShape;
    [SerializeField] private RoomType roomType;

    private void Start()
    {
        activeEntranceways = new bool[24];
    }

    public void ActivateAllEntranceways()
    {
        for (int i = 0; i < activeEntranceways.Length; i++)
        {
            if (activeEntranceways[i] == true)
            {
                ActivateEntranceway(i);
            }
        }
    }

    private void ActivateEntranceway(int entranceNum)
    {
        entranceways[entranceNum].transform.GetChild(0).gameObject.SetActive(false); // Deactivate Wall
        entranceways[entranceNum].transform.GetChild(1).gameObject.SetActive(false); // Activate Entranceway
    }
}
