using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointData : MonoBehaviour
{
    private bool[] checkpointsEnabled;
    private Vector3 latestCheckpointPos;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeData(Vector3 playerInitialPos, int checkpointsIndexLength)
    {
        latestCheckpointPos = playerInitialPos;
        checkpointsEnabled = new bool[checkpointsIndexLength];
        for (int i = 0; i < checkpointsIndexLength; i++)
            checkpointsEnabled[i] = true;
    }

    public bool GetEnabled(int index) => checkpointsEnabled[index];

    public Vector3 GetLatestCheckpointPos() => latestCheckpointPos;

    public void SetEnabled(int index, bool enable) => checkpointsEnabled[index] = enable;

    public void SetLatestCheckpointPos(Vector3 latestPos) => latestCheckpointPos = latestPos;

    public void DestroyData() => Destroy(gameObject);
}
