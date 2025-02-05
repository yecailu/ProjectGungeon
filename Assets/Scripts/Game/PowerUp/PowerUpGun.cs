using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class PowerUpGun : ViewController, IPowerUp
    {
        public GunConfig GunConfig;

        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = Player.Default.GunWithKey(GunConfig.Key).Sprite.sprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {

                Room.PowerUps.Remove(this);//É¾³ýPowerUpsÀïµÄÔªËØ
                GunSystem.GunList.Add(GunConfig.CreateData());
                Player.Default.UseGun(GunSystem.GunList.Count - 1);
                this.DestroyGameObjGracefully();

            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => GetComponent<SpriteRenderer>();
    }
}
