using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace QFramework.ProjectGungeon
{
    public partial class Pistol : Gun
    {


        public override AudioSource AudioPlayer => SelfAudioSource;

        public override GunClip Clip { get; set; } = new GunClip(10);

        public ShootLight ShootLight = new ShootLight();

        public override BulletBag BulletBag { get; set; } = new BulletBag(100,100);



        public override bool Reloading => Clip.Reloading;

        public void Start()
        {
            Clip.UpdateUI();
        }



        public override void Reload()
        {
            if (Reloading) return;
            BulletBag.Reload(Clip, ReloadSound);
        }

        void Shoot(Vector2 direction)
        {
            if (ShootDuration.CanShoot)
            {
                ShootDuration.RecordShootTime();

                BulletHelper.Shoot(BulletPos.Position2D(), direction, 15, Random.Range(1f, 1.5f));


                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();

                Clip.UseBullet();//减少子弹数量

                ShootLight.ShowLight(BulletPos.Position2D(), direction);

                //摄像机震动
                CameraController.Shake.Trigger(0.05f, 2);
            }
        }

        public ShootDuration ShootDuration = new ShootDuration(0.25f);

        public override void ShootDown(Vector2 direction)
        {
            if (Clip.CanShoot)
            {
               Shoot(direction);
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
                Shoot(direction);//长按射击
            }
            else
            {
                TryPlayEmptyShootSound();
            }
        }
        public override void ShootUp(Vector2 direction)
        {

        }
    }

}
