using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDate :MonoBehaviour
{
    [System.Serializable]
    class SaveDate
    {
        public int Coin;
        public int HP;
        public int Armor;
        public int Key;



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

    }


    public static void Save()
    {
        SaveByJson();
    }

    public static void Load()
    {
        LoadFromJson();
    }

    //存档方法
    static void SaveByJson()
    {
        SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, SavingData());
    }

    //读取方法
    static void LoadFromJson()
    {
        var saveDate = SaveSystem.LoadFromJson<SaveDate>(PLAYER_DATA_FILE_NAME);

        LoadData(saveDate);
    }


    //删除存档文件
    [UnityEditor.MenuItem("Developer/Delete Player Data Save File")]
    public static void DeletePlayerDateSaveFile()
    {
        SaveSystem.DeleteSaveFile(PLAYER_DATA_FILE_NAME);
    }


}
