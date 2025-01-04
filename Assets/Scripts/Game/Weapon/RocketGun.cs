using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
    public partial class RocketGun : QFramework.ProjectGungeon.Gun
    {
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override GunClip Clip { get; set; } = new GunClip(1);

        public override bool Reloading => Clip.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(50, 50);


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
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = position;
            playerBullet.Velocity = direction.normalized * 10;
            playerBullet.gameObject.SetActive(true);

            playerBullet.Damage = Random.Range(3f, 8f);//����˺��ж�


            playerBullet.transform.right = direction;//�޸����������һֱ���ҵ�BUG

            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();

            Clip.UseBullet();

            //�������
            CameraController.Shake.Trigger(0.14f, 8);
        }


        public ShootDuration ShootDuration = new ShootDuration(2);
        public override void ShootDown(Vector2 direction)
        {
            if (Clip.CanShoot)
            {
                if (ShootDuration.CanShoot)
                {
                    ShootDuration.RecordShootTime();
                    Shoot(BulletPrefab.Position2D(), direction);
                }
            }
            else
            {
                Reload();
            }

        }

        public override void Shooting(Vector2 direction)
        {
            if (Clip.CanShoot)
            {
                ShootDown(direction);
            }
            else
            {
                TryPlayEmptyShootSound();
            }
        }
    }
}
