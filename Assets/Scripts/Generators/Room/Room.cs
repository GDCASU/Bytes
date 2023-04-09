using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    ToBoss,
    Escape
}

public class Room : MonoBehaviour
{
    const string SPAWN_PAD_TAG = "Spawner";

    [HideInInspector] public Vector3 position;
    public bool[,] activeEntranceways;
    public List<GameObject> entranceways;
    [HideInInspector] public List<SpawnPad> spawnPads;

    public RoomShape roomShape;
    [HideInInspector] public RoomType roomType;

    void Awake()
    {
        activeEntranceways = new bool[4, 6];
        PopulateSpawnPadsList(this.transform);
    }

    public void PopulateSpawnPadsList(Transform parent) // Find All SpawnPads in Room
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == SPAWN_PAD_TAG)
            {
                spawnPads.Add(child.gameObject.GetComponent<SpawnPad>());
            }
            if (child.childCount > 0)
            {
                PopulateSpawnPadsList(child);       // Recursive procedure to find objects in children of parent
            }
        }
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
        entranceways[entranceNum].transform.GetChild(0).gameObject.SetActive(false); // Deactivate Wall
        entranceways[entranceNum].transform.GetChild(1).gameObject.SetActive(true); // Activate Entranceway
    }
}