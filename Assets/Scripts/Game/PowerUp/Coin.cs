using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Coin : ViewController
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Global.Coin.Value++;
                this.DestroyGameObjGracefully();
                AudioKit.PlaySound("resources://Coin"); 
 
            }
        }
    }
}
