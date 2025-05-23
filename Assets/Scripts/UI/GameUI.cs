using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
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
        public GameObject CheatPanel;//作弊面板
        public GameObject SettingPanel;//设置面板

        public GameObject VolumePanel;//音量面板
        public GameObject KeyPanel;//按键面板


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

        //更新枪械UI信息
        public static void UpdateGunInfo(GunClip gunClip)
        {
            var data = Player.Default.CurrentGun.Data;
            Default.Icon.sprite = Player.Default?.CurrentGun?.Sprite?.sprite;
            Default.smallIcon.sprite = Player.Default?.CurrentGun?.Sprite?.sprite;
            if (data.Config.GunBagMaxBulletCount == -1)
            {
                if (data.Reloading)
                {

                    Default.BulletText.text = "(<size=30>换弹中</size>) \u221e";
                }
                else
                {

                    Default.BulletText.text =
                        $"({gunClip.Data.CurrentBulletCount}/{gunClip.Data.Config.ClipBulletCount}) \u221e";
                }

            }
            else
            {
                if (data.Reloading)
                {
                    Default.BulletText.text = $"(<size=30>换弹中</size>) {data.GunBagRemainBulletCount}/{data.Config.GunBagMaxBulletCount}";
                }
                else
                {
                    Default.BulletText.text =
                        $"({gunClip.Data.CurrentBulletCount}/{gunClip.Data.Config.ClipBulletCount}) {data.GunBagRemainBulletCount}/{data.Config.GunBagMaxBulletCount}";

                }
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
            //存储所有音乐资源路径
            var list = new List<string>()
            {
                //"resources://music/darkascent",
                "resources://Music/DOS-88 - Automatav2",
                "resources://Music/FlowState",
                "resources://Music/Night Life",
                "resources://Music/OnlyInDreams",
                "resources://Music/Rest Easy",
                "resources://Music/Smooth Sailing",               
                "resources://Music/Checking Instruments",
                "resources://Music/D0S-88 - Marathon Man",
            };
            //播放随机音乐
            AudioKit.PlayMusic(list.GetRandomItem(), volume: 0.2f);


            GamePass.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//重置数据
                SceneManager.LoadScene("SampleScene");//重新加载场景

            });

            GamePass.transform.Find("BtnRestartMenu").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//重置数据
                AudioKit.StopMusic();
                SceneManager.LoadScene(0);

            });


            GameOver.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
            {

                if (PlayerDate.DoesSaveFileExist())
                {
                    PlayerDate.Load();
                    Time.timeScale = 1;//恢复时间
                }
                else
                {
                    Global.ResetData();//重置数据
                }
                SceneManager.LoadScene("SampleScene");

            });

            GameOver.transform.Find("BtnRestartMenu").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//重置数据
                AudioKit.StopMusic();
                SceneManager.LoadScene(0);

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

            Global.Color.RegisterWithInitValue((color) =>
            {
                ColorText.text = color.ToString();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

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
                    if (!Global.UIOpened)
                    {
                        UIMap.Show();
                    }
                }
            }



            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (CheatPanel.activeSelf)
                {
                    CheatPanel.SetActive(false);
                    Global.UIOpened = false;
                    Time.timeScale = 1;//恢复时间
                }
                else
                {
                    CheatPanel.SetActive(true);
                    Global.UIOpened = true;
                    Time.timeScale = 0;//暂停时间
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SettingPanel.activeSelf)
                {
                    VolumePanel.SetActive(false);
                    KeyPanel.SetActive(false);
                    SettingPanel.SetActive(false);
                    Global.UIOpened = false;
                    Time.timeScale = 1;//恢复时间
                }
                else
                {
                    SettingPanel.SetActive(true);
                    Global.UIOpened = true;
                    Time.timeScale = 0;//暂停时间
                }
            }

        }
    }
}