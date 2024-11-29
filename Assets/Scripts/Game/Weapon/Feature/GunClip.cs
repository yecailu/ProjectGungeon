namespace QFramework.ProjectGungeon
{
    /// <summary>
    /// 弹夹/换弹特性
    /// </summary>
    public class GunClip
    {
        public int CurrentBulletCount;

        public int ClipBulletCount;

        public bool CanShoot => CurrentBulletCount > 0;

        public GunClip(int clipBulletCount)
        {
            ClipBulletCount = clipBulletCount;
            CurrentBulletCount = clipBulletCount;

        }

        public void UpdateUI()
        {
            GameUI.UpdateGunInfo(this);//刷新武器UI信息
        }

        public void Reload()
        {
            CurrentBulletCount = ClipBulletCount;
            UpdateUI();
        }

        public void UseBullet()
        {
            CurrentBulletCount--;
            UpdateUI();
        }
    }
}
