using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Coin : ViewController,IPowerUp
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);
                Global.Coin.Value++;
                this.DestroyGameObjGracefully();
                AudioKit.PlaySound("resources://Coin"); 
 
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();

    }
}
