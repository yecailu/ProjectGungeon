using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.ProjectGungeon
{
	public partial class PowerUpFactory : ViewController
	{
		public static PowerUpFactory Default;

        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;
        }

        public static void GeneratePowerUp(IEnemy enemy)
        {
            //Boos����
            if (enemy.IsBoss)
            {
                //��ɫ����
                var colorCount = Random.Range(3, 5 + 1); 
                for(int i = 0; i < colorCount; i++)
                {
                    var angle = Random.Range(0, 360);
                    var powerUp = Default.PowerUpColor
                        .Instantiate()
                        .Position2D(enemy.GameObject.Position2D() +
                                    angle.AngleToDirection2D() * Random.Range(0.5f, 1.0f))
                        .LocalPositionZ(0)
                        .Show();
                    enemy.Room.AddPowerUp(powerUp);
                }

                //ǹе����
                var availableGuns = GunSystem.GetAvailableGuns();
                if(availableGuns.Count > 0)
                {
                    var angle = Random.Range(0, 360);
                    var powerUp = Default.PowerUpGun
                        .Instantiate()
                        .Self(self =>
                        {
                            self.GunConfig = availableGuns.GetRandomItem();
                        })
                        .Position2D(enemy.GameObject.Position2D() +
                                    angle.AngleToDirection2D() * Random.Range(0.5f, 1.0f))
                        .LocalPositionZ(0)
                        .Show();
                    enemy.Room.AddPowerUp(powerUp);
                }

                //���ɲ���
                var powerUps = new List<IPowerUp>()
                {
                    PowerUpFactory.Default.HP1,
                    PowerUpFactory.Default.HP1,
                    Default.Armor1,
                    Default.Armor1,
                    Default.SingleGunFullBullet,
                    Default.AllGunHalfBullet,
                };

                var takeCount = Random.Range(2, 4 + 1);
                for(var i = 0; i < takeCount; i++)
                {
                    var angle = Random.Range(0, 360);
                    var powerUpObj = powerUps.GetAndRemoveRandomItem()
                        .SpriteRenderer.gameObject
                        .Instantiate()
                        .Position2D(enemy.GameObject.Position2D() +
                                    angle.AngleToDirection2D() * Random.Range(0.5f, 1.0f))
                        .LocalPositionZ(0)
                        .Show();
                    enemy.Room.AddPowerUp(powerUpObj.GetComponent<IPowerUp>());
                }
            }

            var list = new List<IPowerUp>();
            //{
            //    LevelController.Default.HP1,
            //    LevelController.Default.HP1,
            //    Default.Armor1,
            //    Default.Armor1,
            //    Default.SingleGunFullBullet,
            //    Default.AllGunHalfBullet,
            //};

            //HP����,10%���ʵ�1��HP,5%���ʵ�2��HP,5%���ʵ�1������;HP��ʱ,5%���ʵ�1��HP,5%���ʵ�1������
            if(Global.HP.Value < 6)
            {
                if(Random.Range(0, 100) < 10)
                {
                    list.Add(Default.HP1);
                }
                else if(Random.Range(0, 100) < 5)
                {
                    list.Add(Default.HP1);
                }

                if(Random.Range(0, 100) < 5)
                {
                    list.Add(Default.Armor1);
                }
            }

            if(Random.Range(0, 100) < 50)
            {
                list.Add(Default.Coin);
            }

            if(list.Count > 0)
            {
                //������
                var angle = Random.Range(0, 360);
                var powerUp = list.GetRandomItem().SpriteRenderer
                    .Instantiate()
                    .Position2D(enemy.GameObject.Position2D() + angle.AngleToDirection2D() * Random.Range(0.25f, 0.5f))
                    .Show();

                enemy.Room.AddPowerUp(powerUp.GetComponent<IPowerUp>());
            }

            
           

            
        }

    }
}
