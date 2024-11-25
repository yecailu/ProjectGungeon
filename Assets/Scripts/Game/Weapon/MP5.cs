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

        void Shoot(Vector2 direction)
        {
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = BulletPrefab.transform.position;
            playerBullet.Direction = direction;
            playerBullet.gameObject.SetActive(true);

        }


        public override void ShootDown(Vector2 direction)
        {
            Shoot(direction);

            AudioPlayer.clip = ShootSounds[0];
            AudioPlayer.Play();
        }

        private float mCurrentSeconds = 0f;//计时器
        public override void Shooting(Vector2 direction)
        {
            mCurrentSeconds += Time.deltaTime;

            if(mCurrentSeconds >= 0.15f)//每隔0.15秒发射一次子弹
            {
                mCurrentSeconds = 0f;
                Shoot(direction);
            }
        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();
        }

        

    }

}

