using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    [ViewControllerChild]

    public abstract class Gun : ViewController
    {
        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public AudioClip ReloadSound;

        public virtual Bullet BulletPrefab { get; }

        public virtual AudioSource AudioPlayer { get; }

        public virtual bool Reloading { get; }

        public virtual GunClip Clip { get; set; }

        public virtual BulletBag BulletBag { get; set; }


        public ShootBackForce BackForce = new ShootBackForce();

        public SpriteRenderer Sprite;

        public abstract float GunAdditionalCameraSize { get; }


        private void Start()
        {
            BackForce.Setup(Sprite);
        }

        private void Update()
        {
            BackForce.Update();
        }



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

        public virtual void OnRoll()
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

        public void TryPlayEmptyShootSound()
        {
            if (!Clip.CanShoot && !Reloading)//没有弹药了
            {
                if (Time.frameCount % 30 == 0)//每0.5秒播放一次空子弹声音
                {
                    AudioKit.PlaySound("resources://EmptyBulletSound");

                }
            }
        }


        public GunDate Data { get; private set; }
        internal void WithData(GunDate gunData)
        {
            Data = gunData;
            Clip.Data = Data;
            BulletBag.Data = Data;
        }
    }

}
