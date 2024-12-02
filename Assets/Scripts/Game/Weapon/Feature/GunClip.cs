using UnityEngine;

namespace QFramework.ProjectGungeon
{
    /// <summary>
    /// 弹夹/换弹特性
    /// </summary>
    public class GunClip
    {
        public int CurrentBulletCount;

        public int ClipBulletCount;

        public bool CanShoot => CurrentBulletCount > 0 &&!Reloading;

        public GunClip(int clipBulletCount)
        {
            ClipBulletCount = clipBulletCount;
            CurrentBulletCount = clipBulletCount;

        }

        public void UpdateUI()
        {
            GameUI.UpdateGunInfo(this);//刷新武器UI信息
        }


        public bool Full => CurrentBulletCount == ClipBulletCount;//弹夹是否已满

        public bool Reloading = false;

        public int NeedCount => ClipBulletCount - CurrentBulletCount;//需要多少弹夹

        public void Reload(AudioClip reloadSound, int reloadBulletCount = -1)
        {
            if (reloadBulletCount == -1)
            {
                reloadBulletCount = NeedCount;
            }
            Reloading = true;
            ActionKit.Sequence().PlaySound(reloadSound).Callback(() =>
            {
                CurrentBulletCount += reloadBulletCount;
                UpdateUI();
                Reloading = false;
            }).StartCurrentScene();

            
        }


        public void UseBullet()
        {
            CurrentBulletCount--;
            UpdateUI();
        }
    }
}
