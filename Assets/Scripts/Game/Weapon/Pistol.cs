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

        public ShootDuration ShootDuration = new ShootDuration(0.25f);

        public override void ShootDown(Vector2 direction)
        {
            if (ShootDuration.CanShoot)
            {
                ShootDuration.RecordShootTime();
                var playerBullet = Instantiate(BulletPrefab);
                playerBullet.transform.position = BulletPrefab.transform.position;
                playerBullet.Direction = direction;
                playerBullet.gameObject.SetActive(true);

                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();
            }
        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);//³¤°´Éä»÷
        }
        public override void ShootUp(Vector2 direction)
        {

        }
    }

}
