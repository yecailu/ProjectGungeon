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
