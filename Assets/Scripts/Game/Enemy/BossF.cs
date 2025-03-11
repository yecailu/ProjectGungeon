using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class BossF : Enemy, IEnemy
    { 
        public Player player;

        public EnemyBullet EnemyBullet;

        public SpriteRenderer Sprite;

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public Rigidbody2D Rigidbody2D;

        public Animator animator;

        public CircleCollider2D SelfCircleCollider2D;

        public float HP { get; set; } = 200;
        public float mMaxHP { get; set; }

        protected override Rigidbody2D GetRigidbody2D => Rigidbody2D;

        public override bool IsBoss => true;

        public GameObject laserAttack;

        public GameObject flyingFistAttack;

        private bool isFacingRight = true;

        public Transform fistPoint;


        public override void Hurt(float damage, Vector2 hitDirection)
        {
            FxFactory.PlayHurtFx(transform.Position2D());
            FxFactory.PlayEnemyBlood(transform.Position2D());
            GameUI.Default.BossHPBar.fillAmount = HP / mMaxHP;
            

            HP -= damage;
            if (HP <= 0)
            {
                //OnDeath(hitDirection, null, 1.5f);

                SelfCircleCollider2D.Disable();//È¡ÏûÅö×²

                animator.SetTrigger("isDie");



                //µÐÈËËÀÍöÒôÐ§
                AudioKit.PlaySound("resources://EnemyDie2");

                PowerUpFactory.GeneratePowerUp(this);

                Invoke("Destroy", 2f);

                
            }
        }
        private void Destroy()
        {
            Destroy(gameObject);
        }

        //µÐÈË×´Ì¬
        public enum States
        {
            FollowPlayer,
            PrepareToShoot,
            Shoot,
        }

        public FSM<States> State = new FSM<States>();

        public float FollowPlayerSeconds = 3.0f;//Ëæ»ú¸úËæÊ±¼ä


        private void Awake()
        {
            mMaxHP = HP;
            State.State(States.FollowPlayer)
                .OnEnter(() =>
                {
                    FollowPlayerSeconds = Random.Range(0.5f, 3f);//½øÈë¸úËæ×´Ì¬Ê±Ëæ»úÉèÖÃ¸úËæÊ±¼ä
                    MovementPath.Clear();

                    animator.SetBool("isWalk", true);

                })
                .OnUpdate(() =>
                {
                    TryPrepareMovementPath();

                    if (Global.Player)
                    {

                        //µÐÈËÒÆ¶¯Ê±ÇáÎ¢¶¶¶¯
                        //AnimationHelper.UpDownAnimation(Sprite, 0.07f, 100, State.FrameCountOfCurrentState);

                        //AnimationHelper.RotateAnimation(Sprite, 3, 30, State.FrameCountOfCurrentState);

                        var directionToPlayer = Move();
                        //µÐÈË³¯ÏòÖ÷½Ç
                        if (directionToPlayer.x > 0 && !isFacingRight)
                        {
                            Flip();
                        }
                        else if (directionToPlayer.x < 0 && isFacingRight)
                        {
                            Flip();
                        }
                    }


                    if (State.SecondsOfCurrentState >= FollowPlayerSeconds)
                    {                                               
                        State.ChangeState(States.PrepareToShoot);
                        animator.SetBool("isWalk", false);
                        animator.SetBool("isPreShoot", true);
                    }

                }).OnExit(() =>
                {
                    
                });

            void Flip()
            {
                isFacingRight = !isFacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }

            var originSpriteLocalPos = Sprite.LocalPosition2D();
            State.State(States.PrepareToShoot)
                .OnEnter(() =>
                {
                    originSpriteLocalPos = Sprite.LocalPosition2D();
                    Rigidbody2D.velocity = Vector2.zero; //µÐÈËÍ£Ö¹ÒÆ¶¯


                })
                .OnUpdate(() =>
                {
                    //¶¶¶¯0.25Ãë
                    //var shakeRate = (State.SecondsOfCurrentState / 0.25f).Lerp(0.05f, 0.1f);
                    //Sprite.LocalPosition2D(originSpriteLocalPos + new Vector2(Random.Range(-shakeRate, shakeRate)
                    //    , Random.Range(-shakeRate, shakeRate)));
                    if (State.SecondsOfCurrentState > 1.5f)
                    {
                        State.ChangeState(States.Shoot);
                        animator.SetBool("isPreShoot", false);
                    }


                })
                .OnExit(() =>
                {
                    Sprite.LocalPosition2D(originSpriteLocalPos);

                });

            State.State(States.Shoot)
                .OnEnter(() =>
                {
                    Rigidbody2D.velocity = Vector2.zero; //¿ªÇ¹Ê±£¬µÐÈËÍ£Ö¹ÒÆ¶¯


                })
                .OnUpdate(() =>
                {

                    if (HP / mMaxHP > 0.7f)
                    {
                        if (State.FrameCountOfCurrentState % 90 == 0)
                        {
                            FlyingFistAttack();
                        }
                        //¹¥»÷¼ä¸ô
                        if (State.FrameCountOfCurrentState <= 120)
                        {
                            //¹¥»÷ÆµÂÊ
                            if (State.FrameCountOfCurrentState % 40 == 0)
                            {
                                LaserAttack();
                            }
                        }
                        else
                        {
                            State.ChangeState(States.FollowPlayer);
                            animator.SetBool("isWalk", true);
                        }

                    }
                    else if(HP / mMaxHP > 0.4f)
                    {
                        if (State.FrameCountOfCurrentState % 70 == 0)
                        {
                            FlyingFistAttack();
                        }
                        //¹¥»÷¼ä¸ô
                        if (State.FrameCountOfCurrentState <= 120)
                        {
                            //¹¥»÷ÆµÂÊ
                            if (State.FrameCountOfCurrentState % 30 == 0)
                            {
                                LaserAttack();
                            }
                        }
                        else
                        {
                            State.ChangeState(States.FollowPlayer);
                            animator.SetBool("isWalk", true);
                        }
                    }
                    else
                    {
                        if (State.FrameCountOfCurrentState % 50 == 0)
                        {
                            FlyingFistAttack();
                        }
                        //¹¥»÷¼ä¸ô
                        if (State.FrameCountOfCurrentState <= 120)
                        {
                            //¹¥»÷ÆµÂÊ
                            if (State.FrameCountOfCurrentState % 20 == 0)
                            {
                                LaserAttack();
                            }
                        }
                        else
                        {
                            State.ChangeState(States.FollowPlayer);
                            animator.SetBool("isWalk", true);
                        }

                    }



                });

            State.StartState(States.FollowPlayer);
            animator.SetBool("isWalk", true);
        }

        private void LaserAttack()
        {
            laserAttack.Instantiate()
                .Position2D(Player.Default.transform.Position2D())
                .Show();
        }

        GameObject fist = null;
        private void FlyingFistAttack()
        {
            fist = flyingFistAttack.Instantiate()
                .Position2D(fistPoint.Position2D())
                .Show();

            Invoke("DestroyFist", 4f);
        }

        private void DestroyFist()
        {

            Boss_Fist.Default.PlayerSound();
       
           
            BulletFactory.Default.Explosion
                .Instantiate()
                .Position2D(fist.transform.Position2D())
                .Show();

            Destroy(fist.gameObject);

        }



        private void Start()
        {
            Application.targetFrameRate = 60;
            GameUI.Default.BossHpBarBG.Show();
            GameUI.Default.BossHPBar.fillAmount = 1.0f;
        }


        void Update()
        {
            State.Update();

        }

        private void OnDestroy()
        {
            Room.Enemies.Remove(this);
            GameUI.Default.BossHpBarBG.Hide();
            
        }


    }

}