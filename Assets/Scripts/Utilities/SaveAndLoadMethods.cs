using UnityEngine;

public static class SaveAndLoadMethods
{
    public static void SaveLevelTime(string levelName, float time)
    {
        PlayerPrefs.SetFloat(levelName + "-time", time);
    }
    

    /// <summary>
    /// Loads the current record level time for the given level (scene) name.
    /// </summary>
    /// <param name="levelName"></param>
    /// <returns>The current record time. Returns -1 if no record has been saved.</returns>
    public static float LoadLevelTime(string levelName)
    {
        return PlayerPrefs.GetFloat(levelName + "-time", -1);
    }
}
