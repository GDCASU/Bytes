using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomShape
{
    GeneralRoom,
    HallRoom,
    TallRoom,
    BigRoom,
};

public enum RoomType
{
    General,
    Start,
    Augmentation,
    Keycard,
    Trial,
    ToBoss
}

public class Room : MonoBehaviour
{
    [HideInInspector] public Vector3 position;
    public bool[,] activeEntranceways;
    public List<GameObject> entranceways;

    public RoomShape roomShape;
    [HideInInspector] public RoomType roomType;

    void Awake()
    {
        activeEntranceways = new bool[4, 6];
    }

    public void ActivateAllEntranceways()
    {
        int enListIdx = 0;              // iterator for activeEntranceway List
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                enListIdx = (i * 6) + j;
                if (activeEntranceways[i, j] == true)   // Activate entrance if true in activeEntranceway List
                    ActivateEntranceway(enListIdx);
            }
        }
    }

    public void CopyBlueprintArrayFlags(bool[] blueArray, int roomOriginIndex)
    {
        for (int i = 0; i < blueArray.Length; i++) // iterate through all six faces of the Blueprint's flag array
        {
            activeEntranceways[roomOriginIndex, i] = blueArray[i]; // Copy into room array respectively
        }
    }

    private void ActivateEntranceway(int entranceNum)
    {
        //if (entranceways[entranceNum] != null)
        {
            entranceways[entranceNum].transform.GetChild(0).gameObject.SetActive(false); // Deactivate Wall
            entranceways[entranceNum].transform.GetChild(1).gameObject.SetActive(true); // Activate Entranceway
        }
        //else
        //Debug.Log("Error: Could not activate entrance; entrance is null.");
    }
}