using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Laser : QFramework.ProjectGungeon.Gun
	{
        public PlayerBullet Bullet;

        public UnityEngine.AudioSource ShootSoundPlayer;

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => ShootSoundPlayer;

        public override GunClip Clip { get; set; } = new();

        public override bool Reloading => Data.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(500);

        public override float GunAdditionalCameraSize => 1.5f;


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
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = mLaserHitPoint;
            playerBullet.Velocity = direction.normalized * 10;
            playerBullet.gameObject.SetActive(true);

            playerBullet.Damage = Random.Range(0.5f, 1.5f);//随机伤害判定


        }


        private bool mShooting = false;

        public override void ShootDown(Vector2 direction)
        {
            if (Clip.CanShoot)
            {

                Shoot(direction);
                TryPlayShootSound(true);
                mShooting = true;


            }
            else
            {
                Reload();
            }
        }

        public ShootDuration ShootDuration = new ShootDuration(0.1f);
        public override void Shooting(Vector2 direction)
        {

            if (ShootDuration.CanShoot && Clip.CanShoot)//每隔0.15秒发射一次子弹
            {
                ShootDuration.RecordShootTime();

                Shoot(direction);

                Clip.UseBullet();

                mShooting = true;

                TryPlayShootSound(true);
              
            }
            else if (!Clip.CanShoot)
            {
                AudioPlayer.Stop(); 

                mShooting = false;

                SelfLineRenderer.enabled = false;//关闭射线渲染

                TryPlayEmptyShootSound();
            }

            if (mShooting)
            {
                if (!SelfLineRenderer.enabled)
                {
                    SelfLineRenderer.enabled = true;//打开射线渲染
                }

                var layers = LayerMask.GetMask("Wall", "Enemy");
                //射线:从子弹的位置发射一条射线,方向为direction,最大距离为无限,检测的层为"Default"和"Enemy"
                var hit = Physics2D.Raycast(BulletPrefab.Position2D(), direction, float.MaxValue, layers);
                mLaserHitPoint = hit.point;
                SelfLineRenderer.SetPosition(0, BulletPrefab.Position2D());//第一个点为枪口位置
                SelfLineRenderer.SetPosition(1, hit.point);//第二个点为射线与物体的交点
            }
        }

        Vector2 mLaserHitPoint = Vector2.zero;

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();

            mShooting = false;

            SelfLineRenderer.enabled = false;//关闭射线渲染
        }

    }
}
