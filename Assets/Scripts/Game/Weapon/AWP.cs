using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
    public partial class AWP : Gun
    {
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public GunClip Clip = new GunClip(10);

        public ShootLight ShootLight = new ShootLight();

        public override bool Reloading => Clip.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(50, 50);


        public override void OnGunUsed()
        {
            Clip.UpdateUI();
        }

        public override void Reload()
        {
            BulletBag.Reload(Clip, ReloadSound);
        }


        void Shoot(Vector2 position, Vector2 direction, bool playSound = true)
        {
            Bullet.ShootSpeed = 40f;
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = position;
            playerBullet.Direction = direction.normalized;
            playerBullet.gameObject.SetActive(true);

            playerBullet.Damage = Random.Range(5, 10);//Ëæ»úÉËº¦ÅÐ¶¨


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
        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);
        }
    }
}
