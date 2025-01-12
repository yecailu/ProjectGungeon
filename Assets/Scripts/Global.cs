using QFramework.ProjectGungeon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
	public class Global
	{
		public static Player Player;

		public static Room currentRoom;

		public static BindableProperty<int> HP = new BindableProperty<int>(6);

        public static BindableProperty<int> Armor = new BindableProperty<int>(1);

        //BindableProperty<T> 是QFramework框架提供的属性，本身能储存数据，又能监听数据变换事件
        public static BindableProperty<int> Coin = new BindableProperty<int>(0);

		public static DynaGrid<Room> RoomGrid { get; set; }

		public static bool UIOpened = false;
		public static bool CanShoot => !UIOpened;

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

            //将level1的PacingConfig配置赋值给此处CurrentPacing
            CurrentPacing = new Queue<int>(Level1.Config.PacingConfig);
		}
	}
}