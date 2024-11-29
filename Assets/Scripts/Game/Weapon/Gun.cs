using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    [ViewControllerChild]

    public class Gun : ViewController
    {
        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public virtual PlayerBullet BulletPrefab { get; }

        public virtual AudioSource AudioPlayer { get; }

        public virtual void ShootDown(Vector2 direction)
        {

            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = BulletPrefab.transform.position;
            playerBullet.Direction = direction;
            playerBullet.gameObject.SetActive(true);

            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();
        }

        public virtual void Shooting(Vector2 direction)
        {

        }
        public virtual void ShootUp(Vector2 direction)
        {

        }

        public virtual void Reload()
        {

        }
    }

}
