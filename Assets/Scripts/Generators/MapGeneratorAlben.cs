using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Learned about Map Generation here: https://www.youtube.com/watch?v=LnbZLnCXSyI
public class MapGeneratorAlben : MonoBehaviour
{
    private enum StartType { BackToStart, Continue, PickRandom }

    [Header("Map Objects")]
    [SerializeField]
    private GameObject terrain;
    [SerializeField]
    private GameObject[] mapLandmarks;
    [SerializeField]
    private int objSpawnRate;
    [SerializeField]
    private int objSpawnMinSpacing;
    [SerializeField]
    private int objSpawnMaxSpacing;

    [Header("Map Generation Variables")]
    [SerializeField]
    private MapGenerationHelper.DirectionType dirType;
    [SerializeField]
    private Vector3Int startPos;
    [SerializeField]
    private StartType startType;
    [SerializeField]
    private Vector3Int minVector;
    [SerializeField]
    private Vector3Int maxVector;
    [SerializeField]
    private int walkCycles = 10;
    [SerializeField]
    private int walkLength = 10;

    private void Start()
    {
        RunGeneration();
    }

    public void RunGeneration()
    {
        HashSet<Vector3Int> floorPositions = RunRandomWalk();

        // Instantiate(terrain, Vector3.zero, Quaternion.identity);
        Instantiate(terrain);
        foreach (Vector3Int pos in floorPositions)
        {
            int pickedSpacing = Random.Range(objSpawnMinSpacing, objSpawnMaxSpacing);
            if (Random.Range(0, objSpawnRate) == 0 && CheckWithinBounds(pos * pickedSpacing))
                Instantiate(mapLandmarks[Random.Range(0, mapLandmarks.Length)], pos * pickedSpacing, Quaternion.identity);
        }
    }

    private HashSet<Vector3Int> RunRandomWalk()
    {
        Vector3Int currentPos = startPos;
        HashSet<Vector3Int> floorPos = new HashSet<Vector3Int>();
        for (int i = 0; i < walkCycles; i++)
        {
            HashSet<Vector3Int> randomPath = MapGenerationHelper.RandomWalk(currentPos, walkLength, dirType);
            floorPos.UnionWith(randomPath); // Handles duplicate positions

            switch (startType)
            {
                case StartType.BackToStart:
                    currentPos = startPos;
                    break;
                case StartType.Continue:
                    currentPos = floorPos.ElementAt(floorPos.Count - 1);
                    break;
                case StartType.PickRandom:
                    currentPos = floorPos.ElementAt(Random.Range(0, floorPos.Count));
                    break;
                default:
                    Debug.LogError("Unknown StartType received...");
                    break;
            }
        }

        return floorPos;
    }

    private bool CheckWithinBounds(Vector3Int pos)
    {
        if (pos.x >= minVector.x && pos.y >= minVector.y && pos.z >= minVector.z &&
            pos.x <= maxVector.x && pos.y <= maxVector.y && pos.z <= maxVector.z)
        {
            return true;
        }
        return false;
    }
}
