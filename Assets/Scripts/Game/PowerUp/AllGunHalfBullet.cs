using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class AllGunHalfBullet : ViewController,IPowerUp

	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);

                foreach (var gun in GunSystem.GunList)
                {

                    //道具将恢复的子弹数量为原来的一半
                    var bulletCountToAdd = gun.Config.GunBagMaxBulletCount / 2;
                    //计算出需要添加的子弹数量
                    var gunNeedBulletCount = gun.Config.GunBagMaxBulletCount - gun.GunBagRemainBulletCount;

                    if (bulletCountToAdd <= gunNeedBulletCount)
                    {
                        gun.GunBagRemainBulletCount += bulletCountToAdd;
                    }
                    else
                    {
                        gun.GunBagRemainBulletCount = gun.Config.GunBagMaxBulletCount;
                    }
                }

                Global.Player.CurrentGun.Clip.UpdateUI();

                this.DestroyGameObjGracefully();
                AudioKit.PlaySound("resources://PowerUpHalfBullet");

            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    }
}
