using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public float cellSize = 10;

    [Header("Room Prefabs")]
    public GameObject bluePrintPrefab;
    public List<GameObject> gPrefab;
    public List<GameObject> tPrefab;
    public List<GameObject> hPrefab;
    public List<GameObject> bPrefab;

    [Header("Trails")]
    public List<BlueprintRoom> masterTrail = new List<BlueprintRoom>(); // All trails combined
    public List<BlueprintRoom> mainTrail = new List<BlueprintRoom>();
    public List<BlueprintRoom> augmentationTrail = new List<BlueprintRoom>();
    public List<BlueprintRoom> keycardTrail = new List<BlueprintRoom>();
    public List<BlueprintRoom> trialTrail = new List<BlueprintRoom>();
    public List<BlueprintRoom> bossTrail = new List<BlueprintRoom>();

    [Header("Conditions")]
    
    public int mainTrailMaxRooms = 10;
    public int augmentationTrailMaxRooms = 1;
    public int keycardTrailMaxRooms = 2;
    public int trialTrailMaxRooms = 2;
    public int bossTrailMaxRooms = 2;

    int roomFaces = 6;
    int maxRooms = 20;
    int entrFlagIdx = 0;
    void Start()
    {
        maxRooms = mainTrailMaxRooms + augmentationTrailMaxRooms + keycardTrailMaxRooms + trialTrailMaxRooms + bossTrailMaxRooms;

        Procedure();
    }

    void Procedure()
    {
        RandomWalker(mainTrailMaxRooms, Vector3.zero, mainTrail, null); // Main Trail to boss
    }

    #region RandomWalker
    void RandomWalker(int maxRooms, Vector3 startingPos, List<BlueprintRoom> trail, BlueprintRoom startingRoom)
    {
        Vector3 curPos = startingPos; // Set the position of the starting room
        BlueprintRoom curRoom = null;
        Vector3 tempPos; // new postion to be choosen

        if (startingRoom == null)
        {
            BlueprintRoom newRoom = new BlueprintRoom(curPos);
            trail.Add(newRoom);
            masterTrail.Add(newRoom);
            curRoom = newRoom;
        }

        int failedAttempts = 0;
        while (trail.Count < maxRooms)
        {
            tempPos = curPos;
            switch (UnityEngine.Random.Range(1, 7)) // Choosing position of next room
            {
                case 1: tempPos += Vector3.right * cellSize; // E0 (cellSize, 0, 0) * Cell Unit Size
                    entrFlagIdx = 0;
                    break;
                case 2: tempPos += Vector3.left * cellSize; // E1 (-cellSize, 0, 0) * Cell Unit Size
                    entrFlagIdx = 1;
                    break;
                case 3: tempPos += Vector3.forward * cellSize; // E2 (0, 0, cellSize) * Cell Unit Size
                    entrFlagIdx = 2;
                    break;
                case 4: tempPos += Vector3.back * cellSize; // E3 (0, 0, -cellSize) * Cell Unit Size
                    entrFlagIdx = 3;
                    break;
                case 5: tempPos += Vector3.up * cellSize;  // E4 (0, cellSize, 0) * Cell Unit Size
                    entrFlagIdx = 4;
                    break;
                case 6: tempPos += Vector3.down * cellSize; // E5 (0, -cellSize, 0) * Cell Unit Size
                    entrFlagIdx = 5;
                    break; 
            }

            bool inRoomList = false;
            BlueprintRoom conflictedRoom = null;
            foreach(BlueprintRoom room in masterTrail) // Check master trail for conflicting rooms
            {
                if (Vector3.Equals(tempPos, room.position)) // Test Failed
                {
                    conflictedRoom = room;
                    inRoomList = true;
                    failedAttempts++;
                    break;
                }
            }

            if (!inRoomList) // Test Passed
            {
                curPos = tempPos; // Change Current Position to new position

                BlueprintRoom newRoom = new BlueprintRoom(curPos);
                FlagDoorways(newRoom, curRoom, entrFlagIdx);

                curRoom = newRoom;
                trail.Add(newRoom);
                masterTrail.Add(newRoom);
            }

            if (failedAttempts > 6) // If failed backtrack
            {
                curPos = tempPos;
                curRoom = conflictedRoom;
            }
        }
    }

    void FlagDoorways(BlueprintRoom newRoom, BlueprintRoom prevRoom,  int entrFlagIdx) // Flag the entranceways to be activated in each room
    {
        if (entrFlagIdx % 2 == 0) // If choosen an even numbered side (E4) then set opposite (E3) to true
            newRoom.activeEntranceways[entrFlagIdx + 1] = true;
        else // If choosen an odd numbered side (E3) then set opposite (E4) to true
            newRoom.activeEntranceways[entrFlagIdx - 1] = true;

        prevRoom.activeEntranceways[entrFlagIdx] = true;
    }

    private void GenerateBlueprintRoom(Vector3 roomPosition)
    {
        GameObject genRoom = Instantiate(bluePrintPrefab, roomPosition, Quaternion.identity) as GameObject;
        genRoom.name = $"{bluePrintPrefab.name}";
        genRoom.transform.SetParent(transform);
    }
    #endregion
}