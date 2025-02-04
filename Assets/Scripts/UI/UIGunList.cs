using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.ProjectGungeon
{
	public partial class UIGunList : ViewController
	{
		public class GunBaseItem
		{
			public string Name;
            public string Key;
			public int Price;
			public bool Unlocked = false;
            public Sprite Icon;
			public bool InitUnlockState;
		}

		private List<GunBaseItem> mGunBaseItems = new List<GunBaseItem>()
		{
			new(){ Name = "Shotgun Åç×Ó", Price = 0, Key = GunConfig.ShotGun.Key, InitUnlockState = true },
			new(){ Name = "MP5 ³å·æÇ¹", Price = 0, Key = GunConfig.MP5.Key, InitUnlockState = true },
			new(){ Name = "AK47 ×Ô¶¯²½Ç¹", Price = 5, Key = GunConfig.AK47.Key, InitUnlockState = false },
			new(){ Name = "AWP ¾Ñ»÷Ç¹", Price = 6, Key = GunConfig.AWP.Key, InitUnlockState = false },
			new(){ Name = "Laser ¼¤¹âÇ¹", Price = 7, Key = GunConfig.Laser.Key, InitUnlockState = false },
			new(){ Name = "Bow ¹­¼ý", Price = 10, Key = GunConfig.Bow.Key, InitUnlockState = false },
			new(){ Name = "Rocket »ð¼ýÍ²", Price = 15, Key = GunConfig.Rocket.Key, InitUnlockState = false },
		};

        private void Awake()
        {
            foreach(var gunBaseItem in mGunBaseItems)
			{
				gunBaseItem.Unlocked = gunBaseItem.InitUnlockState;
			}
        }

        private void OnEnable()
        {
			Global.UIOpened = true;
			GunItemRoot.DestroyChildren();

            foreach (var gunBaseItem in mGunBaseItems)
            {
				var gunItem = GunItem.InstantiateWithParent(GunItemRoot)
					.Show();

				gunItem.Name.text = gunBaseItem.Name;
				gunItem.BtnUnlock.gameObject.SetActive(!gunBaseItem.Unlocked);
				//gunItem.Icon.sprite = gunBaseItem.Icon;
				
                if (gunBaseItem.Unlocked)
                {
					gunItem.PriceText.Hide();
					gunItem.ColorIcon.Hide();
					gunItem.Icon.color = Color.white;

                }
				else
				{
					gunItem.PriceText.text = "x" + gunBaseItem.Price;
					gunItem.Icon.color = Color.gray;

					var cachedItem = gunBaseItem;
					var cachedItemView = gunItem;	
					gunItem.BtnUnlock.onClick.AddListener(() =>
                    {
						if(cachedItem.Price <= Global.Color.Value)
						{
							cachedItem.Unlocked = true;
							Player.DisplayText("<color=yellow>" + cachedItem.Name + "</color> ÒÑ½âËø", 3);
                            cachedItemView.BtnUnlock.Hide();
                            cachedItemView.Icon.color = Color.gray;
							Global.Color.Value -= cachedItem.Price;
							AudioKit.PlaySound("resources://UnlockGun");
						}
						else
						{
                            Player.DisplayText("ÑÕÉ«²»×ã", 2);

                        }
                    });
				}
            }
        }

        private void OnDisable()
        {
            Global.UIOpened = false;
        }
    }
}
