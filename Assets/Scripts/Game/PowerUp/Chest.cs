using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Chest : ViewController,IPowerUp
	{
        public Room Room { get; set; }

        

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Room.PowerUps.Remove(this);//É¾³ýPowerUpsÀïÔªËØ

            if (collision.CompareTag("Player"))
            {
                var hp = LevelController.Default.HP1.Instantiate()
                    .Position2D(transform.Position2D())
                    .Show();

                Room.AddPowerUp(hp);

                AudioKit.PlaySound("resources://Chest");
                this.DestroyGameObjGracefully();

            }
        }

        public SpriteRenderer SprirerRenderer => GetComponent<SpriteRenderer>();


    }
}

