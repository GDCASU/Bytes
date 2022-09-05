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
    // Variables that need to be checked again after a scene reloads.
    private GameObject checkpointHandler;
    private GameObject player;

    // Variables that will be stored since this object is in the DontDestroyOnLoad category.
    private Vector3 latestCheckpointPos;
    private bool[] checkpointsEnabled;
    private bool firstTimeInScene = true;

    /*
     * Links about OnEnable
     * https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnEnable.html
     * https://forum.unity.com/threads/call-a-function-after-the-scene-is-loaded.1207291/
     * https://answers.unity.com/questions/1174255/since-onlevelwasloaded-is-deprecated-in-540b15-wha.html
     */
    private void OnEnable()
    {
        SceneManager.sceneLoaded += RespawnPlayer;
    }

    public void RespawnPlayer(Scene scene, LoadSceneMode mode)
    {
        if (firstTimeInScene)
        {
            firstTimeInScene = false;
            player = GameObject.FindWithTag("Player");
            latestCheckpointPos = player.transform.position; // After new scene loads, player spawns at scene's start
            checkpointHandler = GameObject.FindWithTag("Respawn");
            if (checkpointHandler != null)
            {
                checkpointsEnabled = new bool[checkpointHandler.transform.childCount];
                for (int i = 0; i < checkpointsEnabled.Length; i++)
                {
                    checkpointsEnabled[i] = true;
                }
            }
            else
            {
                Debug.LogWarning("There is no empty game object called Checkpoint Handler with the tag 'Respawn'.\n" +
                    "It can have Checkpoint prefabs as its children.");
            }
        }
        else
        {
            checkpointHandler = GameObject.FindWithTag("Respawn");
            if (checkpointHandler != null)
            {
                for (int i = 0; i < checkpointHandler.transform.childCount; i++)
                {
                    if (checkpointsEnabled[i] == false)
                    {
                        checkpointHandler.transform.GetChild(i).GetComponent<Checkpoint>().Disable();
                    }
                }
                player = GameObject.FindWithTag("Player");
                player.transform.position = latestCheckpointPos;
            }
            else
            {
                Debug.LogWarning("There is no empty game object called Checkpoint Handler with the tag 'Respawn'.\n" +
                    "It can have Checkpoint prefabs as its children.");
            }
        }
    }

    public void SetLatestCheckpoint(GameObject checkpoint)
    {
        latestCheckpointPos = checkpoint.transform.GetChild(0).position;
        for (int i = 0; i < checkpointHandler.transform.childCount; i++)
        {
            checkpointsEnabled[i] = checkpointHandler.transform.GetChild(i).GetComponent<Checkpoint>().GetEnabled();
        }
    }

    public void ResetManager()
    {
        firstTimeInScene = true;
    }
}
