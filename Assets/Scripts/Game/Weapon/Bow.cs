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

        public override void OnGunUsed()
        {
            Clip.UpdateUI();
        }

        public override void Reload()
        {
            Clip.Reload(ReloadSound);
        }

        void Shoot(Vector2 position, Vector2 direction, bool playSound = true)
        {
            if (Clip.CanShoot)
            {
                Bullet.ShootSpeed = 40f;
                var playerBullet = Instantiate(BulletPrefab);
                playerBullet.transform.position = position;
                playerBullet.Direction = direction.normalized;
                playerBullet.gameObject.SetActive(true);

                playerBullet.transform.right = direction;//ÐÞ¸´¹­¼ý³¯Ïò

                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();

                Clip.UseBullet();

            }
        }



        private bool mPressing = false;
        public override void ShootDown(Vector2 direction)
        {
            if (!Clip.CanShoot) return;

            mPressing = true;
            mCurrentSeconds = 0;

        }

        private float mCurrentSeconds = 0;//¼ÆÊ±Æ÷
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
