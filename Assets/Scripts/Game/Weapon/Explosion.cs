using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Xml.Xsl;

namespace QFramework.ProjectGungeon
{
	public partial class Explosion : ViewController
	{
		public List<Sprite> Frames;
		public string HurtTag = "Enemy";

		public int MinDamege = 5;
		public int MaxDamage = 15;


        private void Start()
        {
			//放大2倍
			this.LocalScale(2);
            //摄像机震动
            CameraController.Shake.Trigger(0.3f, 30);

            ActionKit.Custom(a =>
            {
                var frameIndex = 0;
                var updateFrameCount = 0;
                var spriteRenderer = GetComponent<SpriteRenderer>();
                a.OnStart(() =>
                {
                    frameIndex = 0;


                })
                .OnExecute(dt =>
                {
                    if (updateFrameCount >= 3)
                    {
                        updateFrameCount = 0;
                        frameIndex++;
                        if (frameIndex >= Frames.Count)
                        {
                            a.Finish();
                        }
                        else
                        {
                            spriteRenderer.sprite = Frames[frameIndex];

                        }



                        if (frameIndex == 4)
                        {
                            var filter2D = new ContactFilter2D();
                            var collider2Ds = new Collider2D[10];

                            SelfCircleCollider2D.Enable();
                            var count = SelfCircleCollider2D.OverlapCollider(filter2D, collider2Ds);

                            if (count > 0)
                            {
                                foreach (var collider2D1 in collider2Ds)
                                {
                                    if (HurtTag == "Enemy")
                                    {
                                        if (collider2D1 && collider2D1.attachedRigidbody &&
                                           collider2D1.attachedRigidbody.CompareTag("Enemy"))
                                        {
                                            var enemy = collider2D1.attachedRigidbody.GetComponent<IEnemy>();
                                            enemy?.Hurt(Random.Range(MinDamege, MaxDamage),
                                                collider2D1.Direction2DFrom(this));
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        updateFrameCount++;
                    }

                });


            }).Start(this, () =>
            {
                this.DestroyGameObjGracefully();
            }); 

        }
	}
}
