using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Author: Alben Trang
 * Date: 8/31/2022
 */
public class CheckpointManager : MonoSingleton<CheckpointManager>
{
    /*
     * There's no need to add the checkpoints in the Unity Inspector.
     * However, you should put the Checkpoint game objects in an
     * empty game object so that they are put in order. Otherwise,
     * the player may spawn at the wrong checkpoint.
     */
    [SerializeField] private Transform[] checkpoints;
    [SerializeField] private Transform CPDataPrefab;

    private Transform CPDataInstance = null;
    private GameObject player = null;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += RespawnPlayer;
    }

    public void RespawnPlayer(Scene scene, LoadSceneMode mode)
    {
        // When a scene loads, find the checkpoints because this manager doesn't save them after a scene loads.
        GameObject[] checkpointObjects = GameObject.FindGameObjectsWithTag("Checkpoint");
        checkpoints = new Transform[checkpointObjects.Length];
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i] = checkpointObjects[i].transform;

        if (GameObject.FindWithTag("Respawn") == null)
        {
            CPDataInstance = Instantiate(CPDataPrefab, null, true);
            player = GameObject.FindWithTag("Player");
            CPDataInstance.GetComponent<CheckpointData>().InitializeData(player.transform.position, checkpoints.Length);
        }
        else
        {
            CPDataInstance = GameObject.FindWithTag("Respawn").transform;
            CheckpointData data = CPDataInstance.GetComponent<CheckpointData>();
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (data.GetEnabled(i) == false)
                {
                    checkpoints[i].GetComponent<Checkpoint>().Disable();
                }
            }
            player = GameObject.FindWithTag("Player");
            player.transform.position = data.GetLatestCheckpointPos();
        }
    }
    
    public void SetLatestCheckpoint(GameObject checkpoint)
    {
        CheckpointData data = CPDataInstance.GetComponent<CheckpointData>();
        data.SetLatestCheckpointPos(checkpoint.transform.GetChild(0).position);
        for (int i = 0; i < checkpoints.Length; i++)
        {
            data.SetEnabled(i, checkpoints[i].GetComponent<Checkpoint>().GetEnabled());
        }
    }
}
