using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace QFramework.ProjectGungeon
{
	public partial class Box : ViewController,IPowerUp
	{
        public Room Room { get; set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("PlayerBullet") || collision.collider.CompareTag("EnemyBullet"))
            {
                var randomValue = Random.Range(0, 100);
                if(randomValue < 3)
                {
                    var powerUp = RandomUtility.Choose<IPowerUp>(
                           PowerUpFactory.Default.HP1,
                           PowerUpFactory.Default.Armor1)
                           .SpriteRenderer.gameObject
                           .Instantiate()
                           .Position2D(transform.Position2D())
                           .Show()
                           .GetComponent<IPowerUp>();

                    Room.AddPowerUp(powerUp);
                }
                else if(randomValue < 20)
                {
                    var powerUp = RandomUtility.Choose<IPowerUp>(
                           PowerUpFactory.Default.Coin)
                           .SpriteRenderer.gameObject
                           .Instantiate()
                           .Position2D(transform.Position2D())
                           .Show()
                           .GetComponent<IPowerUp>();

                    Room.AddPowerUp(powerUp);
                }

                //调整音量大小               
                var audioPlayer = AudioKit.PlaySound("resources://Box Break");
                audioPlayer?.SetVolume(0.5f);

                Destroy(collision.collider.gameObject);
                Destroy(gameObject);
            }
        }



        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();


    }
}

