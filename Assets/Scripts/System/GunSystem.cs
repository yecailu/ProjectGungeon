using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static QFramework.ProjectGungeon.UIGunList;

namespace QFramework.ProjectGungeon
{
    public class GunConfig
    {
        public string Key;
        public string Name;

        public int ClipBulletCount;
        public int GunBagMaxBulletCount;

        public static GunConfig Pistol = new GunConfig()
        {
            Name = "ÊÖÇ¹",
            Key = "pistol",
            ClipBulletCount = 10,
            GunBagMaxBulletCount = -1
        };

        public static GunConfig MP5 = new GunConfig()
        {
            Name = "MP5",
            Key = "mp5",
            ClipBulletCount = 30,
            GunBagMaxBulletCount = 500
        };
        public static GunConfig ShotGun = new GunConfig()
        {
            Name = "ö±µ¯Ç¹",
            Key = "shotgun",
            ClipBulletCount = 5,
            GunBagMaxBulletCount = 100
        };
        public static GunConfig AK47 = new GunConfig()
        {
            Name = "AK47",
            Key = "ak47",
            ClipBulletCount = 30,
            GunBagMaxBulletCount = 500
        };
        public static GunConfig AWP = new GunConfig()
        {
            Name = "¾Ñ»÷Ç¹",
            Key = "awp",
            ClipBulletCount = 5,
            GunBagMaxBulletCount = 50
        };
        public static GunConfig Laser = new GunConfig()
        {
            Name = "¼¤¹âÇ¹",
            Key = "laser",
            ClipBulletCount = 50,
            GunBagMaxBulletCount = 500
        };
        public static GunConfig Bow = new GunConfig()
        {
            Name = "¹­¼ý",
            Key = "bow",
            ClipBulletCount = 3,
            GunBagMaxBulletCount = 100
        };
        public static GunConfig Rocket = new GunConfig()
        {
            Name = "»ð¼ýÍ²",
            Key = "rocket",
            ClipBulletCount = 1,
            GunBagMaxBulletCount = 50
        };

        public static List<GunConfig> configs = new List<GunConfig>()
        {
            GunConfig.Rocket,
            GunConfig.Bow,
            GunConfig.Laser,
            GunConfig.ShotGun,
            GunConfig.AK47,
            GunConfig.AWP,
            GunConfig.MP5
        };

        public GunDate CreateData()
        {
            return new GunDate()
            {
                Name = "",
                Key = Key,
                Config = this,
                CurrentBulletCount = this.ClipBulletCount,
                GunBagRemainBulletCount = this.GunBagMaxBulletCount,

            };
        }
    }

    [Serializable]
    public class GunDate
    {
        public string Name;
        public string Key;
        //µ±Ç°×Óµ¯ÊýÁ¿
        public int CurrentBulletCount;
        //Ç¹°üÀïÊ£Óà×Óµ¯ÊýÁ¿
        public int GunBagRemainBulletCount;

        public GunConfig Config;

        public bool Reloading;
    }

    public class GunSystem :AbstractSystem
    {
        public static List<GunDate> GunList = new List<GunDate>();
        //{
        //    GunConfig.Pistol.CreateData(),
        //    GunConfig.MP5.CreateData(),
        //    GunConfig.ShotGun.CreateData(),
        //    GunConfig.AK47.CreateData(),
        //    GunConfig.AWP.CreateData(),
        //    GunConfig.Laser.CreateData(),
        //    GunConfig.Bow.CreateData(),
        //    GunConfig.Rocket.CreateData()
        //};

        public  List<GunBaseItem> GunBaseItems => mGunBaseItems;

        void Load()
        {
            foreach (var gunBaseItem in mGunBaseItems)
            {
                gunBaseItem.Unlocked =
                    PlayerPrefs.GetInt(gunBaseItem.Key + "_unlocked", gunBaseItem.InitUnlockState ? 1 : 0) == 1;
            }
        }

        public void Save()
        {
            foreach (var gunBaseItem in mGunBaseItems)
            {
                PlayerPrefs.SetInt(gunBaseItem.Key + "_unlocked", gunBaseItem.Unlocked ? 1 : 0);
            }
        }

        static List<GunBaseItem> mGunBaseItems = new List<GunBaseItem>()
        {
            new(){ Name = "Shotgun Åç×Ó", Price = 0, Key = GunConfig.ShotGun.Key, InitUnlockState = true },
            new(){ Name = "MP5 ³å·æÇ¹", Price = 0, Key = GunConfig.MP5.Key, InitUnlockState = true },
            new(){ Name = "AK47 ×Ô¶¯²½Ç¹", Price = 5, Key = GunConfig.AK47.Key, InitUnlockState = false },
            new(){ Name = "AWP ¾Ñ»÷Ç¹", Price = 6, Key = GunConfig.AWP.Key, InitUnlockState = false },
            new(){ Name = "Laser ¼¤¹âÇ¹", Price = 7, Key = GunConfig.Laser.Key, InitUnlockState = false },
            new(){ Name = "Bow ¹­¼ý", Price = 10, Key = GunConfig.Bow.Key, InitUnlockState = false },
            new(){ Name = "Rocket »ð¼ýÍ²", Price = 15, Key = GunConfig.Rocket.Key, InitUnlockState = false },
        };

        public static List<GunConfig> GetAvailableGuns()
        {
            var availableGunConfigs = new List<GunConfig>();

            foreach(var gunConfig in GunConfig.configs)
            {

                var gunBaseItem = mGunBaseItems.First(gun => gun.Key == gunConfig.Key);
                if (gunBaseItem.Unlocked && GunList.All(g => g.Key != gunConfig.Key))
                {
                    availableGunConfigs.Add(gunConfig);
                }

            }

            return availableGunConfigs;
        }

        protected override void OnInit()
        {
            Load();
        }
    }
}
