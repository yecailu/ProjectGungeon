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
            Room.PowerUps.Remove(this);//删除PowerUps里元素

            //如果碰撞玩家
            if (collision.CompareTag("Player"))
            {
                //如果有钥匙
                if (Global.Key.Value > 0)
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
                    //过滤掉已经拥有的枪械
                    configs.RemoveAll(c => GunSystem.GunList.Any(g => g.Key == c.Key));
                    //随机获取一个没有的枪械
                    if (configs.Count > 0)
                    {
                        var gunData = configs.GetRandomItem().CreateData();
                        GunSystem.GunList.Add(gunData);
                        Global.Player.UseGun(GunSystem.GunList.Count - 1);
                    }
                    else//如果已经拥有所有枪械，则生成PowerUp
                    {
                        var hp = PowerUpFactory.Default.SingleGunFullBullet.Instantiate()
                            .Position2D(transform.Position2D())
                            .Show();
                        Room.AddPowerUp(hp);
                    }

                    AudioKit.PlaySound("resources://Chest");
                    this.DestroyGameObjGracefully();
                    Global.Key.Value--;
                }
                else
                {
                    Player.DisplayText("没有钥匙", 0.5f);
                }
            }
        }

        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();


    }
}

