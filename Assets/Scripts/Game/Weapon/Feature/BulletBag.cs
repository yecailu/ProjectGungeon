using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class BulletBag
    {
        public BulletBag(int maxBulletCount)
        {
            MaxBulletCount = maxBulletCount;
        }

        public int MaxBulletCount { get; set; }

        public bool HasBullet => Data.GunBagRemainBulletCount != 0;

        public GunDate Data { get; set; }

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
                    if (needCount <= Data.GunBagRemainBulletCount)
                    {
                        //填满子弹
                        clip.Reload(reloadSound, needCount);
                        Data.GunBagRemainBulletCount -= needCount;
                    }
                    else
                    {
                        //填满剩余子弹
                        clip.Reload(reloadSound, Data.GunBagRemainBulletCount);
                        Data.GunBagRemainBulletCount = 0;
                    }

                }
            }
        }
    }
}
