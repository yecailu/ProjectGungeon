
using UnityEngine;
using UnityEngine.Rendering;

namespace QFramework.ProjectGungeon
{
    public class AnimationHelper
    {
        public static void UpDownAnimation(Component component, float a, int upDownFrames, long frameCount, float upDownOffset = 0)
        {
            var posY = Mathf.Lerp(-a, a, 
                ((frameCount % upDownFrames - upDownFrames * 0.5f).Abs() / (upDownFrames * 0.5f)));

            component.LocalPositionY(upDownOffset + posY);
        }

        public static void RotateAnimation(Component component, float angle, int rotateframes, long frameCount)
        {
            var angleZ = Mathf.Lerp(-angle, angle,
                ((frameCount % rotateframes - rotateframes * 0.5f).Abs() / (rotateframes * 0.5f)));

            component.LocalEulerAnglesZ(angleZ);
        }
    }
}
