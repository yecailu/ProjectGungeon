using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class BulletBag
    {
        public BulletBag(int remanBulletCount, int maxBulletCount)
        {
            RemainBulletCount = remanBulletCount;
            MaxBulletCount = maxBulletCount;
        }

        public int RemainBulletCount { get; set; }
        public int MaxBulletCount { get; set; }

        public bool HasBullet => RemainBulletCount != 0;

        public void Reload(GunClip clip, AudioClip reloadSound)
        {
            if (clip.Full)
            {
                //不用换弹
            }
            else
            {
                if(MaxBulletCount == -1)
                {
                    //无限弹药
                    var needCount = clip.NeedCount;
                    clip.Reload(reloadSound, needCount);

                }
                if (HasBullet)
                {
                    var needCount = clip.NeedCount;
                    if (needCount <= RemainBulletCount)
                    {
                        //填满子弹
                        clip.Reload(reloadSound, needCount);
                        RemainBulletCount -= needCount;
                    }
                    else
                    {
                        //填满剩余子弹
                        clip.Reload(reloadSound, RemainBulletCount);
                        RemainBulletCount = 0;
                    }

                }
            }
        }
    }
}
