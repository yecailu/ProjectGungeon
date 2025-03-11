using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.ProjectGungeon
{
	public partial class UIGunList : ViewController,IController
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

		
		private static readonly int GrayValue = Shader.PropertyToID("_GrayValue");

		private GunSystem mGunSystem;

        private void Awake()
        {
            mGunSystem = this.GetSystem<GunSystem>();

        }

	

        private void OnEnable()
        {
			Global.UIOpened = true;
			GunItemRoot.DestroyChildren();

            foreach (var gunBaseItem in mGunSystem.GunBaseItems)
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
							mGunSystem.Save();
						}
						else
						{
                            Player.DisplayText("钻石不足", 2);

                        }
                    });
				}
            }
        }

        private void OnDisable()
        {
            Global.UIOpened = false;
        }

        public IArchitecture GetArchitecture()
        {
			return Global.Interface;
        }
    }
}
