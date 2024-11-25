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

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override void ShootDown(Vector2 direction)
        {

            var playerBullet = Instantiate(Bullet);
            playerBullet.transform.position = Bullet.transform.position;
            playerBullet.Direction = direction;
            playerBullet.gameObject.SetActive(true);

            var soundIndex = Random.Range(0, ShootSounds.Count);
            SelfAudioSource.clip = ShootSounds[soundIndex];
            SelfAudioSource.Play();

        }

        public override void Shooting(Vector2 direction)
        {

        }
        public override void ShootUp(Vector2 direction)
        {

        }
    }

}

