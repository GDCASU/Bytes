using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevelManager : MonoBehaviour
{
    public enum TestLevelType { Reload, Next, Select }
    public TestLevelType testLevelType;
    public int selectLevel = 0;

    private void OnCollisionEnter(Collision collision)
    {
        switch(testLevelType)
        {
            case TestLevelType.Reload:
                LevelManager.Instance.ReloadLevel();
                break;
            case TestLevelType.Next:
                LevelManager.Instance.NextLevel();
                break;
            case TestLevelType.Select:
                LevelManager.Instance.SelectLevel(selectLevel);
                break;
            default:
                Debug.LogError("Unknown TestLevelType received.");
                break;
        }
    }
}
