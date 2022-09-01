/*
 * Author: Alben Trang, Cristion Dominguez
 * Date: 8/30/2022
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    private int maxScenesInBuild;

    private void Start()
    {
        // Make sure player starts in a scene that's in the Build Settings; otherwise, maxScenesInBuild is off by 1.
        maxScenesInBuild = SceneManager.sceneCountInBuildSettings;
        print($"Loaded scene: { SceneManager.GetActiveScene().name}");
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
