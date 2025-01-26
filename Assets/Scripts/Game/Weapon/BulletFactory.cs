using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class BulletFactory : ViewController
	{
		public static BulletFactory Default;

        public static void GenBulletShell(Vector2 direction, Rigidbody2D shellPrefab = null)
        {
            if (shellPrefab == null)
            {
                shellPrefab = Default.PistolShell;
            }
            //µ¯¿ÇµôÂäÌØÐ§
            shellPrefab.Instantiate()
                .Position2D(Player.Default.Position2D() + direction * 0.5f)
                .Show()
                .Self(self =>
                {
                    var velocity = -direction * Random.Range(2, 5f) + Vector2.up * Random.Range(3, 6f);
                    var spriteRenderer = self.GetComponent<SpriteRenderer>();
                    self.velocity = velocity;
                    self.angularVelocity = Random.Range(-720, 720);

                    ActionKit
                    .Sequence()
                    .Delay(Random.Range(0.5f, 1), () =>
                    {
                        self.velocity = -direction * Random.Range(0.5f, 2f) +
                            Vector2.up * Random.Range(0, 0.5f);
                        self.gravityScale = 0.1f;
                        self.angularVelocity = RandomUtility.Choose(-1, 1) * Random.Range(180, 720);
                    })
                    .Parallel(p =>
                    {
                        p.PlaySound($"resources://BulletShell/bullet_shell ({Random.Range(1, 72 + 1)})")
                        .Delay(Random.Range(0.1f, 0.3f), () =>
                        {
                            self.angularVelocity = 0;
                            self.gravityScale = 0;
                            self.velocity = Vector2.zero;
                            ;

                        });
                    }).Start(Default);
                });
        }

        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;
        }
        void Start()
		{

		}
	}
}
