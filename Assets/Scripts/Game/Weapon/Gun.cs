using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    [ViewControllerChild]

    public class Gun : ViewController
    {
        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public AudioClip ReloadSound;

        public virtual PlayerBullet BulletPrefab { get; }

        public virtual AudioSource AudioPlayer { get; }

        public virtual bool Reloading { get; }

        public virtual void OnGunUsed()
        {

        }


        public virtual void ShootDown(Vector2 direction)
        {

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

        public void TryPlayShootSound(bool loop = false)
        {
            if (!AudioPlayer.isPlaying)
            {
                AudioPlayer.clip = ShootSounds[0];
                AudioPlayer.loop = true;
                AudioPlayer.Play();
            }
        }
    }

}
