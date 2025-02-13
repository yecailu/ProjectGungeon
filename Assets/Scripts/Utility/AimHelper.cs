using System.Linq;
using UnityEngine;

namespace QFramework.ProjectGungeon
{
    public class AimHelper
    {
        public static IEnemy GetClosestVisibleEnemy(Transform self, Vector2 worldPositon)
        {
            return Global.CurrentRoom.Enemies
                .OrderBy(e => (e.GameObject.Position2D() - worldPositon).magnitude)//根据距离远近排序
                .FirstOrDefault(e =>
                {
                    var direction = self.Direction2DTo(e.GameObject);
              
                    if (Physics2D.Raycast(self.Position2D(), direction.normalized, direction.magnitude,
                        LayerMask.GetMask("Wall")))
                    {
                        return false;
                    }
                    return true;
                });
        }

       
    }
}
