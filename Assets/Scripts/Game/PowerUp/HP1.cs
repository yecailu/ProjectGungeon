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
                if (Global.HP.Value < Global.MaxHP.Value)
                {

                    Room.PowerUps.Remove(this);//删除PowerUps里的元素

                    Global.HP.Value++;
                    AudioKit.PlaySound("resources://Hp1");
                    this.DestroyGameObjGracefully();
                }
                else
                {
                    Player.DisplayText("现在还不需要", 1.0f);
                }
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    }
}
