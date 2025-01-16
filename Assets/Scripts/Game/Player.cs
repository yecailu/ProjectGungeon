using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            yield return new WaitForSeconds(duration);//延迟
            Text.Hide();
        }

        public PlayerBullet PlayerBullet;//子弹

        public Rigidbody2D Rigidbody2D;

        public SpriteRenderer Sprite;

        public Transform Weapon;//武器位置

        public Gun CurrentGun;//当前武器

        public List<Gun> GunList = new List<Gun>();//武器列表

        public static Player Default;//单例

        public List<AudioClip> GunTakeSfxs = new List<AudioClip>();//切枪音效

        private void Awake()
        {
            Application.targetFrameRate = 60;//设置帧率60
            //同一个音效要大于一定帧数才能播放，默认10帧
            AudioKit.PlaySoundMode = AudioKit.PlaySoundModes.IgnoreSameSoundInSoundFrames;

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

            var gunTakeSfx =  GunTakeSfxs.GetRandomItem();//随机获取一个切枪音效
            SelfAudioSource.clip = gunTakeSfx;
            SelfAudioSource.Play(); 
        }

        void Start()
        {

        }

        public void Hurt(int damage)
        {

            if (Global.Armor.Value > 0)
            {
                if (Global.Armor.Value > damage)
                {
                    Global.Armor.Value -= damage;
                    damage = 0;

                    //播放Armor音效
                    AudioKit.PlaySound("resources://UseArmor");
                }
                else
                {
                    damage -= Global.Armor.Value;
                    Global.Armor.Value = 0;

                    //播放Armor音效
                    AudioKit.PlaySound("resources://UseArmor");

                }
            }

            if (damage > 0)
            {
                //播放受伤特效
                FxFactory.PlayHurtFx(transform.Position2D(), Color.green);
                FxFactory.PlayPlayerBlood(transform.Position2D());

                AudioKit.PlaySound("resources://PlayerHurt");

                Global.HP.Value -= damage;
                if (Global.HP.Value <= 0)
                {
                    Global.HP.Value = 0;
                    GameUI.Default.GameOver.SetActive(true);
                    Time.timeScale = 0;

                }

            }
        }

        private IEnemy mTargetEmeny = null;

        void Update()
        {

            //获取鼠标点击位置
            var mouseScreenPosition = Input.mousePosition;
            //将屏幕坐标转为世界坐标
            var mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            //得到主角朝向鼠标的方向
            var shootDirection = (mouseWorldPoint - transform.position).normalized;


            if (Global.currentRoom && Global.currentRoom.Enemies.Count > 0)
            {
                mTargetEmeny = Global.currentRoom.Enemies
                    .OrderBy(e => (e.GameObject.Position2D() - mouseWorldPoint.ToVector2()).magnitude)//根据距离远近排序
                    .FirstOrDefault(e =>
                    {
                        var direction = this.Direction2DTo(e.GameObject);

                        if (Physics2D.Raycast(this.Position2D(), direction.normalized, direction.magnitude,
                            LayerMask.GetMask("Wall")))
                        {
                            return false;
                        }
                        return true;
                    });

                if (mTargetEmeny != null && mTargetEmeny.GameObject)
                {
                    shootDirection = this.NormalizedDirection2DTo(mTargetEmeny.GameObject);
                    Aim.Position2D(mTargetEmeny.GameObject.Position2D());
                    Aim.Show();
                }
                else
                {
                    Aim.Hide();
                }
            }
            else
            {
                Aim.Hide();
            }


            //弧度
            var radius = Mathf.Atan2(shootDirection.y, shootDirection.x);
            //欧拉角
            var eulerAngles = radius * Mathf.Rad2Deg;
            //设置给Weapon
            Weapon.localRotation = Quaternion.Euler(0, 0, eulerAngles);

            //武器翻转,人物朝向随武器方向
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



            //移动逻辑
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            Rigidbody2D.velocity = new Vector2(horizontal, vertical).normalized * 5;

            //走路时人物效果
            if(horizontal != 0 || vertical != 0)
            {
                //人物上下轻微抖动
                AnimationHelper.UpDownAnimation(Sprite, 0.05f, 10, Time.frameCount, 0.35f);
                //武器轻微抖动
                AnimationHelper.UpDownAnimation(Weapon, 0.05f, 10, Time.frameCount);
                //人物左右轻微摆动
                AnimationHelper.RotateAnimation(Sprite, 3, 30, Time.frameCount);

            }
             

            if (Global.CanShoot)
            {
                if (Input.GetMouseButtonDown(0))//按左键发射子弹
                {
                    CurrentGun.ShootDown(shootDirection);//调用子弹发射方法

                }

                if (Input.GetMouseButton(0))//按左键发射子弹
                {
                    CurrentGun.Shooting(shootDirection);//调用子弹发射方法

                }

                if (Input.GetMouseButtonUp(0))//按左键发射子弹
                {
                    CurrentGun.ShootUp(shootDirection);//调用子弹发射方法

                }

                if (Input.GetKeyDown(KeyCode.R))//换弹
                {
                    CurrentGun.Reload();
                }

                if (Input.GetKeyDown(KeyCode.Q))//切换上一把武器
                {
                    if (!CurrentGun.Reloading)
                    {
                        var index = GunList.FindIndex(gun => gun == CurrentGun);//遍历GUN列表，获得当前武器的索引
                        index--;

                        if (index < 0)
                        {
                            index = GunList.Count - 1;
                        }

                        UseGun(index);

                    }



                }

                if (Input.GetKeyDown(KeyCode.E))//切换下一把武器
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

}