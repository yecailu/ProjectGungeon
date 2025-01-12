using UnityEngine;
using QFramework;
using Unity.VisualScripting;

namespace QFramework.ProjectGungeon
{
	public partial class ShopItem : ViewController
	{
        public Room Room { get; set; }
        public IPowerUp PowerUp { get; set; }

        public int ItemPrice { get; set; }

        public ShopItem UpdateView()
        {
            Price.text = $"${ItemPrice}";
            Icon.sprite = PowerUp.SpriteRenderer.sprite;
            return this;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) 
            {
                KeyBoard.Show();
            }
           
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                KeyBoard.Hide();
            }
        }

        private void Update()
        {
            if (KeyBoard.gameObject.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (Global.Coin.Value >= ItemPrice)
                    {
                        Global.Coin.Value -= ItemPrice;

                        //创建道具
                        var powerUp = PowerUp.SpriteRenderer.Instantiate()
                            .Position2D(transform.Position2D())
                            .Show();

                        Room.AddPowerUp(powerUp.GetComponent<IPowerUp>());

                        this.DestroyGameObjGracefully();
                    }
                    else
                    {
                        //金币不足提示
                        Player.DisplayText("金币不足",0.5f);
                    }
                }
            }
        }
    }
}
