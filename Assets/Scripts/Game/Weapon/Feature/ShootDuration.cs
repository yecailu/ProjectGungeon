using UnityEngine;

namespace QFramework.ProjectGungeon
{
    /// <summary>
    /// 武器攻击间隔特性
    /// 用法:在需要进行开枪间隔的地方序列化改方法
    /// public ShootDuration shootDuration = new ShootDuration(0.25f);
    /// 
    /// if (shootDuration.CanShoot)
    /// {
    ///     shootDuration.RecordShootTime();
    ///     开枪逻辑等内容
    /// }
    /// </summary>
    public class ShootDuration
    {
        private float Duration;

        private float mLastShootTime;

        public bool CanShoot => mLastShootTime == 0 || Time.time - mLastShootTime >= Duration;

        //构造函数，方便传入开枪间隔
        public ShootDuration(float duration)
        {
            Duration = duration;
        }

        public void RecordShootTime()
        {
            mLastShootTime = Time.time;
        }

    }
}
