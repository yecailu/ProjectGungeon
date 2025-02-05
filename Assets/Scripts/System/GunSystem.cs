using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class GunConfig
    {
        public string Key;

        public int ClipBulletCount;
        public int GunBagMaxBulletCount;

        public static GunConfig Pistol = new GunConfig()
        {
            Key = "pistol",
            ClipBulletCount = 10,
            GunBagMaxBulletCount = -1
        };

        public static GunConfig MP5 = new GunConfig()
        {
            Key = "mp5",
            ClipBulletCount = 30,
            GunBagMaxBulletCount = 500
        };
        public static GunConfig ShotGun = new GunConfig()
        {
            Key = "shotgun",
            ClipBulletCount = 5,
            GunBagMaxBulletCount = 100
        };
        public static GunConfig AK47 = new GunConfig()
        {
            Key = "ak47",
            ClipBulletCount = 30,
            GunBagMaxBulletCount = 500
        };
        public static GunConfig AWP = new GunConfig()
        {
            Key = "awp",
            ClipBulletCount = 5,
            GunBagMaxBulletCount = 50
        };
        public static GunConfig Laser = new GunConfig()
        {
            Key = "laser",
            ClipBulletCount = 50,
            GunBagMaxBulletCount = 500
        };
        public static GunConfig Bow = new GunConfig()
        {
            Key = "bow",
            ClipBulletCount = 3,
            GunBagMaxBulletCount = 100
        };
        public static GunConfig Rocket = new GunConfig()
        {
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
                Key = Key,
                Config = this,
                CurrentBulletCount = this.ClipBulletCount,
                GunBagRemainBulletCount = this.GunBagMaxBulletCount,

            };
        }
    }

    public class GunDate
    {
        public string Key;
        //当前子弹数量
        public int CurrentBulletCount;
        //枪包里剩余子弹数量
        public int GunBagRemainBulletCount;

        public GunConfig Config;

        public bool Reloading;
    }

    public class GunSystem
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

        public static List<GunConfig> GetAvailableGuns()
        {
            var availableGunConfigs = new List<GunConfig>();

            foreach(var gunConfig in GunConfig.configs)
            {

                var gunBaseItem = UIGunList.GunBaseItems.First(gun => gun.Key == gunConfig.Key);
                if (gunBaseItem.Unlocked && GunList.All(g => g.Key != gunConfig.Key))
                {
                    availableGunConfigs.Add(gunConfig);
                }

            }

            return availableGunConfigs;
        }

    }
}
