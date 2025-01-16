using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class EnemyG : Enemy, IEnemy
    {
        public Player player;

        public EnemyBullet EnemyBullet;

        public SpriteRenderer Sprite;

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public Rigidbody2D Rigidbody2D;

        public float HP { get; set; } = 5;


        public override void Hurt(float damage, Vector2 hitDirection)
        {
            FxFactory.PlayHurtFx(transform.Position2D());
            FxFactory.PlayEnemyBlood(transform.Position2D());

            HP -= damage;
            if (HP <= 0)
            {
                OnDeath(hitDirection, "EnemyGDie", 1.5f);

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
            State.State(States.FollowPlayer)
                .OnEnter(() =>
                {
                    FollowPlayerSeconds = Random.Range(0.5f, 3f);//进入跟随状态时随机设置跟随时间

                })
                .OnUpdate(() =>
                {

                    if (Global.Player)
                    {
                        var directionToPlayer = (Global.Player.transform.position - transform.position).normalized;//敌人到玩家的方向

                        //敌人移动时轻微抖动
                        AnimationHelper.UpDownAnimation(Sprite, 0.05f, 10, State.FrameCountOfCurrentState);
                        AnimationHelper.RotateAnimation(Sprite, 3, 30, State.FrameCountOfCurrentState);

                        Rigidbody2D.velocity = directionToPlayer; //敌人速度

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

            var shootSeconds = Random.Range(1.0f, 6.0f);
            State.State(States.Shoot)
                .OnEnter(() =>
                {

                    Rigidbody2D.velocity = Vector2.zero; //开枪时，敌人停止移动

                    shootSeconds = Random.Range(1.0f, 6.0f);

                })
                .OnUpdate(() =>
                {
                    if(State.FrameCountOfCurrentState % 12 == 0)
                    {
                        if (Global.Player)
                        {
                            var directionToPlayer = this.NormalizedDirection2DTo(Global.Player);



                            //敌人子弹逻辑
                            BulletHelper.ShootSpread(3, 15, transform.position, directionToPlayer, 0.5f, EnemyBullet);


                            //播放射击音效
                            var soundIndex = Random.Range(0, ShootSounds.Count);
                            AudioKit.PlaySound(ShootSounds[soundIndex]);

                        }
                    }


                    if (State.SecondsOfCurrentState >= shootSeconds)//时间大于1后开始射击，并且赋予敌人随机跟随时间
                    {
                        State.ChangeState(States.FollowPlayer);
                    }


                });

            State.StartState(States.FollowPlayer);
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }


        void Update() => State.Update();

        private void OnDestroy()
        {
            Room.Enemies.Remove(this);
        }

    }

}