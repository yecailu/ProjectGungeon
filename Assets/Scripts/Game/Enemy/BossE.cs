using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class BossE : Enemy, IEnemy
    { 
        public Player player;

        public EnemyBullet EnemyBullet;

        public SpriteRenderer Sprite;

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public Rigidbody2D Rigidbody2D;

        public float HP { get; set; } = 150;
        public float mMaxHP { get; set; }

        protected override Rigidbody2D GetRigidbody2D => Rigidbody2D;

        public override bool IsBoss => true;

        public override void Hurt(float damage, Vector2 hitDirection)
        {
            FxFactory.PlayHurtFx(transform.Position2D());
            FxFactory.PlayEnemyBlood(transform.Position2D());
            GameUI.Default.BossHPBar.fillAmount = HP / mMaxHP;
            

            HP -= damage;
            if (HP <= 0)
            {
                OnDeath(hitDirection, null, 1.5f);

            }
        }

        //敌人状态
        public enum States
        {
            FollowPlayer,
            PrepareToShoot,
            Shoot,
        }

        public FSM<States> State = new FSM<States>();

        public float FollowPlayerSeconds = 3.0f;//随机跟随时间


        private void Awake()
        {
            mMaxHP = HP;
            State.State(States.FollowPlayer)
                .OnEnter(() =>
                {
                    FollowPlayerSeconds = Random.Range(0.5f, 3f);//进入跟随状态时随机设置跟随时间
                    MovementPath.Clear();

                })
                .OnUpdate(() =>
                {
                    TryInitMovementPath();

                    if (Global.Player)
                    {
                        var directionToPlayer = Move();
                        //敌人移动时轻微抖动
                        AnimationHelper.UpDownAnimation(Sprite, 0.05f, 10, State.FrameCountOfCurrentState);
                        AnimationHelper.RotateAnimation(Sprite, 3, 30, State.FrameCountOfCurrentState);


                        //敌人朝向主角
                        if (directionToPlayer.x > 0)
                        {
                            Sprite.flipX = false;
                        }
                        else
                        {
                            Sprite.flipX = true;
                        }
                    }


                    if (State.SecondsOfCurrentState >= FollowPlayerSeconds)
                    {
                        State.ChangeState(States.PrepareToShoot);
                    }

                });

            var originSpriteLocalPos = Sprite.LocalPosition2D();
            State.State(States.PrepareToShoot)
                .OnEnter(() =>
                {
                    originSpriteLocalPos = Sprite.LocalPosition2D();
                })
                .OnUpdate(() =>
                {
                    //抖动0.25秒
                    var shakeRate = (State.SecondsOfCurrentState / 0.25f).Lerp(0.05f, 0.1f);
                    Sprite.LocalPosition2D(originSpriteLocalPos + new Vector2(Random.Range(-shakeRate, shakeRate)
                        , Random.Range(-shakeRate, shakeRate)));
                    if (State.SecondsOfCurrentState > 0.25f)
                    {
                        State.ChangeState(States.Shoot);
                    }


                })
                .OnExit(() =>
                {
                    Sprite.LocalPosition2D(originSpriteLocalPos);
                });

            State.State(States.Shoot)
                .OnEnter(() =>
                {

                    Rigidbody2D.velocity = Vector2.zero; //开枪时，敌人停止移动


                })
                .OnUpdate(() =>
                {
                    //阶段一
                    if (HP / mMaxHP > 0.7f)
                    {
                        if(State.FrameCountOfCurrentState <= 120)
                        {
                            if(State.FrameCountOfCurrentState % 10 == 0)
                            {
                                BulletHelper.ShootAround(3, transform.Position2D(), 1.5f, EnemyBullet, 10,
                                    angleOffset: State.FrameCountOfCurrentState);
                                //播放射击音效
                                if (State.FrameCountOfCurrentState % 20 == 0)
                                {
                                    var soundIndex = Random.Range(0, ShootSounds.Count);
                                    AudioKit.PlaySound(ShootSounds[soundIndex]);
                                }
                            }
                        }
                        else
                        {
                            State.ChangeState(States.FollowPlayer);

                        }

                    }
                    //阶段二
                    else if (HP / mMaxHP > 0.3f)
                    {
                        if (State.FrameCountOfCurrentState % 6 == 0)
                        {
                            BulletHelper.ShootAround(3, transform.Position2D(), 1.5f, EnemyBullet, 10,
                                angleOffset: State.FrameCountOfCurrentState);
                            //播放射击音效
                            if(State.FrameCountOfCurrentState % 12 == 0)
                            {
                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
                            }

                        }

                        if (State.SecondsOfCurrentState > 5f)
                        {
                            State.ChangeState(States.FollowPlayer);
                        }

                    }
                    else
                    {
                        if (State.FrameCountOfCurrentState % 6 == 0)
                        {
                            BulletHelper.ShootAround(3, transform.Position2D(), 1.5f, EnemyBullet, 10,
                                angleOffset: State.FrameCountOfCurrentState);
                            //播放射击音效
                            if (State.FrameCountOfCurrentState % 12 == 0)
                            {
                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
                            }

                        }

                    }


                });

            State.StartState(States.FollowPlayer);
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            GameUI.Default.BossHpBarBG.Show();
            GameUI.Default.BossHPBar.fillAmount = 1.0f;
        }


        void Update() => State.Update();

        private void OnDestroy()
        {
            Room.Enemies.Remove(this);
            GameUI.Default.BossHpBarBG.Hide();
            
        }


    }

}