using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class EnemyB : MonoBehaviour, IEnemy
    {
        public Player player;

        public EnemyBullet EnemyBullet;

        public SpriteRenderer Sprite;

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public Rigidbody2D Rigidbody2D;

        public float HP { get; set; } = 5;


        public void hurt(float damage)
        {
            HP -= damage;
            if (HP <= 0)
            {
                Destroy(gameObject);
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

                            var count = 3;
                            var durationAngle = 15;

                            var mainAngle = directionToPlayer.ToVector2().ToAngle();//将敌人朝向设置成Vector2再变成欧拉角
                            for (int i = 0; i < count; i++)
                            {
                                var angle = mainAngle + i * durationAngle - count * durationAngle * 0.5f;
                                var direction = angle.AngleToDirection2D();
                                var pos = transform.Position2D() + 0.5f * direction;//子弹出现位置


                                //敌人子弹逻辑
                                var enemyBullet = Instantiate(EnemyBullet);
                                enemyBullet.transform.position = pos;
                                enemyBullet.Velocity = direction * 5;
                                enemyBullet.gameObject.SetActive(true);

                            }

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




        void Update() => State.Update();
        public GameObject GameObject => gameObject;


    }
}