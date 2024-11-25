using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace QFramework.ProjectGungeon
{
    public partial class ShotGun : QFramework.ProjectGungeon.Gun
    {

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        void Shoot(Vector2 position, Vector2 direction, bool playSound = true)
        {
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = position;
            playerBullet.Direction = direction.normalized;
            playerBullet.gameObject.SetActive(true);

            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();
        }

        public float ShootDuration => 1;//每间隔1秒射击一次
        private float mLastShootTime = 0;

        public override void ShootDown(Vector2 direction)
        {
            if (mLastShootTime == 0 || Time.time - mLastShootTime >= ShootDuration)
            {
                mLastShootTime = Time.time;
                var angle = direction.ToAngle();//得到欧拉角
                var originPos = transform.parent.Position2D();//得到ShotGun父类Weapon的2D坐标
                var radius = (BulletPrefab.Position2D() - originPos).magnitude;//得到子弹的半径

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


                Shoot(BulletPrefab.Position2D(), direction);
                Shoot(pos1, direction1, false);
                Shoot(pos2, direction2, false);
                Shoot(pos3, direction3, false);
                Shoot(pos4, direction4, false);
            }
        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);
        }
    }
}
