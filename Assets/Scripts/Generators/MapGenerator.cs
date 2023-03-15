using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public enum TrailType
{
    Master,
    Main,
    Augmentation,
    Trial,
    Keycard,
    Boss
}

public class MapGenerator : MonoBehaviour
{
    #region Variables
    const int ROOM_FACES = 6;

    [Header("Settings")]
    public float gridCellSize = 40;
    public bool debug = true;

    [Header("Max Rooms")]
    public int mainTrailMaxRooms = 10;
    public int augmentationTrailMaxRooms = 1;
    public int keycardTrailMaxRooms = 2;
    public int trialTrailMaxRooms = 2;
    public int bossTrailMaxRooms = 2;

    [Header("Room Shape Spawn Chances")]
    [Range(0, 1)] public float tRoomChance = 0.4f;
    [Range(0, 1)] public float hRoomChance = 0.5f;
    [Range(0, 1)] public float bRoomChance = 1.0f;


    [Header("Room Prefabs")]
    public GameObject bluePrintPrefab;
    public List<GameObject> gPrefab;
    public List<GameObject> tPrefab;
    public List<GameObject> hPrefab;
    public List<GameObject> bPrefab;

    [Header("Trails")]
    public List<BlueprintRoom> masterTrail; // All trails combined
    public List<BlueprintRoom> mainTrail; // Trail to Boss Room
    public List<BlueprintRoom> augmentationTrail; // Trail to Augmentation Room
    public List<BlueprintRoom> keycardTrail; // Trail to Keycard Room
    public List<BlueprintRoom> trialTrail; // Trail to Trial Room
    public List<BlueprintRoom> bossTrail; // Trail to Boss Room

    [Header("Rooms")]
    public List<GameObject> masterRooms;
    public List<GameObject> mainRooms;
    public List<GameObject> augmentationRooms;
    public List<GameObject> keycardRooms;
    public List<GameObject> trialRooms;
    public List<GameObject> bossRooms;


    int entrFlagIdx = 0;
    #endregion

    void Start()
    {
        masterTrail = new List<BlueprintRoom>(); // All trails combined
        mainTrail = new List<BlueprintRoom>(); // Trail to Boss Room
        augmentationTrail = new List<BlueprintRoom>(); // Trail to Augmentation Room
        keycardTrail = new List<BlueprintRoom>(); // Trail to Keycard Room
        trialTrail = new List<BlueprintRoom>(); // Trail to Trial Room
        bossTrail = new List<BlueprintRoom>(); // Trail to Boss Room

        masterRooms = new List<GameObject>(); // All Rooms combined
        mainRooms = new List<GameObject>(); // Rooms to Boss Room
        augmentationRooms = new List<GameObject>(); // Rooms to Augmentation Room
        keycardRooms = new List<GameObject>(); // Rooms to Keycard Room
        trialRooms = new List<GameObject>(); // Rooms to Trial Room
        bossRooms = new List<GameObject>(); // Rooms to Boss Room


        if (!debug)
        {
            BlueprintProcedure();
            RoomGenerationProcedure();
        }
    }

    void BlueprintProcedure() // 1. Generate Blueprint Trails
    {
        RandomWalker(mainTrailMaxRooms, mainTrail, null); // Main Trail to boss

        int randomIdx = UnityEngine.Random.Range(1, (mainTrail.Count - 1));
        BlueprintRoom randomStartingRoom = mainTrail[randomIdx];
        RandomWalker(augmentationTrailMaxRooms, augmentationTrail, randomStartingRoom); // Trail to Augmentation Room

        randomIdx = UnityEngine.Random.Range(1, (mainTrail.Count - 1));
        randomStartingRoom = mainTrail[randomIdx];
        RandomWalker(trialTrailMaxRooms, trialTrail, randomStartingRoom); // Trial Trail Generation

        randomIdx = UnityEngine.Random.Range(1, (mainTrail.Count - 1));
        randomStartingRoom = mainTrail[randomIdx];
        RandomWalker(keycardTrailMaxRooms, keycardTrail, randomStartingRoom); // Keycard Trail Generation
    }

