using QFramework;
using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class RocketBullet : Bullet
    {

        private Rigidbody2D mSelfRigidbody2D;

        private void Awake()
        {
            mSelfRigidbody2D = GetComponent<Rigidbody2D>();
        }



        void FixedUpdate()
        {
            mSelfRigidbody2D.velocity = Velocity;
        }

        private void Update()
        {
            var enemy = AimHelper.GetClosestVisibleEnemy(transform, transform.Position2D());

            if(enemy != null)
            {
                var targetDirection = transform.NormalizedDirection2DTo(enemy.GameObject);
                var currentAngle = Velocity.ToAngle();
                var angle = Vector2.SignedAngle(Velocity, targetDirection);
                var sign = angle.Sign();
                currentAngle += sign * Time.deltaTime * 90;
                Velocity = currentAngle.AngleToDirection2D() * Velocity.magnitude;
                transform.right = Velocity.normalized;

            } 
        }

        public List<AudioClip> HitWallSfxs = new List<AudioClip>();
        public List<AudioClip> HitEnemySfx = new List<AudioClip>();

        private void OnCollisionEnter2D(Collision2D collision)
        {
            BulletFactory.Default.Explosion
                .Instantiate()
                .Position2D(transform.Position2D())
                .Show();


            if (collision.gameObject.CompareTag("Enemy"))
            {
                this.Hide();
                var enemy = collision.gameObject.GetComponent<IEnemy>();
                enemy.Hurt(Damage, -collision.GetContact(0).relativeVelocity.normalized); ;//敌人受伤方法
                if (HitEnemySfx.Count > 0)
                {
                    var hitEnemySfx = HitEnemySfx.GetRandomItem();
                    var audioPlayer = AudioKit.PlaySound(hitEnemySfx, callBack: (_) =>
                    {
                        this.DestroyGameObjGracefully();
                    });
                    audioPlayer?.SetVolume(0.5f);
                }
                else
                {
                    this.DestroyGameObjGracefully();
                }
            }
            else if (collision.gameObject.CompareTag("Wall"))
            {
                this.Hide();
                if (HitWallSfxs.Count > 0)
                {
                    var hitWallSfx = HitWallSfxs.GetRandomItem();
                    var audioPlayer = AudioKit.PlaySound(hitWallSfx, callBack: (_) =>
                    {
                        this.DestroyGameObjGracefully();
                    });
                    audioPlayer?.SetVolume(0.5f);
                }
                else
                {
                    this.DestroyGameObjGracefully();
                }

            }

        }
    }
}