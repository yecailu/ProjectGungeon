using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
    public partial class AWP : Gun
    {
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override GunClip Clip { get; set; } = new GunClip(5);

        public ShootLight ShootLight = new ShootLight();

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
            playerBullet.Velocity = direction.normalized * 50;
            playerBullet.gameObject.SetActive(true);

            playerBullet.Damage = Random.Range(5, 10);//����˺��ж�


            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();

            ShootLight.ShowLight(BulletPrefab.Position2D(), direction);
        }


        public ShootDuration ShootDuration = new ShootDuration(2);

        public override void ShootDown(Vector2 direction)
        {
            if (ShootDuration.CanShoot && Clip.CanShoot)
            {
                ShootDuration.RecordShootTime();

                Shoot(BulletPrefab.Position2D(), direction);

                Clip.UseBullet();
            }
            else if (!Clip.CanShoot)
            {
                Reload();
            }
        }

        public override void Shooting(Vector2 direction)
        {
            if(Clip.CanShoot)
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
