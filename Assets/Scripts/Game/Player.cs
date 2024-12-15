using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{

    public partial class Player : ViewController
    {
        public static void DisplayText(string text, float duration)
        {
            Default.StartCoroutine(Default.DoDisplayText(text, duration));
        }

        IEnumerator DoDisplayText(string text, float duration)
        {
            Text.text = text;
            Text.Show();
            yield return new WaitForSeconds(duration);//�ӳ�
            Text.Hide();
        }

        public PlayerBullet PlayerBullet;//�ӵ�

        public Rigidbody2D Rigidbody2D;

        public SpriteRenderer Sprite;

        public Transform Weapon;//����λ��

        public Gun CurrentGun;//��ǰ����

        public List<Gun> GunList = new List<Gun>();//�����б�

        public static Player Default;//����

        public List<AudioClip> GunTakeSfxs = new List<AudioClip>();//��ǹ��Ч

        private void Awake()
        {
            Application.targetFrameRate = 60;//����֡��60

            Text.Hide();

            Default = this;
            GunList.Add(Pistol);
            GunList.Add(AK);
            GunList.Add(AWP);
            GunList.Add(ShotGun);
            GunList.Add(MP5);
            GunList.Add(RocketGun);
            GunList.Add(Bow);
            GunList.Add(Laser);

            GunTakeSfxs.Add(GunTake1);
            GunTakeSfxs.Add(GunTake2);
            GunTakeSfxs.Add(GunTake3);
            GunTakeSfxs.Add(GunTake4);
            GunTakeSfxs.Add(GunTake5);

            UseGun(0);
        }

        private void OnDestroy()
        {
            Default = null;
        }

        void UseGun(int index)
        {
            CurrentGun.Hide();
            CurrentGun = GunList[index];
            CurrentGun.Show();
            CurrentGun.OnGunUsed();

            var gunTakeSfx =  GunTakeSfxs.GetRandomItem();//�����ȡһ����ǹ��Ч
            SelfAudioSource.clip = gunTakeSfx;
            SelfAudioSource.Play(); 
        }

        void Start()
        {

        }

        public void Hurt(int damage)
        {
            Global.HP -= damage;
            if (Global.HP <= 0)
            {
                Global.HP = 0;
                GameUI.Default.GameOver.SetActive(true);
                Time.timeScale = 0;

            }
            Global.HPChangedEvent();//����HP�ı䷽��   
        }

        void Update()
        {

            //��ȡ�����λ��
            var mouseScreenPosition = Input.mousePosition;
            //����Ļ����תΪ��������
            var mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            //�õ����ǳ������ķ���
            var shootDirection = (mouseWorldPoint - transform.position).normalized;

            //����
            var radius = Mathf.Atan2(shootDirection.y, shootDirection.x);
            //ŷ����
            var eulerAngles = radius * Mathf.Rad2Deg;
            //���ø�Weapon
            Weapon.localRotation = Quaternion.Euler(0, 0, eulerAngles);


            //������ת,���ﳯ������������
            if (shootDirection.x > 0)
            {
                Weapon.transform.localScale = new Vector3(1, 1, 1);
                Sprite.flipX = false;
            }
            else if (shootDirection.x < 0) 
            {
                Weapon.transform.localScale = new Vector3(1, -1, 1);
                Sprite.flipX = true;
            }



            //�ƶ��߼�
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            Rigidbody2D.velocity = new Vector2(horizontal, vertical).normalized * 5;



            if (Input.GetMouseButtonDown(0))//����������ӵ�
            {
                CurrentGun.ShootDown(shootDirection);//�����ӵ����䷽��

            }

            if (Input.GetMouseButton(0))//����������ӵ�
            {
                CurrentGun.Shooting(shootDirection);//�����ӵ����䷽��

            }

            if (Input.GetMouseButtonUp(0))//����������ӵ�
            {
                CurrentGun.ShootUp(shootDirection);//�����ӵ����䷽��

            }

            if (Input.GetKeyDown(KeyCode.R))//����
            {
                CurrentGun.Reload();
            }

            if (Input.GetKeyDown(KeyCode.Q))//�л���һ������
            {
                if (!CurrentGun.Reloading)
                {
                    var index = GunList.FindIndex(gun => gun == CurrentGun);//����GUN�б���õ�ǰ����������
                    index--;

                    if (index < 0)
                    {
                        index = GunList.Count - 1;
                    }

                    UseGun(index);

                }



            }

            if (Input.GetKeyDown(KeyCode.E))//�л���һ������
            {
                if (!CurrentGun.Reloading)
                {
                    var index = GunList.FindIndex(gun => gun == CurrentGun);
                    index++;

                    if (index > GunList.Count - 1)
                    {
                        index = 0;
                    }

                    UseGun(index);

                }
            }
        }
    }

}