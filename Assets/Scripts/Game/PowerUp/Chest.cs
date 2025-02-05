using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace QFramework.ProjectGungeon
{
	public partial class Chest : ViewController,IPowerUp
	{
        public Room Room { get; set; }

        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Room.PowerUps.Remove(this);//ɾ��PowerUps��Ԫ��

            //�����ײ���
            if (collision.CompareTag("Player"))
            {
                //�����Կ��
                if (Global.Key.Value > 0)
                {
                    var configs = GunSystem.GetAvailableGuns();

                    //�������һ��û�е�ǹе
                    if (configs.Count > 0)
                    {
                        var powerUpGun = PowerUpFactory.Default.PowerUpGun.Instantiate()
                            .Position2D(transform.Position2D())
                            .Self(self =>{ self.GunConfig = configs.GetRandomItem(); })
                            .Show();

                        Room.AddPowerUp(powerUpGun);
                    }
                    else//����Ѿ�ӵ������ǹе��������PowerUp
                    {

                        var powerUp = RandomUtility.Choose<IPowerUp>(
                            PowerUpFactory.Default.SingleGunFullBullet,
                            PowerUpFactory.Default.AllGunHalfBullet,
                            PowerUpFactory.Default.Armor1)
                            .SpriteRenderer.gameObject
                            .Instantiate()
                            .Position2D(transform.Position2D())
                            .Show()
                            .GetComponent<IPowerUp>();

                        Room.AddPowerUp(powerUp);
                    }

                    AudioKit.PlaySound("resources://Chest");
                    this.DestroyGameObjGracefully();
                    Global.Key.Value--;
                }
                else
                {
                    Player.DisplayText("û��Կ��", 0.5f);
                }
            }
        }

        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();


    }
}

