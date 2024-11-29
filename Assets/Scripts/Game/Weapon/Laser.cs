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

        void Shoot(Vector2 direction)
        {
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = BulletPrefab.transform.position;
            playerBullet.Direction = direction;
            playerBullet.gameObject.SetActive(true);

        }


        private bool mShooting = false;

        public override void ShootDown(Vector2 direction)
        {
            Shoot(direction);

            AudioPlayer.clip = ShootSounds[0];
            AudioPlayer.Play();
            mShooting = true;

            SelfLineRenderer.enabled = true;//打开射线渲染
        }

        public ShootDuration ShootDuration = new ShootDuration(0.1f);
        public override void Shooting(Vector2 direction)
        {

            if (ShootDuration.CanShoot)//每隔0.15秒发射一次子弹
            {
                ShootDuration.RecordShootTime();
    
                Shoot(direction);
            }

            if (mShooting)
            {
                var layers = LayerMask.GetMask("Default", "Enemy");
                //射线:从子弹的位置发射一条射线,方向为direction,最大距离为无限,检测的层为"Default"和"Enemy"
                var hit = Physics2D.Raycast(BulletPrefab.Position2D(), direction, float.MaxValue, layers);
                SelfLineRenderer.SetPosition(0, BulletPrefab.Position2D());//第一个点为枪口位置
                SelfLineRenderer.SetPosition(1, hit.point);//第二个点为射线与物体的交点
            }
        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();

            mShooting = false;

            SelfLineRenderer.enabled = false;//关闭射线渲染
        }

    }
}
