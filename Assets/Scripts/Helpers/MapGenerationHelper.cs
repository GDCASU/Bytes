using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Learned about Random Walk here: https://www.youtube.com/watch?v=F_Zc1nvtB0o
public static class MapGenerationHelper
{
    public enum DirectionType { All, XZ }

    public static HashSet<Vector3Int> RandomWalk(Vector3Int startPos, int walkLength, DirectionType dirType)
    {
        HashSet<Vector3Int> randomPath = new HashSet<Vector3Int>();

        randomPath.Add(startPos);
        Vector3Int previousPos = startPos;
        Vector3Int newPos = Vector3Int.zero;
        for (int i = 0; i < walkLength; i++)
        {
            switch (dirType)
            {
                case DirectionType.All:
                    newPos = previousPos + Direction3D.GetRandomNormalDir();
                    break;
                case DirectionType.XZ:
                    newPos = previousPos + Direction3D.GetRandomNormalDirXZ();
                    break;
                default:
                    Debug.LogError("Unknown DirectionType received...");
                    break;
            }
            randomPath.Add(newPos);
            previousPos = newPos;
        }

        return randomPath;
    }
}

public static class Direction3D
{
    public static List<Vector3Int> normalDirList = new List<Vector3Int>
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
    };

    public static List<Vector3Int> normalDirXZList = new List<Vector3Int>
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
    };

    public static Vector3Int GetRandomNormalDir()
    {
        return normalDirList[Random.Range(0, normalDirList.Count)];
    }

    public static Vector3Int GetRandomNormalDirXZ()
    {
        return normalDirXZList[Random.Range(0, normalDirXZList.Count)];
    }
}
