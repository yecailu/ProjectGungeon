using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Laser : QFramework.ProjectGungeon.Gun
	{
        public PlayerBullet Bullet;

        public UnityEngine.AudioSource ShootSoundPlayer;

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => ShootSoundPlayer;

        void Shoot(Vector2 direction)
        {
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = BulletPrefab.transform.position;
            playerBullet.Direction = direction;
            playerBullet.gameObject.SetActive(true);

        }


        private bool mShooting = false;

        public override void ShootDown(Vector2 direction)
        {
            Shoot(direction);

            AudioPlayer.clip = ShootSounds[0];
            AudioPlayer.Play();
            mShooting = true;

            SelfLineRenderer.enabled = true;//��������Ⱦ
        }

        public ShootDuration ShootDuration = new ShootDuration(0.1f);
        public override void Shooting(Vector2 direction)
        {

            if (ShootDuration.CanShoot)//ÿ��0.15�뷢��һ���ӵ�
            {
                ShootDuration.RecordShootTime();
    
                Shoot(direction);
            }

            if (mShooting)
            {
                var layers = LayerMask.GetMask("Default", "Enemy");
                //����:���ӵ���λ�÷���һ������,����Ϊdirection,������Ϊ����,���Ĳ�Ϊ"Default"��"Enemy"
                var hit = Physics2D.Raycast(BulletPrefab.Position2D(), direction, float.MaxValue, layers);
                SelfLineRenderer.SetPosition(0, BulletPrefab.Position2D());//��һ����Ϊǹ��λ��
                SelfLineRenderer.SetPosition(1, hit.point);//�ڶ�����Ϊ����������Ľ���
            }
        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();

            mShooting = false;

            SelfLineRenderer.enabled = false;//�ر�������Ⱦ
        }

    }
}
