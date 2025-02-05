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

        public override GunClip Clip { get; set; } = new();

        public override bool Reloading => Data.Reloading;

        public override BulletBag BulletBag { get; set; } = new BulletBag(500);

        public override float GunAdditionalCameraSize => 1.5f;


        public override void OnGunUsed()
        {
            Clip.UpdateUI();
        }

        public override void Reload()
        {
            if (Reloading) return;
            BulletBag.Reload(Clip, ReloadSound);
        }

        public override void OnRoll()
        {
            AudioPlayer.Stop();
        }

        void Shoot(Vector2 direction)
        {
            var playerBullet = Instantiate(BulletPrefab);
            playerBullet.transform.position = mLaserHitPoint;
            playerBullet.Velocity = direction.normalized * 10;
            playerBullet.gameObject.SetActive(true);

            playerBullet.Damage = Random.Range(0.5f, 1.5f);//����˺��ж�


        }


        private bool mShooting = false;

        public override void ShootDown(Vector2 direction)
        {
            if (Clip.CanShoot)
            {

                Shoot(direction);
                TryPlayShootSound(true);
                mShooting = true;


            }
            else
            {
                Reload();
            }
        }

        public ShootDuration ShootDuration = new ShootDuration(0.1f);
        public override void Shooting(Vector2 direction)
        {

            if (ShootDuration.CanShoot && Clip.CanShoot)//ÿ��0.15�뷢��һ���ӵ�
            {
                ShootDuration.RecordShootTime();

                Shoot(direction);

                Clip.UseBullet();

                mShooting = true;

                TryPlayShootSound(true);
              
            }
            else if (!Clip.CanShoot)
            {
                AudioPlayer.Stop(); 

                mShooting = false;

                SelfLineRenderer.enabled = false;//�ر�������Ⱦ

                TryPlayEmptyShootSound();
            }

            if (mShooting)
            {
                if (!SelfLineRenderer.enabled)
                {
                    SelfLineRenderer.enabled = true;//��������Ⱦ
                }

                var layers = LayerMask.GetMask("Wall", "Enemy");
                //����:���ӵ���λ�÷���һ������,����Ϊdirection,������Ϊ����,���Ĳ�Ϊ"Default"��"Enemy"
                var hit = Physics2D.Raycast(BulletPrefab.Position2D(), direction, float.MaxValue, layers);
                mLaserHitPoint = hit.point;
                SelfLineRenderer.SetPosition(0, BulletPrefab.Position2D());//��һ����Ϊǹ��λ��
                SelfLineRenderer.SetPosition(1, hit.point);//�ڶ�����Ϊ����������Ľ���
            }
        }

        Vector2 mLaserHitPoint = Vector2.zero;

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();

            mShooting = false;

            SelfLineRenderer.enabled = false;//�ر�������Ⱦ
        }

    }
}
