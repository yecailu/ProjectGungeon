using UnityEngine;
using QFramework;
using System;

namespace QFramework.ProjectGungeon
{
	public partial class GunBase : ViewController
	{
		private bool mPlayerIn = false;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody.CompareTag("Player"))
            {
                if (!mPlayerIn)
                {
                    GameUI.Default.UIGunList.Show();
                    mPlayerIn = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.attachedRigidbody.CompareTag("Player"))
            {
                if (mPlayerIn)
                {
                    GameUI.Default.UIGunList.Hide();
                    mPlayerIn = false;
                }
            }
        }
    }
}
