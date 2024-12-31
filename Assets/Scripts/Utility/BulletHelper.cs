using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace QFramework.ProjectGungeon
{
    public class BulletHelper
    {
        public static void Shoot(Vector2 pos, Vector2 direction, float speed, float damage)
        {
            var playerBullet = Object.Instantiate(BulletFactory.Default.PistolBullet);
            playerBullet.transform.position = pos;
            playerBullet.Velocity = direction.normalized * speed;
            playerBullet.gameObject.SetActive(true);

            playerBullet.Damage = damage;//随机伤害判定
        }

        public static void ShootSpread(int count, float durationAngle, Vector2 origin, Vector2 mainDirection, float radius, EnemyBullet bulletPrefab ,float speed = 5)
        {
            //敌人到玩家的方向

            var mainAngle = mainDirection.ToAngle();//将敌人朝向设置成Vector2再变成欧拉角
            for (int i = 0; i < count; i++)
            {
                var angle = mainAngle + i * durationAngle - count * durationAngle * 0.5f;
                var direction = angle.AngleToDirection2D();
                var pos = origin + radius * direction;//子弹出现位置


                //敌人子弹逻辑
                var enemyBullet = Object.Instantiate(bulletPrefab);
                enemyBullet.transform.position = pos;
                enemyBullet.Velocity = direction * speed;
                enemyBullet.gameObject.SetActive(true);

            }
        }
        public static void ShootAround(int count, Vector2 origin, float radius, EnemyBullet bulletPrefab, float speed = 5)
        {
            var durationAngle = 360 / count;

            var angleOffset = Random.Range(0, 360);
            for (int i = 0; i < count; i++)
            {
                var angle = angleOffset + i * durationAngle;
                var direction = angle.AngleToDirection2D();
                var pos = origin + radius * direction;//子弹出现位置


                //敌人子弹逻辑
                var enemyBullet = Object.Instantiate(bulletPrefab);
                enemyBullet.transform.position = pos;
                enemyBullet.Velocity = direction * speed;
                enemyBullet.gameObject.SetActive(true);

            }
        }

    }
}
