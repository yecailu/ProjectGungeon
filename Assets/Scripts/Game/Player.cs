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
            yield return new WaitForSeconds(duration);//�ӳ�
            Text.Hide();
        }

        public PlayerBullet PlayerBullet;//�ӵ�

        public Rigidbody2D Rigidbody2D;

        public SpriteRenderer Sprite;

        public Transform Weapon;//����λ��

        public Gun CurrentGun;//��ǰ����


        public static Player Default;//����

        public List<AudioClip> GunTakeSfxs = new List<AudioClip>();//��ǹ��Ч

        private void Awake()
        {
            Application.targetFrameRate = 60;//����֡��60
            //ͬһ����ЧҪ����һ��֡�����ܲ��ţ�Ĭ��10֡
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

            var gunTakeSfx = GunTakeSfxs.GetRandomItem();//�����ȡһ����ǹ��Ч
            SelfAudioSource.clip = gunTakeSfx;
            SelfAudioSource.Play();

            Global.GunAdditionalCameraSize = CurrentGun.GunAdditionalCameraSize;
        }

        private void Start()
        {
            var gunIndex = GunSystem.GunList.FindIndex(g => g == Global.CurrentGun);
            UseGun(gunIndex);

            //��ʼ��״̬��
            State.State(States.Idle)
                .OnUpdate(() => 
                {

                    //��ȡ�����λ��
                    var mouseScreenPosition = Input.mousePosition;
                    //����Ļ����תΪ��������
                    var mouseWorldPoint = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                    //�õ����ǳ������ķ���
                    var shootDirection = (mouseWorldPoint - transform.position).normalized;


                    if (Global.CurrentRoom && Global.CurrentRoom.Enemies.Count > 0)
                    {
                        mTargetEmeny = Global.CurrentRoom.Enemies
                            .OrderBy(e => (e.GameObject.Position2D() - mouseWorldPoint.ToVector2()).magnitude)//���ݾ���Զ������
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

                    //��·ʱ����Ч��
                    if (horizontal != 0 || vertical != 0)
                    {
                        //����������΢����
                        AnimationHelper.UpDownAnimation(Sprite, 0.05f, 10, Time.frameCount, 0.35f);
                        //������΢����
                        AnimationHelper.UpDownAnimation(Weapon, 0.05f, 10, Time.frameCount);
                        //����������΢�ڶ�
                        AnimationHelper.RotateAnimation(Sprite, 3, 30, Time.frameCount);

                    }

                    //�����ƫ��
                    var offsetLength = (mouseWorldPoint - transform.position).magnitude;
                    Global.CameraPosOffset = (shootDirection * (3 + Mathf.Clamp(offsetLength * 0.15f, 0, 3))).ToVector2();

                    if (Global.CanShoot)
                    {
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
                                var index = GunSystem.GunList.FindIndex(gun => gun == CurrentGun.Data);//����GUN�б���õ�ǰ����������
                                index--;

                                if (index < 0)
                                {
                                    index = GunSystem.GunList.Count - 1;
                                }

                                UseGun(index);

                            }



                        }

                        if (Input.GetKeyDown(KeyCode.E))//�л���һ������
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
                    SelfCircleCollider2D.Disable();//ȡ����ײ

                    var x = Input.GetAxis("Horizontal");
                    var y = Input.GetAxis("Vertical");

                    if(x != 0 || y != 0)
                    {
                        faceDirection = new Vector2(x, y).normalized;//��ȡ���ﳯ��
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
                    SelfCircleCollider2D.Enable();//�ָ���ײ
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

                    //����Armor��Ч
                    AudioKit.PlaySound("resources://UseArmor");
                }
                else
                {
                    damage -= Global.Armor.Value;
                    Global.Armor.Value = 0;

                    //����Armor��Ч
                    AudioKit.PlaySound("resources://UseArmor");

                }
            }

            if (damage > 0)
            {
                //����������Ч
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
            //״̬������
            State.Update();

        }

        private void FixedUpdate()
        {
            //״̬������
            State.FixedUpdate();
        }
    }

}