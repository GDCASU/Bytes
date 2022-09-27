using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEditor.SearchService;
using UnityEngine;
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

    [HideInInspector] public float minX, maxX, minY, maxY, minZ, maxZ;

    //private List<Vector3> roomPositionsList = new List<Vector3>();
    [SerializeField] List<GameObject> roomList = new List<GameObject>();

    #region Blueprint
    void GenBlueprint()
    {
        Vector3 curPos = Vector3.zero;
        GenerateBlueprintRoom(curPos, null);
        int backtrackCounter = 1;

        while (roomList.Count < maxRooms)
        {
            GameObject prevRoom = new GameObject();
            prevRoom = roomList[roomList.Count - backtrackCounter];

            switch (UnityEngine.Random.Range(1, 7)) // Choosing position of next room
            {
                case 1: curPos += Vector3.up * cellSize; break; // (0, cellSize, 0) * Cell Unit Size
                case 2: curPos += Vector3.down * cellSize; break; // (0, cellSize, 0) * Cell Unit Size
                case 3: curPos += Vector3.right * cellSize; break; // (cellSize, 0, 0) * Cell Unit Size
                case 4: curPos += Vector3.left * cellSize; break; // (cellSize, 0, 0) * Cell Unit Size
                case 5: curPos += Vector3.forward * cellSize; break; // (0, 0, cellSize) * Cell Unit Size
                case 6: curPos += Vector3.back * cellSize; break; // (0, 0, cellSize) * Cell Unit Size
                default: curPos += Vector3.forward * cellSize; break; // Just in case of bug
            }

            bool inRoomList = false;
            for (int i = roomList.Count - 1; i >= 0; i--) // Looping back through list and Checking for collisions with other rooms
            {
                if (Vector3.Equals(curPos, roomList[i].GetComponent<Room>().transform.position))
                {
                    inRoomList = true;
                    backtrackCounter++;
                    break;
                }
            }
            if (!inRoomList)
            {
                GenerateBlueprintRoom(curPos, prevRoom);
                backtrackCounter = 1;
            }
        }
    }

    private void GenerateBlueprintRoom(Vector3 roomPosition, GameObject prevRoom)
    {
        GameObject genRoom = Instantiate(bluePrintPrefab, roomPosition, Quaternion.identity) as GameObject;
        genRoom.name = $"{bluePrintPrefab.name} [{roomList.Count}]";
        genRoom.transform.SetParent(transform);
        roomList.Add(genRoom);
        genRoom.GetComponent<Room>().SetPrev(prevRoom);
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
    /*
    void GenerateJRoom(int roomNum, int index)
    {
        switch (roomNum)
        {
            case 1:
                Debug.Log("J Room Generated.");
                Vector3 curRoomPos = roomPositionsList[index]; // Generate 'G' Room
                GameObject genRoom = Instantiate(jPrefab, curRoomPos, Quaternion.identity) as GameObject;
                genRoom.name = $"{jPrefab.name} [{index}]";
                genRoom.transform.SetParent(transform);
                roomList.Add(genRoom);
                break;

        }
    }
    */
    #endregion

    #region Entranceways
    /*
    void ActivateEntranceways()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            for (int j = 0; j < roomList.Count; j++)
            {
                Room iRoom = roomList[i].GetComponent<Room>();
                Room jRoom = roomList[j].GetComponent<Room>();

                Vector3 iRoomPos = roomPositionsList[i];
                Vector3 jRoomPos = roomPositionsList[j];

                if (iRoomPos == jRoomPos + (Vector3.right * 5)) // if room b4 the current room is on the right
                {
                    jRoom.ActivateEntrance(0); // Activate Entrance E0 for current room
                    iRoom.ActivateEntrance(2); // Activate Entrance E2 for prev. room
                }
                if (iRoomPos == jRoomPos + (Vector3.forward * 5)) // if room b4 the current room is on the forward
                {
                    jRoom.ActivateEntrance(1); // Activate Entrance E1
                    iRoom.ActivateEntrance(3); // Activate Entrance E2 for prev. room
                }
                if (iRoomPos == jRoomPos + (Vector3.left * 5)) // if room b4 the current room is on the left
                {
                    jRoom.ActivateEntrance(2); // Activate Entrance E2
                    iRoom.ActivateEntrance(0); // Activate Entrance E2 for prev. room
                }
                if (iRoomPos == jRoomPos + (Vector3.back * 5)) // if room b4 the current room is on the back
                {
                    jRoom.ActivateEntrance(3); // Activate Entrance E3
                    iRoom.ActivateEntrance(1); // Activate Entrance E2 for prev. room
                }
                if (iRoomPos == jRoomPos + (Vector3.up * 5)) // if room b4 the current room is on the up
                {
                    jRoom.ActivateEntrance(4); // Activate Entrance E4
                    iRoom.ActivateEntrance(5); // Activate Entrance E2 for prev. room
                }
                if (iRoomPos == jRoomPos + (Vector3.down * 5)) // if room b4 the current room is on the down
                {
                    jRoom.ActivateEntrance(5); // Activate Entrance E5
                    iRoom.ActivateEntrance(4); // Activate Entrance E2 for prev. room
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
                GenRooms();
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
                Debug.Log("Entranceways are currently a work in progress.");
            }
            else
                Debug.Log("Error: Generate the rooms before activating entrances.");
        }
    }
}
