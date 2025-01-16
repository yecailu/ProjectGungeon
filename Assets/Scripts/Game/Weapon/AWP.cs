using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
    public partial class AWP : Gun
    {
        public override AudioSource AudioPlayer => SelfAudioSource;

        public override GunClip Clip { get; set; } = new();

        public ShootLight ShootLight = new ShootLight();

        public override bool Reloading => Clip.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(50);

        public override float GunAdditionalCameraSize => 3;

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
            BulletHelper.Shoot(BulletPos.Position2D(), direction, 50, Random.Range(5, 10));

            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();

            ShootLight.ShowLight(BulletPos.Position2D(), direction);
            //ÉãÏñ»úÕð¶¯
            CameraController.Shake.Trigger(0.12f, 7);

            BackForce.Shoot(0.1f, 5);

        }


        public ShootDuration ShootDuration = new ShootDuration(2);

        public override void ShootDown(Vector2 direction)
        {
            if (ShootDuration.CanShoot && Clip.CanShoot)
            {
                ShootDuration.RecordShootTime();

                Shoot(BulletPos.Position2D(), direction);

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
