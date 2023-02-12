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
    public List<Vector2> NormalTrail;
    public List<Vector2> AugmentationTrail;
    public List<Vector2> KeycardTrail;
    public List<Vector2> TrialTrail;
    public List<Vector2> BossTrail;

    [Header("Conditions")]
    public int GeneralTrailMaxRooms = 10;
    public int AugmentationTrailMaxRooms = 1;
    public int KeycardTrailMaxRooms = 2;
    public int TrialTrailMaxRooms = 2;
    public int BossTrailMaxRooms = 2;
}
