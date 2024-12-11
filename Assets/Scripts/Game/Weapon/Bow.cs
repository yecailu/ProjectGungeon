using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
    public partial class Bow : QFramework.ProjectGungeon.Gun
    {
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public GunClip Clip = new GunClip(3);

        public override bool Reloading => Clip.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(100, 100);

        public override void OnGunUsed()
        {
            Clip.UpdateUI();
        }

        public override void Reload()
        {
            if (Reloading) return;
            BulletBag.Reload(Clip, ReloadSound);
        }

        void Shoot(Vector2 position, Vector2 direction, bool playSound = true)
        {
            if (Clip.CanShoot)
            {
                var playerBullet = Instantiate(BulletPrefab);
                playerBullet.transform.position = position;
                playerBullet.Velocity = direction.normalized * 30;
                playerBullet.gameObject.SetActive(true);

                playerBullet.Damage = Random.Range(5, 10);//����˺��ж�


                playerBullet.transform.right = direction;//�޸���������

                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();

                Clip.UseBullet();

            }
        }



        private bool mPressing = false;
        public override void ShootDown(Vector2 direction)
        {
            if (Clip.CanShoot)
            {
                mCurrentSeconds = 0;
                mPressing = true;
            }
            else
            {
                Reload();
            }

        }

        private float mCurrentSeconds = 0;//��ʱ��
        public override void Shooting(Vector2 direction)
        {
            if (!Clip.CanShoot) return;
            if (mPressing == true)
            {
                mCurrentSeconds += Time.deltaTime;

                if (mCurrentSeconds >= 0.5f)
                {
                    CanShootSprite.Show();
                }
                else
                {
                    CanShootSprite.Hide();
                }
            }
            else
            {
                CanShootSprite.Hide();
            }


        }

        public override void ShootUp(Vector2 direction)
        {
            if (!Clip.CanShoot) return;
            if (mPressing == true && mCurrentSeconds >= 0.5f)
            {
                Shoot(BulletPrefab.Position2D(), direction);
            }
            CanShootSprite.Hide();
            mPressing = false;
        }
    }
}
