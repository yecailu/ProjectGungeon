using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
    public partial class AWP : Gun
    {
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        

        void Shoot(Vector2 position, Vector2 direction, bool playSound = true)
        {
            Bullet.ShootSpeed = 40f;
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = position;
            playerBullet.Direction = direction.normalized;
            playerBullet.gameObject.SetActive(true);

            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();
        }

        public float ShootDuration => 2;//每间隔2秒射击一次
        private float mLastShootTime = 0;

        

        public override void ShootDown(Vector2 direction)
        {
            if (mLastShootTime == 0 || Time.time - mLastShootTime >= ShootDuration)
            {
                mLastShootTime = Time.time;
              
                Shoot(BulletPrefab.Position2D(), direction);

            }
        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);
        }
    }
}
