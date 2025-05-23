using UnityEngine;
using QFramework;
using System.Collections.Generic;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.ProjectGungeon
{
    public partial class MP5 : Gun
    {
        public override AudioSource AudioPlayer => ShootSoundPlayer;

        public override GunClip Clip { get; set; } = new();

        public ShootLight ShootLight = new ShootLight();

        public override bool Reloading => Data.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(500);

        public float UnstableRate => 0.2f;

        public override float GunAdditionalCameraSize => 1;

        public PlayerBullet MP5Bullet;


        public override void OnGunUsed()
        {
            Clip.UpdateUI();
        }

        public override void Reload()
        {
            if (Reloading) return;
            BulletBag.Reload(Clip, ReloadSound);
        }

        public override void OnRoll()
        {
            AudioPlayer.Stop();
        }

        void Shoot(Vector2 direction)
        {
            var angle = direction.ToAngle() + Random.Range(0.05f, UnstableRate) * 30 * RandomUtility.Choose(-1, 1);


            BulletHelper.Shoot(BulletPos.Position2D(), angle.AngleToDirection2D(), 20, Random.Range(1f, 2f), MP5Bullet);
 
            Clip.UseBullet();

            ShootLight.ShowLight(BulletPos.Position2D(), direction);

            //摄像机震动
            CameraController.Shake.Trigger(0.05f, 2);

            BackForce.Shoot(0.05f, 2);

            BulletFactory.GenBulletShell(direction);

        }


        public override void ShootDown(Vector2 direction)
        {
            if (ShootDuration.CanShoot && Clip.CanShoot)
            {

                ShootDuration.RecordShootTime();

                Shoot(direction);

                TryPlayShootSound(true);

            }
            else
            {
                Reload();
            }
        }

        public ShootDuration ShootDuration = new ShootDuration(0.1f);
        public override void Shooting(Vector2 direction)
        {

            if(ShootDuration.CanShoot && Clip.CanShoot)//每隔0.15秒发射一次子弹
            {
                ShootDuration.RecordShootTime();
                Shoot(direction);

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
        }

        

    }

}

