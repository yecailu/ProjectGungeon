using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class ShootBackForce
    {
        public SpriteRenderer mSpriteRenderer;

        private Vector2 mSpriteOriginLocalPos;

        public Vector2 mSpriteBackwardPos;

        private int mSpriteBackwardFrames;

        private int mSpriteBackwardTotalFrames;

        public void Setup(SpriteRenderer spriteRenderer)
        {
            mSpriteRenderer = spriteRenderer;
            mSpriteOriginLocalPos = spriteRenderer.LocalPosition2D();
        }

        public void Update()
        {
            if(mSpriteBackwardFrames > 0)
            {
                mSpriteRenderer.LocalPosition2D(Vector2.Lerp(mSpriteBackwardPos, mSpriteOriginLocalPos,
                    1 - mSpriteBackwardFrames / (float)mSpriteBackwardTotalFrames));
                mSpriteBackwardFrames--; 
            }
        }

        public void Shoot(float a, int frames)
        {
            mSpriteBackwardPos = mSpriteOriginLocalPos + Vector2.left * a * 2;
            mSpriteBackwardFrames = frames * 2;
            mSpriteBackwardTotalFrames = frames * 2;
        }

    }
}
