using QFramework.ProjectGungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
	public class Global
	{
		public static Player Player;

		public static Room CurrentRoom;

		public static GunDate CurrentGun;

		public static LevelConfig CurrentLevel;

		public static BindableProperty<int> HP = new BindableProperty<int>(6);

        public static BindableProperty<int> Armor = new BindableProperty<int>(1);

        //BindableProperty<T> 是QFramework框架提供的属性，本身能储存数据，又能监听数据变换事件
        public static BindableProperty<int> Coin = new BindableProperty<int>(0);

		public static DynaGrid<Room> RoomGrid { get; set; }

		public static bool UIOpened = false;
		public static bool CanShoot => !UIOpened;

		public static float GunAdditionalCameraSize;

        public static List<LevelConfig> Levels = new List<LevelConfig>()
		{
			Level1.Config,
			Level2.Config,
			Level3.Config,
			Level4.Config,
		};

		public static Queue<int> CurrentPacing = null;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void AutoInit()
		{
			ResetData();
		}

		public static void ResetData()
		{
			Coin.Value = 0;
			HP.Value = 6;
			Armor.Value = 1;
			Time.timeScale = 1;//恢复时间

			//清空武器库，并添加一把普通的手枪
			GunSystem.GunList.Clear();
			GunSystem.GunList.Add(GunConfig.Pistol.CreateData());
			Global.CurrentGun = GunSystem.GunList.First();

			CurrentLevel =Level1.Config;

            //将level1的PacingConfig配置赋值给此处CurrentPacing
            CurrentPacing = new Queue<int>(CurrentLevel.PacingConfig);
		}

		//true表示进入下一关，false表示通关
		public static bool NextLevel()
		{
			var levelIndex = Global.Levels.FindIndex(l => l == Global.CurrentLevel);

			levelIndex++;

			if(levelIndex == Global.Levels.Count)
			{
				//游戏通关

				return false;
			}
			else
			{
				CurrentLevel = Global.Levels[levelIndex];

				
			}

            return true;
        }
	}
}