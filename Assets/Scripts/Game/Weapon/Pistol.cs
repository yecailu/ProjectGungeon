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



        private void Start()
        {
            Clip.UpdateUI();//刷新武器UI信息
        }


        public override void Reload()
        {
            Clip.Reload();
        }


        public ShootDuration ShootDuration = new ShootDuration(0.25f);

        public override void ShootDown(Vector2 direction)
        {
            if (ShootDuration.CanShoot && Clip.CanShoot)
            {
                ShootDuration.RecordShootTime();
                var playerBullet = Instantiate(BulletPrefab);
                playerBullet.transform.position = BulletPrefab.transform.position;
                playerBullet.Direction = direction;
                playerBullet.gameObject.SetActive(true);

                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();

                Clip.UseBullet();//减少子弹数量
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
