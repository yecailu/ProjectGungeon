using QFramework.ProjectGungeon;
using System;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
	public class Global
	{
		public static Player Player;

		public static Room currentRoom;

		public static int HP = 3;

		public static Action HPChangedEvent;

        //BindableProperty<T> ��QFramework����ṩ�����ԣ������ܴ������ݣ����ܼ������ݱ任�¼�
        public static BindableProperty<int> Coin = new BindableProperty<int>(0);

		public static DynaGrid<Room> RoomGrid { get; set; }

		public static void ResetData()
		{
			Coin.Value = 0;
			HP = 3;
			Time.timeScale = 1;//�ָ�ʱ��
		}
	}
}