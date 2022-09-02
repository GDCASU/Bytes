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
    private GameObject[] checkpoints = null;
    private ArrayList usedCheckpoints = new ArrayList();
    private GameObject player;

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

    private void Start()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void RespawnPlayer(Scene scene, LoadSceneMode mode)
    //public void RespawnPlayer()
    {
        print("Testing RespawnPlayer...");
        if (usedCheckpoints != null && usedCheckpoints.Count > 0)
        {
            GameObject latestCheckpoint = (GameObject) usedCheckpoints[usedCheckpoints.Count - 1];
            player.transform.position = latestCheckpoint.transform.GetChild(0).position;
            foreach (GameObject usedCP in usedCheckpoints)
            {
                usedCP.GetComponent<Checkpoint>().DisableCheckpoint();
                usedCheckpoints.Remove(usedCP);
            }
            print("Finished!");
        }
        else
        {
            print("No checkpoints disabled!");
        }
    }

    public void SetLatestCheckpoint(GameObject newCheckpoint)
    {
        usedCheckpoints.Add(newCheckpoint);
    }

    // public void DontDestroyManager() => DontDestroyOnLoad(this.gameObject);

    public void DestroyManager() => Destroy(this.gameObject);
}
