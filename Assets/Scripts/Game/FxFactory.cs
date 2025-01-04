using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class FxFactory : ViewController
	{
        public static void PlayHurtFx(Vector2 pos, Color color = default)
        {
            if (color == default)
            {
                color = Color.red;
            }
            Default.HurtFx
                .Instantiate()
                .Position2D(pos)
                .Show()
                .Self(self =>
                {
                    var main = self.main;
                    main.startColor = color;
                    ActionKit.Delay(self.main.duration +0.3f, self.DestroyGameObjGracefully).StartCurrentScene();
                })
                .Play();
        }

        public static void PlayEnemyBlood(Vector2 originPos)
        {
            //播放贱血动画
            var blood = Default.EnemyBlood.Instantiate()
                .Position2D(originPos) 
                .EulerAnglesZ(Random.Range(0, 360f))
                .LocalScale(0.1f)
                .Show();

            var bloodOriginPos = blood.Position2D();
            var angle = Random.Range(0, 360f);//随机角度
            var radius = Random.Range(0.2f, 1.5f);//随机半径
            var moveBy = angle.AngleToDirection2D() * radius;//随机位置
            var scaleTo = Random.Range(0.2f, 3f);//随机大小


            ActionKit.Lerp(0, 1, Random.Range(0.1f, 0.3f),
                (p) =>
                {
                    p = EaseUtility.InCubic(0, 1, p);
                    blood.Position2D(bloodOriginPos + moveBy * p);
                    blood.LocalScale(scaleTo * p);
                }).StartCurrentScene();


        }

        public static void PlayPlayerBlood(Vector2 originPos)
        {
            //播放贱血动画
            var blood = Default.PlayerBlood.Instantiate()
                .Position2D(originPos)
                .EulerAnglesZ(Random.Range(0, 360f))
                .LocalScale(0.1f)
                .Show();

            var bloodOriginPos = blood.Position2D();
            var angle = Random.Range(0, 360f);//随机角度
            var radius = Random.Range(0.2f, 1.5f);//随机半径
            var moveBy = angle.AngleToDirection2D() * radius;//随机位置
            var scaleTo = Random.Range(0.2f, 3f);//随机大小


            ActionKit.Lerp(0, 1, Random.Range(0.1f, 0.3f),
                (p) =>
                {
                    p = EaseUtility.InCubic(0, 1, p);
                    blood.Position2D(bloodOriginPos + moveBy * p);
                    blood.LocalScale(scaleTo * p);
                }).StartCurrentScene();


        }
        public static FxFactory Default;

        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
