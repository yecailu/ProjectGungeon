using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class HP1 : ViewController
	{

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Global.HP++;
                Global.HPChangedEvent.Invoke();
                AudioKit.PlaySound("resources://Hp1");
                this.DestroyGameObjGracefully(); 
            }
        }
    }
}
