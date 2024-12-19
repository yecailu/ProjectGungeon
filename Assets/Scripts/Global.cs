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

		public static void ResetData()
		{
			HP = 3;
			Time.timeScale = 1;//»Ö¸´Ê±¼ä
		}
	}
}