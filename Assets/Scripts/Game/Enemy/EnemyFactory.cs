using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class EnemyFactory : ViewController
	{
		public static EnemyFactory Default;

        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;
        }

        public static IEnemy EnemyByName(string enemyName)
        {
            //µ–»À≈‰÷√

            return enemyName switch
            {
                Constant.EnemyA => Default.EnemyA,
                Constant.EnemyB => Default.EnemyB,
                Constant.EnemyC => Default.EnemyC,
                Constant.EnemyD => Default.EnemyD,
                Constant.EnemyE => Default.EnemyE,
                Constant.EnemyF => Default.EnemyF,
                Constant.EnemyG => Default.EnemyG,
                Constant.EnemyH => Default.EnemyH,
                Constant.EnemyABig => Default.EnemyABig,
                Constant.EnemyBBig => Default.EnemyBBig,
                Constant.EnemyCBig => Default.EnemyCBig,
                Constant.EnemyDBig => Default.EnemyDBig,
                Constant.BossA => Default.BossA,
                Constant.BossB => Default.BossB,
                Constant.BossC => Default.BossC,
                Constant.BossD => Default.BossD,
                Constant.BossE => Default.BossE,

                _ => null,
            };
        }

        public static int GenTargetEnemyScore()
        {
            if (Level1.Config == Global.CurrentLevel)
            {
                return RandomUtility.Choose(2, 2, 3, 3, 8);
            }
            if (Level2.Config == Global.CurrentLevel)
            {
                return RandomUtility.Choose(2, 2, 3, 3, 4, 4, 8, 9);
            }
            if (Level3.Config == Global.CurrentLevel)
            {
                return RandomUtility.Choose(2, 3, 4, 5, 9);
            }
            if (Level4.Config == Global.CurrentLevel)
            {
                return RandomUtility.Choose(3, 4, 5, 6, 7, 9, 10);
            }


            return RandomUtility.Choose(2, 3, 4, 5, 6, 7, 8, 9, 10);
        }

        public static string EnemyByScore(int score)
        {
            //≤‚ ‘”√
            //return Constant.EnemyA;


            if (score == 2)
            {
                return Constant.EnemyA;
            }
            if(score == 3)
            {
                return RandomUtility.Choose(Constant.EnemyB, Constant.EnemyC);
            }
            if(score == 4)
            {
                return RandomUtility.Choose(Constant.EnemyD);
            }
            if(score == 5)
            {
                return RandomUtility.Choose(Constant.EnemyF);
            }
            if(score == 6)
            {
                return RandomUtility.Choose(Constant.EnemyG);
            }
            if(score == 7)
            {
                return RandomUtility.Choose(Constant.EnemyH);
            }
            if(score == 8)
            {
                return RandomUtility.Choose(Constant.EnemyABig);
            }
           if(score == 9)
            {
                return RandomUtility.Choose(Constant.EnemyBBig, Constant.EnemyCBig);
            }
            if(score == 10)
            {
                return RandomUtility.Choose(Constant.EnemyDBig);
            }

            return null;
        }

    }
}
