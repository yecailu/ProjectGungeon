using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// ¥Êµµπ§æﬂ¿‡
/// </summary>
public class SaveSystem
{
    
    public static void SaveByJson(string saveFileName, object date)
    {
        var json = JsonUtility.ToJson(date);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.WriteAllText(path, json);

            Debug.Log("Save successful to " + path);

        }
        catch (System.Exception exception)
        {
            Debug.LogError("Save failed: " + exception);
        }
    }

    public static T LoadFromJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            var json = File.ReadAllText(path);
            var date = JsonUtility.FromJson<T>(json);

            return date;
        }
        catch (System.Exception exception)
        {
            Debug.LogError("Load failed: " + exception);

            return default;
        }      
    }

    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);

        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
            Debug.LogError("Delete failed: " + exception);
        }
    }
}
