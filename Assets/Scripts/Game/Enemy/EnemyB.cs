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

        //����״̬
        public enum States
        {
            FollowPlayer,
            Shoot,
        }

        public FSM<States> State = new FSM<States>();

        public float FollowPlayerSeconds = 3.0f;//�������ʱ��


        private void Awake()
        {
            State.State(States.FollowPlayer)
                .OnEnter(() =>
                {
                    FollowPlayerSeconds = Random.Range(0.5f, 3f);//�������״̬ʱ������ø���ʱ��

                })
                .OnUpdate(() =>
                {

                    if (Global.Player)
                    {
                        var directionToPlayer = (Global.Player.transform.position - transform.position).normalized;//���˵���ҵķ���

                        Rigidbody2D.velocity = directionToPlayer; //�����ٶ�

                        //���˳�������
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
                    Rigidbody2D.velocity = Vector2.zero; //��ǹʱ������ֹͣ�ƶ�
                    if (State.SecondsOfCurrentState <= Time.deltaTime * 1.5f)
                    {

                        if (Global.Player)
                        {
                            //���˵���ҵķ���
                            var directionToPlayer = (Global.Player.transform.position - transform.position).normalized;

                            var count = 3;
                            var durationAngle = 15;

                            var mainAngle = directionToPlayer.ToVector2().ToAngle();//�����˳������ó�Vector2�ٱ��ŷ����
                            for (int i = 0; i < count; i++)
                            {
                                var angle = mainAngle + i * durationAngle - count * durationAngle * 0.5f;
                                var direction = angle.AngleToDirection2D();
                                var pos = transform.Position2D() + 0.5f * direction;//�ӵ�����λ��


                                //�����ӵ��߼�
                                var enemyBullet = Instantiate(EnemyBullet);
                                enemyBullet.transform.position = pos;
                                enemyBullet.Velocity = direction * 5;
                                enemyBullet.gameObject.SetActive(true);

                            }

                            //���������Ч
                            var soundIndex = Random.Range(0, ShootSounds.Count);
                            AudioKit.PlaySound(ShootSounds[soundIndex]);

                        }


                    }
                })
                .OnUpdate(() =>
                {



                    if (State.SecondsOfCurrentState >= 1.0f)//ʱ�����1��ʼ��������Ҹ�������������ʱ��
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