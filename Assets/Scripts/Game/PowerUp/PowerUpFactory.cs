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
