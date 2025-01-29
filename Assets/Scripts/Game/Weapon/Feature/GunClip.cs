using UnityEngine;

namespace QFramework.ProjectGungeon
{
    /// <summary>
    /// 弹夹/换弹特性
    /// </summary>
    public class GunClip
    {

        public bool CanShoot => Data.CurrentBulletCount > 0 &&!Data.Reloading;



        public void UpdateUI()
        {
            GameUI.UpdateGunInfo(this);//刷新武器UI信息
        }

        public void UseBullet()
        {
            Data.CurrentBulletCount--;

            if (Data.CurrentBulletCount == 0)
            {
                Player.DisplayText("没子弹了", 2);
            }

            UpdateUI();
        }


        public bool Full => Data.CurrentBulletCount == Data.Config.ClipBulletCount;//弹夹是否已满

        public int NeedCount => Data.Config.ClipBulletCount - Data.CurrentBulletCount;//需要多少弹夹

        public GunDate Data { get; set; }

        public void Reload(AudioClip reloadSound, int reloadBulletCount = -1)
        {
            if (reloadBulletCount == -1)
            {
                reloadBulletCount = NeedCount;
            }
            Data.Reloading = true;
            UpdateUI();
            ActionKit.Sequence()
                .PlaySound(reloadSound)
                .Callback(() =>
            {
                Data.CurrentBulletCount += reloadBulletCount;
                Data.Reloading = false;
                UpdateUI();
            }).StartCurrentScene(); 

            
        }


        
    }
}
