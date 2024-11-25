using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
    public partial class RocketGun : QFramework.ProjectGungeon.Gun
    {
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;


        


        void Shoot(Vector2 position, Vector2 direction, bool playSound = true)
        {
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = position;
            playerBullet.Direction = direction.normalized;
            playerBullet.gameObject.SetActive(true);

            playerBullet.transform.right = direction;//�޸����������һֱ���ҵ�BUG

            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();
        }

        public float ShootDuration => 2;//ÿ���2�����һ��
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
