using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Alben Trang
 * Date: 8/31/2022
 */
public class CheckpointManager : MonoSingleton<CheckpointManager>
{
    private GameObject[] checkpoints = null;
    private GameObject[] usedCheckpoints = null;
    private GameObject latestCheckpoint = null;
    private GameObject player = null;

    private void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        usedCheckpoints = new GameObject[checkpoints.Length];
        player = GameObject.FindGameObjectWithTag("Player");

        CheckpointManager.Instance.RespawnPlayer();
    }

    public void RespawnPlayer()
    {
        if (latestCheckpoint != null && usedCheckpoints.Length > 0)
        {
            player.transform.position = latestCheckpoint.transform.GetChild(0).position;
            foreach (GameObject usedCP in usedCheckpoints)
            {
                usedCP.GetComponent<Checkpoint>().DisableCheckpoint();
            }
        }
    }

    public void SetLatestCheckpoint(GameObject newCheckpoint) => latestCheckpoint = newCheckpoint;

    public void DontDestroyManager() => DontDestroyOnLoad(this.gameObject);

    public void DestroyManager() => Destroy(this.gameObject);
}
