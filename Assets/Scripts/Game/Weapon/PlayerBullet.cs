using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon {
    public class PlayerBullet : Bullet
    {

        private Rigidbody2D mSelfRigidbody2D;




        private void Awake()
        {
            mSelfRigidbody2D = GetComponent<Rigidbody2D>();
        }

        void Start()
        {

        }



        void FixedUpdate()
        {
            mSelfRigidbody2D.velocity = Velocity;
        }

        public List<AudioClip> HitWallSfxs = new List<AudioClip>();
        public List<AudioClip> HitEnemySfx = new List<AudioClip>();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                this.Hide();
                var enemy = collision.gameObject.GetComponent<IEnemy>();
                enemy.Hurt(Damage, -collision.GetContact(0).relativeVelocity.normalized);;//敌人受伤方法
                if (HitEnemySfx.Count > 0)
                {
                    var hitEnemySfx = HitEnemySfx.GetRandomItem();
                    var audioPlayer = AudioKit.PlaySound(hitEnemySfx, callBack: (_) =>
                    {
                        this.DestroyGameObjGracefully();
                    });
                    audioPlayer?.SetVolume(0.1f);
                }
                else
                {
                    this.DestroyGameObjGracefully();
                }
            }
            else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door"))
            {
                this.Hide();
                if (HitWallSfxs.Count > 0)
                {
                    var hitWallSfx = HitWallSfxs.GetRandomItem();
                    var audioPlayer = AudioKit.PlaySound(hitWallSfx, callBack: (_) =>
                    {
                        this.DestroyGameObjGracefully();
                    });
                    audioPlayer?.SetVolume(0.1f);
                }
                else
                { 
                    this.DestroyGameObjGracefully();
                }

            }

        }
    }
}