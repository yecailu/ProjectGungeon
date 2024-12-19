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
            if(mFrameCount % 6 == 0)
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
