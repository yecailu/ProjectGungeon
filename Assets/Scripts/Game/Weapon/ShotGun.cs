using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace QFramework.ProjectGungeon
{
    public partial class ShotGun : QFramework.ProjectGungeon.Gun
    {

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override GunClip Clip { get; set; } = new GunClip(5);

        public ShootLight ShootLight = new ShootLight();

        public override bool Reloading => Clip.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(100, 100);

        public float UnstableRate => 0.3f;

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
            BulletHelper.Shoot(position, direction, 20, Random.Range(1f, 2f));


            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();

        }


        public ShootDuration ShootDuration = new ShootDuration(1);


        public override void ShootDown(Vector2 direction)
        {
            if (Clip.CanShoot)
            {
                if (ShootDuration.CanShoot)
                { 
                    ShootDuration.RecordShootTime();

                    var angle = direction.ToAngle() + Random.Range(0.05f, UnstableRate) * 30 * RandomUtility.Choose(-1, 1);


                    var originPos = transform.parent.Position2D();//得到ShotGun父类Weapon的2D坐标
                    var radius = (BulletPos.Position2D() - originPos).magnitude;//得到子弹的半径

                    var angle1 = angle + 2;//根据中间的角度计算第二颗子弹的欧拉角
                    var direction1 = angle1.AngleToDirection2D();//得到第二颗子弹的方向
                    var pos1 = originPos + direction1 * radius;//得到第二颗子弹应该生成的的位置

                    var angle2 = angle - 2;
                    var direction2 = angle2.AngleToDirection2D();
                    var pos2 = originPos + direction2 * radius;

                    var angle3 = angle + 4;
                    var direction3 = angle3.AngleToDirection2D();
                    var pos3 = originPos + direction3 * radius;

                    var angle4 = angle - 4;
                    var direction4 = angle4.AngleToDirection2D();
                    var pos4 = originPos + direction4 * radius;


                    Shoot(BulletPos.Position2D(), direction);
                    Shoot(pos1, direction1, false);
                    Shoot(pos2, direction2, false);
                    Shoot(pos3, direction3, false);
                    Shoot(pos4, direction4, false);

                    Clip.UseBullet();

                    ShootLight.ShowLight(BulletPos.Position2D(), direction);

                    //摄像机震动
                    CameraController.Shake.Trigger(0.08f, 4);
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
