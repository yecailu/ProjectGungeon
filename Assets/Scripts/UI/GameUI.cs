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
        public static GameUI Default;//����

        public GameObject GamePass;//ͨ�ؽ���
        public GameObject GameOver;//ʧ�ܽ���

        public Text HP;

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
                Global.ResetData();//��������
                SceneManager.LoadScene("SampleScene");//���¼��س���

            });

            GameOver.transform.Find("BtnRestart").GetComponent<Button>().onClick.AddListener(() =>
            {
                Global.ResetData();//��������
                SceneManager.LoadScene("SampleScene");

            });
;

            Global.HP.RegisterWithInitValue(hp =>
            {
                UpdateHP();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Armor.RegisterWithInitValue(armor =>
            {
                Armor.text = "����:" + armor;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            //ע��Coin ��������{����}.���ٺ�ȡ��ע��
            Global.Coin.RegisterWithInitValue((coin) =>
            {
                CoinText.text = coin.ToString();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Key.RegisterWithInitValue((key) =>
            {
                KeyText.text = key.ToString();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }



        void UpdateHP()
        {
            HP.text = $"HP:({Global.HP.Value}/{Global.MaxHP.Value})";
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