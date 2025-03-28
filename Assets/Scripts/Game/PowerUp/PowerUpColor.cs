using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class PowerUpColor : ViewController, IPowerUp
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {

                    Room.PowerUps.Remove(this);//ɾ��PowerUps���Ԫ��

                    Global.Color.Value++;
                    AudioKit.PlaySound("resources://Color");
                    this.DestroyGameObjGracefully();
               
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    }
}
