using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Key : ViewController,IPowerUp
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);
                Global.Key.Value++;
                this.DestroyGameObjGracefully();
                AudioKit.PlaySound("resources://Key"); 
 
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();

    }
}
