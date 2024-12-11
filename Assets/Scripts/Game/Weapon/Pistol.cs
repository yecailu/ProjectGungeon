using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace QFramework.ProjectGungeon
{
    public partial class Pistol : Gun
    {

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public GunClip Clip = new GunClip(10);

        public ShootLight ShootLight = new ShootLight();

        public override BulletBag BulletBag { get; set; } = new BulletBag(100,100);



        public override bool Reloading => Clip.Reloading;

        public void Start()
        {
            Clip.UpdateUI();
        }


        public override void Reload()
        {
            BulletBag.Reload(Clip, ReloadSound);
        }


        public ShootDuration ShootDuration = new ShootDuration(0.25f);

        public override void ShootDown(Vector2 direction)
        {
            if (ShootDuration.CanShoot && Clip.CanShoot)
            {
                ShootDuration.RecordShootTime();
                var playerBullet = Instantiate(BulletPrefab);
                playerBullet.transform.position = BulletPrefab.transform.position;
                playerBullet.Velocity = direction.normalized * 15;
                playerBullet.gameObject.SetActive(true);

                playerBullet.Damage = Random.Range(1f, 1.5f);//随机伤害判定


                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();

                Clip.UseBullet();//减少子弹数量

                ShootLight.ShowLight(BulletPrefab.Position2D(), direction);
            }
        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);//长按射击
        }
        public override void ShootUp(Vector2 direction)
        {

        }
    }

}
