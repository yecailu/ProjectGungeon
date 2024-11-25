using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class AK : QFramework.ProjectGungeon.Gun
	{
        public PlayerBullet Bullet;

        public UnityEngine.AudioSource ShootSoundPlayer;

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => ShootSoundPlayer;

        void Shoot(Vector2 direction)
        {
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = BulletPrefab.transform.position;
            playerBullet.Direction = direction;
            playerBullet.gameObject.SetActive(true);

        }


        public override void ShootDown(Vector2 direction)
        {
            Shoot(direction);

            AudioPlayer.clip = ShootSounds[0];
            AudioPlayer.Play();
        }

        private float mCurrentSeconds = 0f;//计时器

      

        public override void Shooting(Vector2 direction)
        {
            mCurrentSeconds += Time.deltaTime;

            if (mCurrentSeconds >= 0.15f)//每隔0.15秒发射一次子弹
            {
                mCurrentSeconds = 0f;
                Shoot(direction);
            }
        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();

            AudioPlayer.clip = AKShootEnd;
            AudioPlayer.loop = false;
            AudioPlayer.Play();
        }
    }
}
