using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    

    public class EnemyA : Enemy, IEnemy
    {
        public Player player;

        public EnemyBullet EnemyBullet;

        public SpriteRenderer Sprite;

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public Rigidbody2D Rigidbody2D;

        public float HP { get; set; } = 5;

        protected override Rigidbody2D GetRigidbody2D => Rigidbody2D;

        public override void Hurt(float damage, Vector2 hitDirection)
        {
            FxFactory.PlayHurtFx(transform.Position2D());
            FxFactory.PlayEnemyBlood(transform.Position2D());

            HP -= damage;
            if (HP <= 0)
            {
                OnDeath(hitDirection, "EnemyADie", 1.3f);
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
                        if (directionToPlayer.x < 0)
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
                    if (State.SecondsOfCurrentState <= Time.deltaTime * 1.5f)
                    {

                        if (Global.Player)
                        {
                            //敌人到玩家的方向
                            var directionToPlayer = (Global.Player.transform.position - transform.position).normalized;



                            //敌人子弹逻辑
                            var enemyBullet = Instantiate(EnemyBullet);
                            enemyBullet.transform.position = transform.position;
                            enemyBullet.Velocity = directionToPlayer.normalized * 5;
                            enemyBullet.gameObject.SetActive(true);

                            //播放射击音效
                            var soundIndex = Random.Range(0, ShootSounds.Count);
                            AudioKit.PlaySound(ShootSounds[soundIndex]);

                        }


                    }
                })
                .OnUpdate(() =>
                {



                    if (State.SecondsOfCurrentState >= 1.0f)//时间大于1后开始射击，并且赋予敌人随机跟随时间
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