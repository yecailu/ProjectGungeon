using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Chest : ViewController
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                LevelController.Default.HP1.Instantiate()
                    .Position2D(transform.Position2D())
                    .Show();
                AudioKit.PlaySound("resources://Chest");
                this.DestroyGameObjGracefully();
            }
        }
    }
}

