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
    public List<Room> mainTrail;
    public List<Room> augmentationTrail;
    public List<Room> keycardTrail;
    public List<Room> trialTrail;
    public List<Room> bossTrail;

    public List<Room> masterTrail; // All trails combined

    [Header("Conditions")]
    public int mainTrailMaxRooms = 10;
    public int augmentationTrailMaxRooms = 1;
    public int keycardTrailMaxRooms = 2;
    public int trialTrailMaxRooms = 2;
    public int bossTrailMaxRooms = 2;

    void Procedure()
    {
        RandomWalker(mainTrailMaxRooms, Vector3.zero, mainTrail, null);
    }

    #region RandomWalker
    void RandomWalker(int maxRooms, Vector3 startingPos, List<Room> trail, Room startingRoom)
    {
        Vector3 curPos = startingPos; // Set the position of the starting room
        Vector3 tempPos;

        if (startingRoom == null)
        {
            Room newRoom = new Room(curPos);
            trail.Add(newRoom);
        }

        int failedAttempts = 0;
        while (trail.Count < maxRooms)
        {
            tempPos = curPos;
            switch (UnityEngine.Random.Range(1, 7)) // Choosing position of next room
            {
                case 1: tempPos += Vector3.right * cellSize; break; // (0, cellSize, 0) * Cell Unit Size
                case 2: tempPos += Vector3.left * cellSize; break; // (0, -cellSize, 0) * Cell Unit Size
                case 3: tempPos += Vector3.forward * cellSize; break; // (cellSize, 0, 0) * Cell Unit Size
                case 4: tempPos += Vector3.back * cellSize; break; // (-cellSize, 0, 0) * Cell Unit Size
                case 5: tempPos += Vector3.up * cellSize; break; // (0, 0, cellSize) * Cell Unit Size
                case 6: tempPos += Vector3.down * cellSize; break; // (0, 0, -cellSize) * Cell Unit Size
            }

            bool inRoomList = false;
            for (int i = masterTrail.Count - 1; i >= 0; i--) // Looping back through master list and Checking for collisions with any other rooms
            {
                if (Vector3.Equals(tempPos, masterTrail[i]))
                {
                    inRoomList = true;
                    failedAttempts++;
                    break;
                }
            }
            if (!inRoomList)
            {
                curPos = tempPos;
                Room newRoom = new Room(curPos);
                trail.Add(newRoom);
                masterTrail.Add(newRoom);
            }
            if (failedAttempts > 4)
            {
                curPos = tempPos;
            }
        }
    }

    private void GenerateBlueprintRoom(Vector3 roomPosition)
    {
        GameObject genRoom = Instantiate(bluePrintPrefab, roomPosition, Quaternion.identity) as GameObject;
        genRoom.name = $"{bluePrintPrefab.name}";
        genRoom.transform.SetParent(transform);
    }
    #endregion
}