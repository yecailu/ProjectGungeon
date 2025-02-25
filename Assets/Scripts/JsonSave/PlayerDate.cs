using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerDate :MonoBehaviour
{
    public static PlayerDate Default;

    private void Awake()
    {
        Default = this;
    }

    private void OnDestroy()
    {
        Default = null;

    }

    [Header("UI References")]
    public GameObject ContinuePanel; 

    [System.Serializable]
    class SaveDate
    {
        public int Coin;
        public int HP;
        public int Armor;
        public int Key;


        public bool IsPanelActive = true;

        // 新增关卡进度字段
        public int CurrentLevelIndex;
        public int[] CurrentPacingArray; // Queue 无法直接序列化，转为数组


    }


    const string PLAYER_DATA_KEY = "PlayerData";
    const string PLAYER_DATA_FILE_NAME = "PlayerData.sav";

    //存档数据
    static SaveDate SavingData()
    {
        var saveDate = new SaveDate();

        // 保存基础数值
        saveDate.Coin = Global.Coin.Value;
        saveDate.HP = Global.HP.Value;
        saveDate.Armor = Global.Armor.Value;
        saveDate.Key = Global.Key.Value;

        // 保存面板状态
        if(Default.ContinuePanel != null)
           saveDate.IsPanelActive = Default.ContinuePanel.activeSelf;

        // 关卡进度
        saveDate.CurrentLevelIndex = Global.Levels.IndexOf(Global.CurrentLevel);
        saveDate.CurrentPacingArray = Global.CurrentPacing?.ToArray();

        return saveDate;
    }

    //读取数据
    static void LoadData(SaveDate saveDate)
    {
        //playerName = saveDate.playerName
        Global.Coin.Value = saveDate.Coin;
        Global.HP.Value = saveDate.HP;
        Global.Armor.Value = saveDate.Armor;
        Global.Key.Value = saveDate.Key;

        // 加载面板状态
        if (Default.ContinuePanel != null)
            Default.ContinuePanel.SetActive(saveDate.IsPanelActive);

        // 关卡进度
        if (saveDate.CurrentLevelIndex >= 0 &&
            saveDate.CurrentLevelIndex < Global.Levels.Count)
        {
            Global.CurrentLevel = Global.Levels[saveDate.CurrentLevelIndex];
            Global.CurrentPacing = saveDate.CurrentPacingArray != null ?
                new Queue<int>(saveDate.CurrentPacingArray) :
                new Queue<int>(Global.CurrentLevel.PacingConfig);
        }
        else
        {
            Debug.LogError("Invalid level index, loading default level");
            Global.CurrentLevel = Global.Levels[0];
            Global.CurrentPacing = new Queue<int>(Global.CurrentLevel.PacingConfig);
        }
    }


    public static void Save()
    {
        SaveByJson();
    }

    public static void Load()
    {
        if (DoesSaveFileExist())
        {
            LoadFromJson();
        }
        else
        {
            Debug.Log("无存档，初始化新数据");
            Global.ResetData();
        }
    }

    //存档方法
    static void SaveByJson()
    {
        SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, SavingData());
    }

    //读取方法
    static void LoadFromJson()
    {
        try
        {
            var saveData = SaveSystem.LoadFromJson<SaveDate>(PLAYER_DATA_FILE_NAME);
            if (saveData == null)
            {
                Debug.Log("未找到存档，开始新游戏");
                Global.ResetData();
                return;
            }
            LoadData(saveData);
        }
        catch (FileNotFoundException)
        {
            Debug.Log("存档文件不存在，初始化新数据");
            Global.ResetData();
        }
    }


    //删除存档文件
    [UnityEditor.MenuItem("Developer/Delete Player Data Save File")]
    public static void DeletePlayerDateSaveFile()
    {
        SaveSystem.DeleteSaveFile(PLAYER_DATA_FILE_NAME);
    }

    public static bool DoesSaveFileExist()
{
    string filePath = Path.Combine(Application.persistentDataPath, PLAYER_DATA_FILE_NAME);
    return File.Exists(filePath);
}
    
}
