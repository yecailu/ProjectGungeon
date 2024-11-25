using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace QFramework.ProjectGungeon
{
    public partial class Pistol : Gun
    {

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override void ShootDown(Vector2 direction)
        {


        }

        public override void Shooting(Vector2 direction)
        {

        }
        public override void ShootUp(Vector2 direction)
        {

        }
    }

}
