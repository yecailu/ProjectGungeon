using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class SingleGunFullBullet : ViewController,IPowerUp
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);

                var gun = Global.Player.CurrentGun;

                if (gun.BulletBag.MaxBulletCount == gun.BulletBag.Data.GunBagRemainBulletCount)
                {

                }
                else
                {
                    var bag = gun.BulletBag;
                    //加满子弹
                    bag.Data.GunBagRemainBulletCount = bag.MaxBulletCount;

                    //更新UI
                    Global.Player.CurrentGun.Clip.UpdateUI();
                    //播放音效
                    this.DestroyGameObjGracefully();
                    AudioKit.PlaySound("resources://PowerUpHalfBullet");
                }
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    }
}
