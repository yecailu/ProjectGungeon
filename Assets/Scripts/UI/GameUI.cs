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
        public static GameUI Default;//����

        public GameObject GamePass;//ͨ�ؽ���
        public GameObject GameOver;//ʧ�ܽ���


        public static void PlayerHurtFlashScreen()
        {
            //͸���ȴ�0��1��Ȼ���1��0������Ϊԭ������ɫ
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

        //����ǹеUI��Ϣ
        public static void UpdateGunInfo(GunClip gunClip)
        {
            var data = Player.Default.CurrentGun.Data;
            Default.Icon.sprite = Player.Default?.CurrentGun?.Sprite?.sprite;
            if(data.Config.GunBagMaxBulletCount == -1)
            {
                if (data.Reloading)
                {

                    Default.BulletText.text = "(<size=24>Reloading</size>) \u221e";
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
                    Default.BulletText.text = $"(<size=24>Reloading</size>) {data.GunBagRemainBulletCount}/{data.Config.GunBagMaxBulletCount}";
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
            //�洢����������Դ·��
            var list = new List<string>() 
            {
                "resources://Music/darkascent",
                "resources://Music/DOS-88 - Automatav2",
                "resources://Music/DOS-88 - Press Start",
                "resources://Music/FlowState",
                "resources://Music/Night Life",
                "resources://Music/OnlyInDreams",
                "resources://Music/Rest Easy",
                "resources://Music/Smooth Sailing",
                "resources://Music/UndergroundConcourse",
                "resources://Music/Checking Instruments", 
                "resources://Music/D0S-88 - Marathon Man",
            };
            //�����������
            AudioKit.PlayMusic(list.GetRandomItem(),volume:0.2f);


            GamePass.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//��������
                SceneManager.LoadScene("SampleScene");//���¼��س���

            });

            GameOver.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//��������
                SceneManager.LoadScene("SampleScene");

            });


            //ע��Coin ��������{����}.���ٺ�ȡ��ע��
            Global.Coin.RegisterWithInitValue((coin) =>
            {
                CoinText.text = coin.ToString();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Key.RegisterWithInitValue((key) =>
            {
                KeyText.text = key.ToString();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            //HP ��Armor ��MaxHP ����һ��ֵ�ı�ʱ����{����}
            Global.Armor.Or(Global.HP).Or(Global.MaxHP).Register(() =>
            {
                UpdateHPAndArmorView();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            UpdateHPAndArmorView();

        }


        //����HP��Armor����ʾ
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