    void RoomGenerationProcedure() // 2. Generate Rooms
    {

    }

    #region BlueprintProcedure
    void RandomWalker(int maxRooms, List<BlueprintRoom> trail, BlueprintRoom startingRoom)
    {
        Vector3 curPos = Vector3.zero; // Set the position of the starting room
        BlueprintRoom curRoom = null;
        Vector3 tempPos = Vector3.zero; // new postion to be choosen

        if (startingRoom == null) // If there are not yet any rooms
        {
            BlueprintRoom newRoom = new BlueprintRoom(curPos);
            if (debug)
            {
                newRoom.roomName = $"{bluePrintPrefab.name} | {trail.Count}";
                GenerateBlueprintGizmo(curPos, trail.Count, newRoom.roomName);
            }
            trail.Add(newRoom);
            masterTrail.Add(newRoom);
            curRoom = newRoom;
        }
        else
        {
            curPos = startingRoom.position;
            curRoom = startingRoom;
        }

        int failedAttempts = 0;
        while (trail.Count < maxRooms)
        {
            tempPos = curPos;
            switch (UnityEngine.Random.Range(1, ROOM_FACES + 1)) // Choosing position of next room from 6 possible directions
            {
                case 1: tempPos += Vector3.right * gridCellSize; // E0 (cellSize, 0, 0) * Cell Unit Size
                    entrFlagIdx = 0;
                    break;
                case 2: tempPos += Vector3.left * gridCellSize; // E1 (-cellSize, 0, 0) * Cell Unit Size
                    entrFlagIdx = 1;
                    break;
                case 3: tempPos += Vector3.forward * gridCellSize; // E2 (0, 0, cellSize) * Cell Unit Size
                    entrFlagIdx = 2;
                    break;
                case 4: tempPos += Vector3.back * gridCellSize; // E3 (0, 0, -cellSize) * Cell Unit Size
                    entrFlagIdx = 3;
                    break;
                case 5: tempPos += Vector3.up * gridCellSize;  // E4 (0, cellSize, 0) * Cell Unit Size
                    entrFlagIdx = 4;
                    break;
                case 6: tempPos += Vector3.down * gridCellSize; // E5 (0, -cellSize, 0) * Cell Unit Size
                    entrFlagIdx = 5;
                    break; 
            }

            bool inRoomList = false;
            BlueprintRoom collidedRoom = null;
            foreach(BlueprintRoom room in masterTrail) // Check master trail for colliding rooms (the temp pos is inside another designated room space)
            {
                if (Vector3.Equals(tempPos, room.position)) // Test Failed
                {
                    collidedRoom = room;
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

                if (debug)
                {
                    newRoom.roomName = $"{bluePrintPrefab.name} | {trail.Count}";
                    GenerateBlueprintGizmo(curPos, trail.Count, newRoom.roomName);
                }

                curRoom = newRoom;
                trail.Add(newRoom);
                masterTrail.Add(newRoom);

                failedAttempts = 0;
            }

            if (failedAttempts >= ROOM_FACES) // If failed too many times backtrack (very rare)
            {
                curPos = tempPos;
                curRoom = collidedRoom;
                failedAttempts = 0;
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

    private void GenerateBlueprintGizmo(Vector3 roomPosition, int count, string roomName) // Generate Gizmo for Debugging purposes 
    {
        GameObject genRoom = Instantiate(bluePrintPrefab, roomPosition, Quaternion.identity) as GameObject;
        genRoom.name = roomName;
        genRoom.transform.SetParent(transform);
    }
    #endregion

    private enum RoomCase
    {
        PosZ = 0,
        NegZ = 1,
        PosX = 2,
        NegX = 3,
        PosY = 4,
        NegY = 5
    }

    #region RoomGenerationProcedure
    public void GenerateRooms(List<BlueprintRoom> trail, TrailType trailType)
    {
        switch (trailType)
        {
            case TrailType.Master:
                break;
            case TrailType.Main:
                mainRooms.Add(GenerateRoom(RoomShape.GeneralRoom, RoomType.Start, trail, 0, 0)); // Generate Starting Room G-Room varient
                for (int i = 1; i < mainTrail.Count; )   // loop through all blueprint rooms
                {
                    float roomChanceRoll = Random.Range(0, 1.01f);
                    RoomCase rCase = RoomCase.PosZ;
                    if (false)  // if can spawn B-Room & passed B-Room spawn chance
                    {
                        // spawn B-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        i += 4; // jump index to next empty blueprint room
                    }
                    else if ((roomChanceRoll <= tRoomChance) && (i < mainTrail.Count - 1) && TRoomPositionCondition(trail[i].position, trail[i + 1].position, out rCase))  // else if can spawn T-Room & passed T-Room spawn chance && extra space for a 1x2 at end of trail
                    {
                        mainRooms.Add(GenerateRoom(RoomShape.TallRoom, RoomType.General, trail, i, rCase)); // Spawn T-Room
                        i += 2; // jump index to next empty blueprint room
                    }
                    else if ((roomChanceRoll <= hRoomChance) && (i < mainTrail.Count - 1) && HRoomPositionCondition(trail[i].position, trail[i + 1].position, out rCase)) // else if can spawn H-Room & passed H-Room spawn chance && extra space for a 2x1 at end of trail
                    {
                        mainRooms.Add(GenerateRoom(RoomShape.HallRoom, RoomType.General, trail, i, rCase)); // Spawn H-Room
                        i += 2; // jump index to next empty blueprint room
                    }
                    else
                    {
                        mainRooms.Add(GenerateRoom(RoomShape.GeneralRoom, RoomType.General, trail, i, 0)); // Spawn G-Room
                        i++; // jump index to next empty blueprint room
                    }
                }
                break;
            case TrailType.Augmentation:
                // loop through all blueprint rooms
                    // if can spawn B-Room & passed B-Room spawn chance
                        // spawn B-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else if can spawn T-Room & passed T-Room spawn chance
                        // Spawn T-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else if can spawn H-Room & passed H-Room spawn chance
                        // Spawn H-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else
                        // Spawn G-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                // Tag last Room as Augmentation
                break;
            case TrailType.Trial:
                // loop through all blueprint rooms
                    // if can spawn B-Room & passed B-Room spawn chance
                        // spawn B-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else if can spawn T-Room & passed T-Room spawn chance
                        // Spawn T-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else if can spawn H-Room & passed H-Room spawn chance
                        // Spawn H-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else
                        // Spawn G-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                // Tag last Room as Trial
                break;
            case TrailType.Keycard:
                // loop through all blueprint rooms
                    // if can spawn B-Room & passed B-Room spawn chance
                        // spawn B-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else if can spawn T-Room & passed T-Room spawn chance
                        // Spawn T-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else if can spawn H-Room & passed H-Room spawn chance
                        // Spawn H-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                    // else
                        // Spawn G-Room
                        // Hook up blueprintRoom.entrancewayflags to new room
                        // jump index to next empty blueprint room
                // Tag last Room as Keycard
                break;
            case TrailType.Boss:
                // Skip for now
                break;
            default:
                Debug.Log("Error: Undefined trail type.");
                break;
        }
    }

    bool TRoomPositionCondition(Vector3 originRoomPos, Vector3 nextRoomPos, out RoomCase rCase)
    {
        if (originRoomPos.x == nextRoomPos.x                // if both rooms have same x value
            && originRoomPos.z == nextRoomPos.z             // if both rooms have same y value
            && (originRoomPos.y - nextRoomPos.y) <= 0)      // if difference of z <= 0
        {
            rCase = RoomCase.PosY; // Room Case is used to specify the Room's rotation and movement on instantiation (Difference: origin - next)
            return true;
        }
        else if (originRoomPos.x == nextRoomPos.x           // if both rooms on same x value
            && originRoomPos.z == nextRoomPos.z             // if both rooms on same y value
            && (originRoomPos.y - nextRoomPos.y) > 0)       // if difference of z > 0
        {
            rCase = RoomCase.NegY;
            return true;
        }
        else
        {
            rCase = 0;
            return false;
        } // if both rooms differ by cellsize on y
    }

    bool HRoomPositionCondition(Vector3 originRoomPos, Vector3 nextRoomPos, out RoomCase rCase)
    {
        if (originRoomPos.x == nextRoomPos.x                // if both rooms have same x value
            && originRoomPos.y == nextRoomPos.y             // if both rooms have same y value
            && (originRoomPos.z - nextRoomPos.z) <= 0)      // if difference of z <= 0
        {
            rCase = RoomCase.PosZ; // Room Case is used to specify the Room's rotation and movement on instantiation (Difference: origin - next)
            return true; 
        }
        else if (originRoomPos.x == nextRoomPos.x           // if both rooms on same x value
            && originRoomPos.y == nextRoomPos.y             // if both rooms on same y value
            && (originRoomPos.z - nextRoomPos.z) > 0)       // if difference of z > 0
        {
            rCase = RoomCase.NegZ;
            return true;
        }
        else if (originRoomPos.z == nextRoomPos.z           // if both rooms on same z value
            && originRoomPos.y == nextRoomPos.y             // if both rooms on same y value
            && (originRoomPos.x - nextRoomPos.x) <= 0)      // if difference of x <= 0
        {
            //rCase = RoomCase.PosX;
            rCase = 0;                                      // *** Skipping PosX condition for now ***
            return false;
        }
        else if (originRoomPos.z == nextRoomPos.z               // if both rooms on same z value
                && originRoomPos.y == nextRoomPos.y             // if both rooms on same y value
                && (originRoomPos.x - nextRoomPos.x) > 0)       // if difference of x > 0
        {
            //rCase = RoomCase.NegX;                        // *** Skipping NegX condition for now ***
            rCase = 0;
            return false;
        }
        else
        {
                rCase = 0;
                return false;
            } // if both rooms differ by cellsize on y
    }

    GameObject GenerateRoom(RoomShape shape, RoomType type, List<BlueprintRoom> trail, int index, RoomCase rCase)
    {
        GameObject genRoom = null;
        Quaternion rotation = Quaternion.identity;
        switch (shape)
        {
            case RoomShape.GeneralRoom:
                genRoom = Instantiate(gPrefab[Random.Range(0, (gPrefab.Count - 1))], trail[index].position, rotation) as GameObject; // Instantiate G-Room at position of indexed blueprint room; use a random room in the G-Room list
                genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index].activeEntranceways, 0); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array
                genRoom.GetComponent<Room>().ActivateAllEntranceways(); // Activate new rooms entranceways
                break;
            case RoomShape.HallRoom:
                if (rCase == RoomCase.PosZ)
                {
                    genRoom = Instantiate(hPrefab[Random.Range(0, (hPrefab.Count - 1))], trail[index].position, rotation) as GameObject; // Instantiate G-Room at position of indexed blueprint room; use a random room in the G-Room list
                    genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index].activeEntranceways, 0); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array (first 6 elements : 0 - 5)
                    genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index + 1].activeEntranceways, 1); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array (next 6 elements : 6 - 11)
                    genRoom.GetComponent<Room>().ActivateAllEntranceways(); // Activate new rooms entranceways
                }
                else if (rCase == RoomCase.NegZ)
                {
                    genRoom = Instantiate(hPrefab[Random.Range(0, (hPrefab.Count - 1))], (trail[index + 1].position), rotation) as GameObject; // Instantiate G-Room at position of indexed blueprint room; use a random room in the G-Room list
                    genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index].activeEntranceways, 1); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array (first 6 elements : 0 - 5)
                    genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index + 1].activeEntranceways, 0); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array (next 6 elements : 6 - 11)
                    genRoom.GetComponent<Room>().ActivateAllEntranceways(); // Activate new rooms entranceways
                }
                else if (rCase == RoomCase.PosX)
                {
                    // *** Skipping for now ***
                }
                else if (rCase == RoomCase.NegX)
                {
                    // *** Skipping for now ***
                }
                else
                {
                    Debug.Log("Error: Roomcase does not match any valid H-Room Cases.");
                }
                break;
            case RoomShape.TallRoom:
                if (rCase == RoomCase.PosY)
                {
                    genRoom = Instantiate(tPrefab[Random.Range(0, (tPrefab.Count - 1))], trail[index].position, rotation) as GameObject; // Instantiate G-Room at position of indexed blueprint room; use a random room in the G-Room list
                    genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index].activeEntranceways, 0); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array (first 6 elements : 0 - 5)
                    genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index + 1].activeEntranceways, 1); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array (next 6 elements : 6 - 11)
                    genRoom.GetComponent<Room>().ActivateAllEntranceways(); // Activate new rooms entranceways
                }
                else if (rCase == RoomCase.NegY)
                {
                    genRoom = Instantiate(tPrefab[Random.Range(0, (tPrefab.Count - 1))], (trail[index + 1].position), rotation) as GameObject; // Instantiate G-Room at position of indexed blueprint room; use a random room in the G-Room list
                    genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index].activeEntranceways, 1); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array (first 6 elements : 0 - 5)
                    genRoom.GetComponent<Room>().CopyBlueprintArrayFlags(trail[index + 1].activeEntranceways, 0); // Copy array of blueprint's entrencewayFlags to the newly generated room's entrancewayFlags array (next 6 elements : 6 - 11)
                    genRoom.GetComponent<Room>().ActivateAllEntranceways(); // Activate new rooms entranceways
                }
                else
                {
                    Debug.Log("Error: Roomcase does not match any valid T-Room Cases.");
                }
                break;
            case RoomShape.BigRoom:
                break;
            default:
                Debug.Log("Error: Room Shape Invalid.");
                break;
        }

        switch (type)
        {
            case RoomType.General:
                genRoom.GetComponent<Room>().roomType = RoomType.General;
                break;
            case RoomType.Start:
                genRoom.GetComponent<Room>().roomType = RoomType.Start;
                break;
            case RoomType.Augmentation:
                genRoom.GetComponent<Room>().roomType = RoomType.Augmentation;
                break;
            case RoomType.Keycard:
                genRoom.GetComponent<Room>().roomType = RoomType.Keycard;
                break;
            case RoomType.Trail:
                genRoom.GetComponent<Room>().roomType = RoomType.Trail;
                break;
            case RoomType.Boss:
                genRoom.GetComponent<Room>().roomType = RoomType.Boss;
                break;
            case RoomType.Escape:
                genRoom.GetComponent<Room>().roomType = RoomType.Escape;
                break;

        }

        return genRoom;
    }
    #endregion
    
    void ClearAllTrails()
    {
        masterTrail.Clear(); // All trails combined
        mainTrail.Clear(); // Trail to Boss Room
        augmentationTrail.Clear(); // Trail to Augmentation Room
        keycardTrail.Clear(); // Trail to Keycard Room
        trialTrail.Clear(); // Trail to Trial Room
        bossTrail.Clear(); // Trail to Boss Room
    }

    #region DebugGUI
    bool alreadyBMain, alreadyBAugmentation, alreadyBTrial,     // Blueprint Procedure Flags
        alreadyBKeycard;                                        
    bool alreadyGMain, alreadyGAugmentation, alreadyGTrial,     // Room Generation Procedure Flags
        alreadyGKeycard;

    void OnGUI()
    {
        if (debug)
        {
            if (GUILayout.Button("Reload Scene"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload Active Scene
            }

            if (GUILayout.Button("Generate Main Trail"))
            {
                if (!alreadyBMain)
                {
                    RandomWalker(mainTrailMaxRooms, mainTrail, null); // Main Trial Generation

                    alreadyBMain = true;
                    Debug.Log("Main Trail Generated");
                }
                else
                    Debug.Log("Error: Already generated Main Trail");
            }

            if (GUILayout.Button("Generate Augmentation Trail"))
            {
                if (!alreadyBAugmentation && alreadyBMain)
                {
                    int randomIdx = UnityEngine.Random.Range(1, (mainTrail.Count - 1));
                    BlueprintRoom randomStartingRoom = mainTrail[randomIdx];

                    RandomWalker(augmentationTrailMaxRooms, augmentationTrail, randomStartingRoom); // Augmentation Trail Generation

                    alreadyBAugmentation = true;
                    Debug.Log($"Augmentation Trail Generated at room index : {randomIdx}");
                }
                else
                    Debug.Log("Error: Already generated Augmentation Trail or Main Trail has not yet been generated.");
            }

            if (GUILayout.Button("Generate Trial Trail"))
            {
                if (!alreadyBTrial && alreadyBMain)
                {
                    int randomIdx = UnityEngine.Random.Range(1, (mainTrail.Count - 1));
                    BlueprintRoom randomStartingRoom = mainTrail[randomIdx];

                    RandomWalker(trialTrailMaxRooms, trialTrail, randomStartingRoom); // Trial Trail Generation

                    alreadyBTrial = true;
                    Debug.Log($"Trial Trail Generated at room index : {randomIdx}");
                }
                else
                    Debug.Log("Error: Already generated Trial Trail or Main Trail has not yet been generated.");
            }

            if (GUILayout.Button("Generate Keycard Trail"))
            {
                if (!alreadyBKeycard && alreadyBMain)
                {
                    int randomIdx = UnityEngine.Random.Range(1, (mainTrail.Count - 1));
                    BlueprintRoom randomStartingRoom = mainTrail[randomIdx];

                    RandomWalker(keycardTrailMaxRooms, keycardTrail, randomStartingRoom); // Keycard Trail Generation

                    alreadyBKeycard = true;
                    Debug.Log($"Keycard Trail Generated at room index : {randomIdx}");
                }
                else
                    Debug.Log("Error: Already generated Keycard Trail or Main Trail has not yet been generated.");
            }

            if (GUILayout.Button("Print Entranceway Flags: Blueprint Rooms"))
            {
                if (!alreadyGMain && !alreadyGAugmentation && !alreadyGTrial && !alreadyGKeycard)
                {
                    Debug.Log("Main Trail:");
                    if (mainTrail != null)
                        PrintBRoomFlags(mainTrail);
                    else
                        Debug.Log("\tNo Data.");

                    Debug.Log("Augmentation Trail:");
                    if (augmentationTrail != null)
                        PrintBRoomFlags(augmentationTrail);
                    else
                        Debug.Log("\tNo Data.");

                    Debug.Log("Trial Trail:");
                    if (trialTrail != null)
                        PrintBRoomFlags(trialTrail);
                    else
                        Debug.Log("\tNo Data.");

                    Debug.Log("Keycard Trail:");
                    if (keycardTrail != null)
                        PrintBRoomFlags(keycardTrail);
                    else
                        Debug.Log("\tNo Data.");
                }
                else
                    Debug.Log("Error: Room Generation has already begun. Therefore, cannot print flags.");
            }

            if (GUILayout.Button("Generate Rooms: Main Trail"))
            {
                if (alreadyBMain)
                {
                    GenerateRooms(mainTrail, TrailType.Main);
                    Debug.Log("Main Trail Rooms Successfully Generated!");
                }
                else
                    Debug.Log("Error: Main Trail has not yet been made.");
            }
        }
    }

    private void PrintBRoomFlags(List<BlueprintRoom> trail)
    {
        foreach (BlueprintRoom bRoom in trail)
        {
            Debug.Log($"\tBluePrint Room \"{bRoom.roomName}\" Active Entrances: " +
                $"E0= {bRoom.activeEntranceways[0]}, E1= {bRoom.activeEntranceways[1]}, E2= {bRoom.activeEntranceways[2]}, " +
                $"E3= {bRoom.activeEntranceways[3]}, E4= {bRoom.activeEntranceways[4]}, E5= {bRoom.activeEntranceways[5]}");
        }
    }
    #endregion
}