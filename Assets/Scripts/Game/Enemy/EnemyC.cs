using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace QFramework.ProjectGungeon
{
    public class EnemyC : Enemy, IEnemy
    {
        public Player player;

        public EnemyBullet EnemyBullet;

        public SpriteRenderer Sprite;

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public Rigidbody2D Rigidbody2D;

        public float HP { get; set; } = 5;


        public void Hurt(float damage, Vector2 hitDirection)
        {
            FxFactory.PlayHurtFx(transform.Position2D());
            FxFactory.PlayEnemyBlood(transform.Position2D());

            HP -= damage;
            if (HP <= 0)
            {
                OnDeath(hitDirection, "EnemyCDie", 1.5f);

            }
        }

        //敌人状态
        public enum States
        {
            FollowPlayer,
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
                        State.ChangeState(States.Shoot);
                    }

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

                            ActionKit.Sequence()
                            .Callback(() =>
                            {
                                //敌人子弹逻辑
                                var enemyBullet = Instantiate(EnemyBullet);
                                enemyBullet.transform.position = transform.position;
                                enemyBullet.Velocity = directionToPlayer * 5;
                                enemyBullet.gameObject.SetActive(true);

                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
                            })
                            .Delay(0.2f)
                            .Callback(() =>
                            {
                                //敌人子弹逻辑
                                var enemyBullet = Instantiate(EnemyBullet);
                                enemyBullet.transform.position = transform.position;
                                enemyBullet.Velocity = directionToPlayer * 5;
                                enemyBullet.gameObject.SetActive(true);

                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
                            })
                            .Delay(0.2f)
                            .Callback(() =>
                            {
                                //敌人子弹逻辑
                                var enemyBullet = Instantiate(EnemyBullet);
                                enemyBullet.transform.position = transform.position;
                                enemyBullet.Velocity = directionToPlayer * 5;
                                enemyBullet.gameObject.SetActive(true);

                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
                            })
                            .Start(this);



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
        public Room Room { get; set; }
        public GameObject GameObject => gameObject;

        private void OnDestroy()
        {
            Room.Enemies.Remove(this);
        }
    }
}