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

                //foreach (var gun in GunSystem.GunList)
                //{
                    
                //    var bag = gun.BulletBag;
                //    //���߽��ָ����ӵ�����Ϊԭ����һ��
                //    var bulletCountToAdd = bag.MaxBulletCount / 2;
                //    //�������Ҫ��ӵ��ӵ�����
                //    var gunNeedBulletCount = bag.MaxBulletCount - bag.RemainBulletCount;

                //    if (bulletCountToAdd <= gunNeedBulletCount)
                //    {
                //        bag.RemainBulletCount += bulletCountToAdd;
                //    }
                //    else
                //    {
                //        bag.RemainBulletCount = bag.MaxBulletCount;
                //    }
                //}

                Global.Player.CurrentGun.Clip.UpdateUI();

                this.DestroyGameObjGracefully();
                AudioKit.PlaySound("resources://PowerUpHalfBullet");

            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    }
}
