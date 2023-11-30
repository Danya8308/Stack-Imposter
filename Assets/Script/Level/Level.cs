using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public static event Action OnStart;


    public static int LastLevelNumber { get; private set; } = 5;

    public static Level Instance { get; private set; }

    protected void Awake()
    {
        Instance = this;
    }

    public static void StartGame()
    {
        OnStart?.Invoke();
    }

    public static void Next()
    {
        int currentLevel = Profile.IntegerValues.GetValue(IntegerSaveType.CurrentLevel);
        int lastCompletedLevel = Profile.IntegerValues.GetValue(IntegerSaveType.LastCompletedLevel);

        if (currentLevel < 1)
        {
            currentLevel = 1;
        }

        if (currentLevel + 1 <= LastLevelNumber)
        {
            currentLevel++;
        }
        else
        {
            currentLevel = 1;
        }

        if (lastCompletedLevel < currentLevel)
        {
            lastCompletedLevel = currentLevel;
        }

        if (lastCompletedLevel != currentLevel && lastCompletedLevel < LastLevelNumber)
        {
            currentLevel = lastCompletedLevel;
        }       

        Profile.IntegerValues.Save(IntegerSaveType.CurrentLevel, currentLevel);
        Profile.IntegerValues.Save(IntegerSaveType.LastCompletedLevel, lastCompletedLevel);

        SceneManager.LoadScene(currentLevel.ToString());
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}