using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int maxScenesInBuild;

    #region Singleton
    private static LevelManager instance;

    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("The LevelManager is NULL.");
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Start()
    {
        // Make sure player starts in a scene that's in the Build Settings; otherwise, maxScenesInBuild is off by 1.
        maxScenesInBuild = SceneManager.sceneCountInBuildSettings;
    }

    public void ReloadLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void NextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex < maxScenesInBuild)
            SceneManager.LoadScene(nextLevelIndex);
        else
            Debug.LogError("There are no more scenes after this one in the build settings.");
    }

    public void SelectLevel(int selectedIndex)
    {
        if (selectedIndex >= 0 && selectedIndex < maxScenesInBuild)
            SceneManager.LoadScene(selectedIndex);
        else
            Debug.LogError("That scene index does not exist in the build settings.");
    }
}
