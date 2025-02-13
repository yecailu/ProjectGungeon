using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class ShootLight
    {
        public void ShowLight(Vector2 pos, Vector2 direction)
        {

            Player.Default.GunShootLight.Position2D(pos);//设置到枪口位置
            Player.Default.GunShootLight.transform.right = direction;//设置方向
            Player.Default.GunShootLight.Show();//显示枪火光效


            ActionKit.DelayFrame(3, () =>
            {
                Player.Default.GunShootLight.Hide();
            }).StartCurrentScene();
        }
    }
}
