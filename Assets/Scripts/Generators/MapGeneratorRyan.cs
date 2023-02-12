using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class MapGeneratorRyan : MonoBehaviour
{
    [Header("Grid Settings")]
    public float cellSize = 10;

    [Header("Room Prefabs")]
    public GameObject bluePrintPrefab;
    public GameObject gPrefab;
    public GameObject tPrefab;
    public GameObject hPrefab;
    public GameObject lPrefab;
    public GameObject jPrefab;
    public GameObject bPrefab;

    [Header("Conditions")]
    public int maxRooms;
    public int tRoomSpawnChance;
    public int hRoomSpawnChance;

    [SerializeField] List<GameObject> roomList = new List<GameObject>();
    [SerializeField] int[] prevRooms;

    private void Start()
    {
        prevRooms = new int[maxRooms];
    }

    #region Blueprint
    void GenBlueprint()
    {
        Vector3 curPos = Vector3.zero;
        GenerateBlueprintRoom(curPos);
        prevRooms[0] = -1;
        int prevRoomIdx = 0;

        while (roomList.Count < maxRooms)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].transform.position == curPos)
                {
                    prevRoomIdx = i;
                }
            }

            switch (UnityEngine.Random.Range(1, 7)) // Choosing position of next room
            {
                case 1: curPos += Vector3.right * cellSize; break; // (0, cellSize, 0) * Cell Unit Size
                case 2: curPos += Vector3.left * cellSize; break; // (0, -cellSize, 0) * Cell Unit Size
                case 3: curPos += Vector3.forward * cellSize; break; // (cellSize, 0, 0) * Cell Unit Size
                case 4: curPos += Vector3.back * cellSize; break; // (-cellSize, 0, 0) * Cell Unit Size
                case 5: curPos += Vector3.up * cellSize; break; // (0, 0, cellSize) * Cell Unit Size
                case 6: curPos += Vector3.down * cellSize; break; // (0, 0, -cellSize) * Cell Unit Size
            }

            bool inRoomList = false;
            for (int i = roomList.Count - 1; i >= 0; i--) // Looping back through list and Checking for collisions with other rooms
            {
                if (Vector3.Equals(curPos, roomList[i].GetComponent<Room>().transform.position))
                {
                    inRoomList = true;
                    break;
                }
            }
            if (!inRoomList)
            {
                GenerateBlueprintRoom(curPos);
                prevRooms[roomList.Count - 1] = prevRoomIdx;
            }
        }
    }

    private void GenerateBlueprintRoom(Vector3 roomPosition)
    {
        GameObject genRoom = Instantiate(bluePrintPrefab, roomPosition, Quaternion.identity) as GameObject;
        genRoom.name = $"{bluePrintPrefab.name} [{roomList.Count}]";
        genRoom.transform.SetParent(transform);
        roomList.Add(genRoom);
    }
    #endregion

    #region Rooms
    void GenRooms() // Generate Rooms
    {
        GameObject firstRoom = Instantiate(gPrefab, roomList[0].transform.position, Quaternion.identity) as GameObject; // Generate Start Room
        firstRoom.name = $"{gPrefab.name} [{0}]";
        firstRoom.transform.SetParent(transform);
        roomList[0] = firstRoom;

        for (int i = 1; i < roomList.Count; i++)
        {
            if (i < roomList.Count - 2)
            {
                int roomChanceRoll = UnityEngine.Random.Range(1, 101);
                bool TRoomCondidion = (roomList[i].transform.position.x == roomList[i + 1].transform.position.x 
                                            && roomList[i].transform.position.z == roomList[i + 1].transform.position.z
                                            && Mathf.Abs(roomList[i].transform.position.y - roomList[i + 1].transform.position.y) <= cellSize);
                bool HRoomCondition = (roomList[i].transform.position.x == roomList[i + 1].transform.position.x 
                                            && roomList[i].transform.position.y == roomList[i + 1].transform.position.y
                                            && Mathf.Abs(roomList[i].transform.position.z - roomList[i + 1].transform.position.z) <= cellSize);

                if (roomChanceRoll <= tRoomSpawnChance && TRoomCondidion)
                {
                    if (roomList[i].transform.position.y < roomList[i + 1].transform.position.y)
                    {
                        GenerateTRoom(1, i);
                    }
                    else
                    {
                        GenerateTRoom(2, i);
                    }
                    i++;
                }
                else if (roomChanceRoll <= hRoomSpawnChance && HRoomCondition)
                {
                    if (roomList[i].transform.position.z < roomList[i + 1].transform.position.z)
                    {
                        GenerateHRoom(1, i);
                    }
                    else
                    {
                        GenerateHRoom(2, i);
                    }
                    i++;
                }
                else
                {
                    GenerateGRoom(1, i);
                }
            }
            else
            {
                GenerateGRoom(1, i);
            }
        }
    }

    void GenerateGRoom(int roomNum, int index)
    {
        switch (roomNum)
        {
            case 1:
                Vector3 curRoomPos = roomList[index].transform.position; // Generate 'G' Room
                GameObject genRoom = Instantiate(gPrefab, curRoomPos, Quaternion.identity) as GameObject;
                genRoom.name = $"{gPrefab.name} [{index}]";
                genRoom.transform.SetParent(transform);
                roomList[index] = genRoom;
                break;

        }
    }

    void GenerateTRoom(int roomNum, int index)
    {
        Vector3 curRoomPos = roomList[index].transform.position;
        GameObject genRoom = new GameObject();

        switch (roomNum)
        {
            case 1:
                genRoom = Instantiate(tPrefab, curRoomPos, Quaternion.identity) as GameObject;
                genRoom.name = $"{tPrefab.name} [{index}]";
                genRoom.transform.SetParent(transform);
                roomList[index] = genRoom;
                roomList[index + 1] = genRoom;
               break;
            case 2:
                curRoomPos.y = curRoomPos.y - cellSize;
                genRoom = Instantiate(tPrefab, curRoomPos, Quaternion.identity) as GameObject;
                genRoom.name = $"{tPrefab.name} [{index}]";
                genRoom.transform.SetParent(transform);
                roomList[index] = genRoom;
                roomList[index + 1] = genRoom;
                break;

            default:
                Debug.Log("Error: When trying to generate TRoom. Default case selected");
                break;
        }
    }

    void GenerateHRoom(int roomNum, int index)
    {
        Vector3 curRoomPos = roomList[index].transform.position;
        GameObject genRoom = new GameObject();

        switch (roomNum)
        {
            case 1:
                genRoom = Instantiate(hPrefab, curRoomPos, Quaternion.identity) as GameObject;
                genRoom.name = $"{hPrefab.name} [{index}]";
                genRoom.transform.SetParent(transform);
                roomList[index] = genRoom;
                roomList[index + 1] = genRoom;
                break;
            case 2:
                curRoomPos.z = curRoomPos.z - cellSize;
                genRoom = Instantiate(hPrefab, curRoomPos, Quaternion.identity) as GameObject;
                genRoom.name = $"{hPrefab.name} [{index}]";
                genRoom.transform.SetParent(transform);
                roomList[index] = genRoom;
                roomList[index + 1] = genRoom;
                break;

            default:
                Debug.Log("Error: When trying to generate TRoom");
                break;

        }
    }
    #endregion

    #region Entranceways
    /*
    void ActivateEntranceways()
    {
        for(int currRoomIdx = 1; currRoomIdx < roomList.Count; currRoomIdx++)
        {
            Room currentRoom = roomList[currRoomIdx].GetComponent<Room>();
            Room prevRoom = currentRoom.prevRoom.GetComponent<Room>();

            Vector3 currRoomPos = transform.position;
            Vector3 prevRoomPos = prevRoom.transform.position;

            // Activate prev room entrance
            // Activate this room entrance
            if (currentRoom.shape == RoomShape.General)
            {
                // open E0 & E1
                if (currRoomPos.x < prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (+x ,=y, =z)
                {
                    currentRoom.ActivateEntrance(0);
                    prevRoom.ActivateEntrance(1);
                }
                // open E1 & E0
                else if (currRoomPos.x > prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (-x ,=y, =z)
                {
                    currentRoom.ActivateEntrance(1);
                    prevRoom.ActivateEntrance(0);
                }
                // open E2 & E3
                else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z < prevRoomPos.z) // (=x ,=y, +z)
                {
                    currentRoom.ActivateEntrance(2);
                    prevRoom.ActivateEntrance(3);
                }
                // open E3 & E2
                else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z > prevRoomPos.z) // (=x ,=y, -z)
                {
                    currentRoom.ActivateEntrance(3);
                    prevRoom.ActivateEntrance(2);
                }
                // open E4 & E5
                else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y < prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,+y, =z)
                {
                    currentRoom.ActivateEntrance(4);
                    prevRoom.ActivateEntrance(5);
                }
                // open E5 & E4
                else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y > prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,-y, +z)
                {
                    currentRoom.ActivateEntrance(5);
                    prevRoom.ActivateEntrance(4);
                }
            }

            if (currentRoom.shape == RoomShape.Tall)
            {
                if (linkedRooms[currRoomIdx] == 1 && linkedRooms[currRoomIdx - 1] == 1) // if curr tall = next cell && prev tall == next cell
                {
                    // open E0 & E1
                    if (currRoomPos.x < prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (+x ,=y, =z)
                    {
                        currentRoom.ActivateAltEntrance(0);
                        prevRoom.ActivateAltEntrance(1);
                    }
                    // open E1 & E0
                    else if (currRoomPos.x > prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (-x ,=y, =z)
                    {
                        currentRoom.ActivateAltEntrance(1);
                        prevRoom.ActivateAltEntrance(0);
                    }
                    // open E2 & E3
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z < prevRoomPos.z) // (=x ,=y, +z)
                    {
                        currentRoom.ActivateAltEntrance(2);
                        prevRoom.ActivateAltEntrance(3);
                    }
                    // open E3 & E2
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z > prevRoomPos.z) // (=x ,=y, -z)
                    {
                        currentRoom.ActivateAltEntrance(3);
                        prevRoom.ActivateAltEntrance(2);
                    }
                    // open E4 & E5
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y < prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,+y, =z)
                    {
                        currentRoom.ActivateAltEntrance(4);
                        prevRoom.ActivateAltEntrance(5);
                    }
                    // open E5 & E4
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y > prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,-y, +z)
                    {
                        currentRoom.ActivateAltEntrance(5);
                        prevRoom.ActivateAltEntrance(4);
                    }
                }
                else if (linkedRooms[currRoomIdx] == 1) // if curr tall = next cell && prev tall == first cell
                {
                    // open E0 & E1
                    if (currRoomPos.x < prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (+x ,=y, =z)
                    {
                        currentRoom.ActivateAltEntrance(0);
                        prevRoom.ActivateEntrance(1);
                    }
                    // open E1 & E0
                    else if (currRoomPos.x > prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (-x ,=y, =z)
                    {
                        currentRoom.ActivateAltEntrance(1);
                        prevRoom.ActivateEntrance(0);
                    }
                    // open E2 & E3
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z < prevRoomPos.z) // (=x ,=y, +z)
                    {
                        currentRoom.ActivateAltEntrance(2);
                        prevRoom.ActivateEntrance(3);
                    }
                    // open E3 & E2
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z > prevRoomPos.z) // (=x ,=y, -z)
                    {
                        currentRoom.ActivateAltEntrance(3);
                        prevRoom.ActivateEntrance(2);
                    }
                    // open E4 & E5
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y < prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,+y, =z)
                    {
                        currentRoom.ActivateAltEntrance(4);
                        prevRoom.ActivateEntrance(5);
                    }
                    // open E5 & E4
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y > prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,-y, +z)
                    {
                        currentRoom.ActivateAltEntrance(5);
                        prevRoom.ActivateEntrance(4);
                    }
                }
                else if (linkedRooms[currRoomIdx - 1] == 1) // if curr tall = first cell && prev tall == next cell
                {
                    // open E0 & E1
                    if (currRoomPos.x < prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (+x ,=y, =z)
                    {
                        currentRoom.ActivateEntrance(0);
                        prevRoom.ActivateAltEntrance(1);
                    }
                    // open E1 & E0
                    else if (currRoomPos.x > prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (-x ,=y, =z)
                    {
                        currentRoom.ActivateEntrance(1);
                        prevRoom.ActivateAltEntrance(0);
                    }
                    // open E2 & E3
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z < prevRoomPos.z) // (=x ,=y, +z)
                    {
                        currentRoom.ActivateEntrance(2);
                        prevRoom.ActivateAltEntrance(3);
                    }
                    // open E3 & E2
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z > prevRoomPos.z) // (=x ,=y, -z)
                    {
                        currentRoom.ActivateEntrance(3);
                        prevRoom.ActivateAltEntrance(2);
                    }
                    // open E4 & E5
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y < prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,+y, =z)
                    {
                        currentRoom.ActivateEntrance(4);
                        prevRoom.ActivateAltEntrance(5);
                    }
                    // open E5 & E4
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y > prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,-y, +z)
                    {
                        currentRoom.ActivateEntrance(5);
                        prevRoom.ActivateAltEntrance(4);
                    }
                }
                else // if curr tall = first cell && prev tall == first cell
                {
                    // open E0 & E1
                    if (currRoomPos.x < prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (+x ,=y, =z)
                    {
                        currentRoom.ActivateEntrance(0);
                        prevRoom.ActivateEntrance(1);
                    }
                    // open E1 & E0
                    else if (currRoomPos.x > prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (-x ,=y, =z)
                    {
                        currentRoom.ActivateEntrance(1);
                        prevRoom.ActivateEntrance(0);
                    }
                    // open E2 & E3
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z < prevRoomPos.z) // (=x ,=y, +z)
                    {
                        currentRoom.ActivateEntrance(2);
                        prevRoom.ActivateEntrance(3);
                    }
                    // open E3 & E2
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y == prevRoomPos.y && currRoomPos.z > prevRoomPos.z) // (=x ,=y, -z)
                    {
                        currentRoom.ActivateEntrance(3);
                        prevRoom.ActivateEntrance(2);
                    }
                    // open E4 & E5
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y < prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,+y, =z)
                    {
                        currentRoom.ActivateEntrance(4);
                        prevRoom.ActivateEntrance(5);
                    }
                    // open E5 & E4
                    else if (currRoomPos.x == prevRoomPos.x && currRoomPos.y > prevRoomPos.y && currRoomPos.z == prevRoomPos.z) // (=x ,-y, +z)
                    {
                        currentRoom.ActivateEntrance(5);
                        prevRoom.ActivateEntrance(4);
                    }
                }
            }
        }
    }
    */
    #endregion

    bool alreadyGBlue = false;
    bool alreadyGRoom = false;
    void OnGUI()
    {
        if (GUILayout.Button("Reload Scene"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (GUILayout.Button("Generate Blueprint"))
        {
            if (!alreadyGBlue)
            {
                alreadyGBlue = true;
                GenBlueprint();
            }
            else
                Debug.Log("Error: Already generated Blueprint");
        }

        if (GUILayout.Button("Generate Rooms"))
        {
            if (alreadyGBlue && !alreadyGRoom)
            {
                alreadyGRoom = true;
                //GenRooms();
            }
            else if (!alreadyGBlue)
                Debug.Log("Error: Blueprint needs to be generated before the rooms.");
            else
                Debug.Log("Error: Already Generated Rooms.");
        }

        if (GUILayout.Button("Activate Entranceways"))
        {
            if (alreadyGRoom && alreadyGBlue)
            {
                alreadyGRoom = true;
                //ActivateEntranceways();
            }
            else
                Debug.Log("Error: Generate the rooms before activating entrances.");
        }
    }
}
