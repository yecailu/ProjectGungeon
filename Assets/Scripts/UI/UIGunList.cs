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
			public Sprite Icon => Player.Default.GunWithKey(Key).Sprite.sprite;
			public bool InitUnlockState;
		}

		private List<GunBaseItem> mGunBaseItems = new List<GunBaseItem>()
		{
			new(){ Name = "Shotgun 喷子", Price = 0, Key = GunConfig.ShotGun.Key, InitUnlockState = true },
			new(){ Name = "MP5 冲锋枪", Price = 0, Key = GunConfig.MP5.Key, InitUnlockState = true },
			new(){ Name = "AK47 自动步枪", Price = 5, Key = GunConfig.AK47.Key, InitUnlockState = false },
			new(){ Name = "AWP 狙击枪", Price = 6, Key = GunConfig.AWP.Key, InitUnlockState = false },
			new(){ Name = "Laser 激光枪", Price = 7, Key = GunConfig.Laser.Key, InitUnlockState = false },
			new(){ Name = "Bow 弓箭", Price = 10, Key = GunConfig.Bow.Key, InitUnlockState = false },
			new(){ Name = "Rocket 火箭筒", Price = 15, Key = GunConfig.Rocket.Key, InitUnlockState = false },
		};

		private static readonly int GrayValue = Shader.PropertyToID("_GrayValue");

        private void Awake()
        {
            foreach(var gunBaseItem in mGunBaseItems)
			{
				gunBaseItem.Unlocked = 
					PlayerPrefs.GetInt(gunBaseItem.Key + "_unlocked", gunBaseItem.InitUnlockState ? 1 : 0) == 1;
			}
        }

		void Save()
		{
            foreach (var gunBaseItem in mGunBaseItems)
            {
                PlayerPrefs.SetInt(gunBaseItem.Key + "_unlocked", gunBaseItem.Unlocked ? 1 : 0);
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
				gunItem.Icon.sprite = gunBaseItem.Icon;

				if (gunBaseItem.Unlocked)
                {
					gunItem.PriceText.Hide();
					gunItem.ColorIcon.Hide();
                    //设置灰度值0
                    gunItem.Icon.material = gunItem.Icon.material.Instantiate();
					gunItem.Icon.material.SetFloat(GrayValue, 0);

                }
				else
				{
					gunItem.PriceText.text = "x" + gunBaseItem.Price;
					//设置灰度值1
                    gunItem.Icon.material = gunItem.Icon.material.Instantiate();
                    gunItem.Icon.material.SetFloat(GrayValue, 1);

                    var cachedItem = gunBaseItem;
					var cachedItemView = gunItem;	
					gunItem.BtnUnlock.onClick.AddListener(() =>
                    {
						if(cachedItem.Price <= Global.Color.Value)
						{
							cachedItem.Unlocked = true;
							Player.DisplayText("<color=yellow>" + cachedItem.Name + "</color> 已解锁", 3);
                            cachedItemView.BtnUnlock.Hide();
                            gunItem.Icon.material.SetFloat(GrayValue, 0);
                            Global.Color.Value -= cachedItem.Price;
							AudioKit.PlaySound("resources://UnlockGun");
							Save();
						}
						else
						{
                            Player.DisplayText("颜色不足", 2);

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
