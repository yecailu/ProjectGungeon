using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Armor1 : ViewController,IPowerUp
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);
                Global.Armor.Value++;
                this.DestroyGameObjGracefully();
                AudioKit.PlaySound("resources://Armor1");

            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    }
}
