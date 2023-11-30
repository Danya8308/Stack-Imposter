using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    protected void Awake()
    {
        int levelNumber = Profile.IntegerValues.GetValue(IntegerSaveType.CurrentLevel);

        if (levelNumber < 1)
        {
            levelNumber = 1;
            Profile.IntegerValues.Save(IntegerSaveType.CurrentLevel, levelNumber);
        }

        SceneManager.LoadScene(levelNumber.ToString());
    }
}