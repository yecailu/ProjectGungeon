using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class HP1 : ViewController,IPowerUp
	{

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);//É¾³ýPowerUpsÀïµÄÔªËØ

                Global.HP.Value++;
                AudioKit.PlaySound("resources://Hp1");
                this.DestroyGameObjGracefully(); 
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    }
}
