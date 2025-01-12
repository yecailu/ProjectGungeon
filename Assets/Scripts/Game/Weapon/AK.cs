using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class AK : QFramework.ProjectGungeon.Gun
	{
        public UnityEngine.AudioSource ShootSoundPlayer;

        public override AudioSource AudioPlayer => ShootSoundPlayer;

        public ShootDuration ShootDuration = new ShootDuration(0.1f);

        public override GunClip Clip { get; set; } = new GunClip(30);

        public ShootLight ShootLight = new ShootLight();

        public override bool Reloading => Clip.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(100, 100);
        
        public float UnstableRate => 0.2f;

        public override void OnGunUsed()
        {
            Clip.UpdateUI();
        }

        public override void Reload()
        {
            if(Reloading) return;
            BulletBag.Reload(Clip, ReloadSound);
        }

        void Shoot(Vector2 direction)
        {
            var angle = direction.ToAngle() + Random.Range(0.05f, UnstableRate) * 30 * RandomUtility.Choose(-1, 1);


            BulletHelper.Shoot(BulletPos.Position2D(), angle.AngleToDirection2D(), 30, Random.Range(1.5f, 2.0f));

            ShootLight.ShowLight(BulletPos.Position2D(), direction);

            //ÉãÏñ»úÕð¶¯
            CameraController.Shake.Trigger(0.07f, 5);
        }


        public override void ShootDown(Vector2 direction)
        {
            if (Clip.CanShoot)
            {
                ShootDuration.RecordShootTime();
                Shoot(direction);
 

                AudioPlayer.clip = ShootSounds[0];
                AudioPlayer.loop = true;
                AudioPlayer.Play();

                Clip.UseBullet();

                TryPlayShootSound(true);
            }
            else
            {
                Reload();
            }
        }




        public override void Shooting(Vector2 direction)
        {
            if (ShootDuration.CanShoot && Clip.CanShoot)
            {
                ShootDuration.RecordShootTime();
                Shoot(direction);

                Clip.UseBullet();

                TryPlayShootSound(true);


            }
            else if (!Clip.CanShoot)
            {
                AudioPlayer.Stop();
                TryPlayEmptyShootSound();
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
