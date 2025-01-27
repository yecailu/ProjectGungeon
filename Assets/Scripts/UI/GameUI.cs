using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace QFramework.ProjectGungeon
{

    public partial class GameUI : ViewController
    {
        public static GameUI Default;//单例

        public GameObject GamePass;//通关界面
        public GameObject GameOver;//失败界面


        public static void PlayerHurtFlashScreen()
        {
            //透明度从0到1，然后从1到0，最后变为原来的颜色
            ActionKit.Sequence()
                .Lerp01(0.01f, p =>
                {
                    Default.ScreenColor.ColorAlpha(p);
                }, () =>
                {
                    Default.ScreenColor.ColorAlpha(1);
                })
                .Lerp(1, 0, 0.3f, p =>
                {
                    Default.ScreenColor.ColorAlpha(p);
                }, () =>
                {
                    Default.ScreenColor.ColorAlpha(0);
                })
                .StartCurrentScene()
                .IgnoreTimeScale();
        }

        public static void UpdateGunInfo(GunClip gunClip)
        {
            var bulletBag = (Player.Default.CurrentGun).BulletBag;
            if(bulletBag.MaxBulletCount == -1)
            {
                Default.GunInfo.text = $"Bullet:({gunClip.Data.CurrentBulletCount}/{gunClip.Data.Config.ClipBulletCount}) (\u221e)";
            }
            else
            {
                Default.GunInfo.text = $"Bullet:({gunClip.Data.CurrentBulletCount}/{gunClip.Data.Config.ClipBulletCount}) ({bulletBag.Data.GunBagRemainBulletCount}/{bulletBag.MaxBulletCount})"; 
            }
        }
         
        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;

        }



        void Start()
        {
            GamePass.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//重置数据
                SceneManager.LoadScene("SampleScene");//重新加载场景

            });

            GameOver.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//重置数据
                SceneManager.LoadScene("SampleScene");

            });


            //注册Coin 变更后调用{方法}.销毁后取消注册
            Global.Coin.RegisterWithInitValue((coin) =>
            {
                CoinText.text = coin.ToString();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Key.RegisterWithInitValue((key) =>
            {
                KeyText.text = key.ToString();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            //HP 、Armor 、MaxHP 任意一个值改变时调用{方法}
            Global.Armor.Or(Global.HP).Or(Global.MaxHP).Register(() =>
            {
                UpdateHPAndArmorView();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            UpdateHPAndArmorView();

        }


        //更新HP和Armor的显示
        void UpdateHPAndArmorView()
        {
            HPArmorBg.DestroyChildrenWithCondition(item => item != HP.transform && item != Armor.transform);

            for (int i = 0; i < Global.MaxHP.Value / 2; i++)
            {
                var hp = HP.InstantiateWithParent(HPArmorBg)
                    .Show();

                var result = Global.HP.Value - i * 2;
                var image = hp.transform.Find("Value").GetComponent<Image>();

                if (result > 0)
                {
                    if (result == 1)
                    {
                        image.fillAmount = 0.5f;
                    }
                    else
                    {
                        image.fillAmount = 1;
                    }
                }
                else
                {
                    image.fillAmount = 0;
                }
            }

            for (int i = 0; i < Global.Armor.Value; i++)
            {
                Armor.InstantiateWithParent(HPArmorBg) 
                    .Show();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (UIMap.gameObject.activeSelf)
                {
                    UIMap.Hide();
                }
                else
                {
                    UIMap.Show();
                }
            }
        }

    }

}