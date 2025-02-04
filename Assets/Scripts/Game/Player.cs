using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public partial class Player : ViewController
    {
        public enum States
        {
            Idle,
            Rolling,
        }

        public FSM<States> State = new FSM<States>();

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


        public static Player Default;//单例

        public List<AudioClip> GunTakeSfxs = new List<AudioClip>();//切枪音效

        private void Awake()
        {
            Application.targetFrameRate = 60;//设置帧率60
            //同一个音效要大于一定帧数才能播放，默认10帧
            AudioKit.PlaySoundMode = AudioKit.PlaySoundModes.IgnoreSameSoundInSoundFrames;

            Text.Hide();

            Default = this;


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
        
        public Gun GunWithKey(string key)
        {
            if (key == GunConfig.Pistol.Key)
                return Pistol;
            if (key == GunConfig.MP5.Key)
                return MP5;
            if (key == GunConfig.ShotGun.Key)
                return ShotGun;
            if (key == GunConfig.AK47.Key)
                return AK;
            if (key == GunConfig.Rocket.Key)
                return RocketGun;
            if (key == GunConfig.AWP.Key)
                return AWP;
            if (key == GunConfig.Laser.Key)
                return Laser;
            if (key == GunConfig.Bow.Key)
                return Bow;

            return null;
        }
        
        public void UseGun(int index)
        {
            var gunData = GunSystem.GunList[index];
            CurrentGun.Hide();
            CurrentGun = GunWithKey(gunData.Key);
            Global.CurrentGun = gunData;
            CurrentGun.WithData(gunData);
            CurrentGun.Show();
            CurrentGun.OnGunUsed();

            var gunTakeSfx = GunTakeSfxs.GetRandomItem();//随机获取一个切枪音效
            SelfAudioSource.clip = gunTakeSfx;
            SelfAudioSource.Play();

            Global.GunAdditionalCameraSize = CurrentGun.GunAdditionalCameraSize;
        }

        private void Start()
        {
            var gunIndex = GunSystem.GunList.FindIndex(g => g == Global.CurrentGun);
            UseGun(gunIndex);

            //初始化状态机
            State.State(States.Idle)
                .OnUpdate(() => 
                {

                    //获取鼠标点击位置
                    var mouseScreenPosition = Input.mousePosition;
                    //将屏幕坐标转为世界坐标
                    var mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                    //得到主角朝向鼠标的方向
                    var shootDirection = (mouseWorldPoint - transform.position).normalized;


                    if (Global.CurrentRoom && Global.CurrentRoom.Enemies.Count > 0)
                    {
                        mTargetEmeny = Global.CurrentRoom.Enemies
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
                    if (horizontal != 0 || vertical != 0)
                    {
                        //人物上下轻微抖动
                        AnimationHelper.UpDownAnimation(Sprite, 0.05f, 10, Time.frameCount, 0.35f);
                        //武器轻微抖动
                        AnimationHelper.UpDownAnimation(Weapon, 0.05f, 10, Time.frameCount);
                        //人物左右轻微摆动
                        AnimationHelper.RotateAnimation(Sprite, 3, 30, Time.frameCount);

                    }

                    //摄像机偏移
                    var offsetLength = (mouseWorldPoint - transform.position).magnitude;
                    Global.CameraPosOffset = (shootDirection * (3 + Mathf.Clamp(offsetLength * 0.15f, 0, 3))).ToVector2();

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
                                var index = GunSystem.GunList.FindIndex(gun => gun == CurrentGun.Data);//遍历GUN列表，获得当前武器的索引
                                index--;

                                if (index < 0)
                                {
                                    index = GunSystem.GunList.Count - 1;
                                }

                                UseGun(index);

                            }



                        }

                        if (Input.GetKeyDown(KeyCode.E))//切换下一把武器
                        {
                            if (!CurrentGun.Reloading)
                            {
                                var index = GunSystem.GunList.FindIndex(gun => gun == CurrentGun.Data);
                                index++;

                                if (index > GunSystem.GunList.Count - 1)
                                {
                                    index = 0;
                                }

                                UseGun(index);

                            }
                        }

                        if (Input.GetMouseButtonDown(1))
                        {
                            if(horizontal != 0 || vertical != 0)
                            {
                                State.ChangeState(States.Rolling);
                            }
                        }

                    }
                });

            var faceDirection = Vector2.zero;

            State.State(States.Rolling)
                .OnEnter(() =>
                {
                    SelfCircleCollider2D.Disable();//取消碰撞

                    var x = Input.GetAxis("Horizontal");
                    var y = Input.GetAxis("Vertical");

                    if(x != 0 || y != 0)
                    {
                        faceDirection = new Vector2(x, y).normalized;//获取人物朝向
                    }

                    ActionKit.Lerp(0, 1, 0.4f, (p) =>
                    { 
                        p = EaseUtility.InCubic(0, 1, p);

                        if (x > 0)
                        {
                            transform.LocalEulerAnglesZ(p * -360f);
                        }
                        else
                        {
                            transform.LocalEulerAnglesZ(p * 360f);
                        }

                    }, () =>
                    {
                        transform.LocalEulerAnglesZ(0);
                        State.ChangeState(States.Idle);

                    }).Start(this);
                })
                .OnFixedUpdate(() =>
                {
                    Rigidbody2D.velocity = faceDirection * 8;
                })
                .OnExit(() =>
                {
                    SelfCircleCollider2D.Enable();//恢复碰撞
                });

            State.StartState(States.Idle);

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

                GameUI.PlayerHurtFlashScreen();

            }
        }

        private IEnemy mTargetEmeny = null;

        void Update()
        {
            //状态机更新
            State.Update();

        }

        private void FixedUpdate()
        {
            //状态机更新
            State.FixedUpdate();
        }
    }

}