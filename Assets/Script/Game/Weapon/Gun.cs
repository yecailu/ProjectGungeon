using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
public abstract class Gun : ViewController
    {
        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public abstract PlayerBullet BulletPrefab { get; }

        public abstract AudioSource AudioPlayer { get; }

        public virtual void ShootDown(Vector2 direction)
        {

        }

        public virtual void Shooting(Vector2 direction)
        {

        }
        public virtual void ShootUp(Vector2 direction)
        {

        }
    }

}
