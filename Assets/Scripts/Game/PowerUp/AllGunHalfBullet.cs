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

                    //���߽��ָ����ӵ�����Ϊԭ����һ��
                    var bulletCountToAdd = gun.Config.GunBagMaxBulletCount / 2;
                    //�������Ҫ��ӵ��ӵ�����
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
