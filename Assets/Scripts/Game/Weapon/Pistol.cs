using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace QFramework.ProjectGungeon
{
    public partial class Pistol : Gun
    {


        public override AudioSource AudioPlayer => SelfAudioSource;

        public override GunClip Clip { get; set; } = new();

        public ShootLight ShootLight = new ShootLight();

        public override BulletBag BulletBag { get; set; } = new BulletBag(100);

        public override bool Reloading => Clip.Reloading;

        public float UnstableRate => 0.1f;

        public override float GunAdditionalCameraSize => 0;


        public override void OnGunUsed()
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

                //�ӵ��Ƕ����ƫ�ƣ�
                var angle = direction.ToAngle() + Random.Range(0.05f, UnstableRate) * 30 * RandomUtility.Choose(-1, 1);

                BulletHelper.Shoot(BulletPos.Position2D(), angle.AngleToDirection2D(), 15, Random.Range(1f, 1.5f));


                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();

                Clip.UseBullet();//�����ӵ�����

                ShootLight.ShowLight(BulletPos.Position2D(), direction);

                //�������
                CameraController.Shake.Trigger(0.05f, 2);

                BackForce.Shoot(0.05f, 2);

                BulletFactory.GenBulletShell(direction);

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
                Shoot(direction);//�������
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
