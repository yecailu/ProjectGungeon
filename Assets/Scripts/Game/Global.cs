using System;
using UnityEngine;

public class Global
{
	public static Player Player;

	public static int HP = 3;

	public static Action HPChangedEvent;

	public static void ResetData()
	{
		HP = 3;
        Time.timeScale = 1;//»Ö¸´Ê±¼ä
    }
}