using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.ProjectGungeon
{
	public partial class Aim : ViewController
	{

		private List<Sprite> mFrames = new List<Sprite>();

        private int mFrameIndex = 0;
        private int mFrameCount = 0;

        private SpriteRenderer mSpriteRenderer;

        // 是否开启瞄准
        public static bool isAimingEnabled = true; // 默认开启

        private void Awake()
        {
            mSpriteRenderer = GetComponent<SpriteRenderer>();

            mFrames.Add(Aim1);
            mFrames.Add(Aim2);
            mFrames.Add(Aim3);
        }

        private void Start()
        {
            UpdateSprite();
        }

        void UpdateSprite()
        {
            mSpriteRenderer.sprite = mFrames[mFrameIndex];
        }

        private void Update()
        {
            // 只有激活状态下才运行瞄准动画
            if (!isAimingEnabled)
                return;

            if (mFrameCount % 6 == 0)
            {
                mFrameIndex++;

                if(mFrameIndex >= mFrames.Count)
                {
                    mFrameIndex = 0;
                }

                UpdateSprite();
            }

            mSpriteRenderer.sprite = mFrames[mFrameIndex];
            mFrameCount++;
        }
    }
}
