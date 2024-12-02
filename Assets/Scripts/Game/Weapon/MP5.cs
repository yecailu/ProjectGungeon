using UnityEngine;
using QFramework;
using System.Collections.Generic;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.ProjectGungeon
{
    public partial class MP5 : Gun
    {
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => ShootSoundPlayer;

        public GunClip Clip = new GunClip(30);

        public ShootLight ShootLight = new ShootLight();

        public override bool Reloading => Clip.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(500, 500);


        public override void OnGunUsed()
        {
            Clip.UpdateUI();
        }

        public override void Reload()
        {
            BulletBag.Reload(Clip, ReloadSound);
        }

        void Shoot(Vector2 direction)
        {
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = BulletPrefab.transform.position;
            playerBullet.Direction = direction;
            playerBullet.gameObject.SetActive(true);
            Clip.UseBullet();

            playerBullet.Damage = Random.Range(1f, 2f);//随机伤害判定


            ShootLight.ShowLight(BulletPrefab.Position2D(), direction);

        }


        public override void ShootDown(Vector2 direction)
        {
            if(!Clip.CanShoot) return;
            Shoot(direction);

            TryPlayShootSound(true);
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
            }
        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();
        }

        

    }

}

