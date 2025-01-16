using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Linq;

namespace QFramework.ProjectGungeon
{
	public partial class Chest : ViewController,IPowerUp
	{
        public Room Room { get; set; }

        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Room.PowerUps.Remove(this);//ɾ��PowerUps��Ԫ��

            if (collision.CompareTag("Player"))
            {


                var configs = new List<GunConfig>()
                {
                    GunConfig.Rocket,
                    GunConfig.Bow,
                    GunConfig.Laser,
                    GunConfig.ShotGun,
                    GunConfig.AK47,
                    GunConfig.AWP,
                    GunConfig.MP5
                };
                //���˵��Ѿ�ӵ�е�ǹе
                configs.RemoveAll(c => GunSystem.GunList.Any(g => g.Key == c.Key));
                //�����ȡһ��û�е�ǹе
                if (configs.Count > 0)
                {
                    var gunData = configs.GetRandomItem().CreateData();
                    GunSystem.GunList.Add(gunData);
                    Global.Player.UseGun(GunSystem.GunList.Count - 1);
                }
                else//����Ѿ�ӵ������ǹе��������PowerUp
                {
                    var hp = PowerUpFactory.Default.SingleGunFullBullet.Instantiate()
                        .Position2D(transform.Position2D())
                        .Show();
                    Room.AddPowerUp(hp);
                }

                AudioKit.PlaySound("resources://Chest");
                this.DestroyGameObjGracefully();

            }
        }

        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();


    }
}

