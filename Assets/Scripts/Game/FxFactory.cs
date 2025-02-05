using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class FxFactory : ViewController
	{
        public static Transform PlayEnemyDieBody(Vector2 pos, Vector2 flyDirection, string name, float scale)
        {
            var dieBody = Default.transform.Find(name)
                .GetComponent<SpriteRenderer>()
                .Instantiate()
                .LocalScale(scale)
                .LocalEulerAnglesZ(Random.Range(-45, 45))
                .Self(self =>
                {
                    self.flipX = RandomUtility.Choose(true, false);
                    self.flipY = RandomUtility.Choose(true, false);
                })
                .Show();

            var dieBodyOriginPos = pos; //����������ʼλ��
            var dieBodyMoveToDistance = Random.Range(0.5f, 1.5f);//ʬ���ƶ�����


            ActionKit.Lerp(0, 1, 0.3f, (p) =>
            {
                dieBody.transform.Position2D(Vector2.Lerp(dieBodyOriginPos,
                    dieBodyOriginPos + dieBodyMoveToDistance * flyDirection, p));

            }).StartCurrentScene();

            
            return dieBody.transform;
        }

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
            //���ż�Ѫ����
            var blood = Default.EnemyBlood.Instantiate()
                .Position2D(originPos) 
                .EulerAnglesZ(Random.Range(0, 360f))
                .LocalScale(0.1f)
                .Show();

            var bloodOriginPos = blood.Position2D();
            var angle = Random.Range(0, 360f);//����Ƕ�
            var radius = Random.Range(0.2f, 1.5f);//����뾶
            var moveBy = angle.AngleToDirection2D() * radius;//���λ��
            var scaleTo = Random.Range(0.2f, 3f);//�����С


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
            //���ż�Ѫ����
            var blood = Default.PlayerBlood.Instantiate()
                .Position2D(originPos)
                .EulerAnglesZ(Random.Range(0, 360f))
                .LocalScale(0.1f)
                .Show();

            var bloodOriginPos = blood.Position2D();
            var angle = Random.Range(0, 360f);//����Ƕ�
            var radius = Random.Range(0.2f, 1.5f);//����뾶
            var moveBy = angle.AngleToDirection2D() * radius;//���λ��
            var scaleTo = Random.Range(0.2f, 3f);//�����С


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
