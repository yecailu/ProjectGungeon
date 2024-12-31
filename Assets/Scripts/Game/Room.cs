using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

namespace QFramework.ProjectGungeon
{
	public partial class Room : ViewController
	{
		private List<Vector3> mEnemyGeneratePoses = new List<Vector3>();

        private List<Door> mDoors = new List<Door>();

        public List<Door> Doors => mDoors;

        private HashSet<IEnemy> mEnemies = new HashSet<IEnemy>();

        public HashSet<IEnemy> Enemies => mEnemies;

        public LevelController.RoomGenerateNode GenerateNode { get; set; }

        //敌人波次配置，new EnemyWaveConfig(),每多一个波次就加一个
        private List<EnemyWaveConfig> mWaves = new List<EnemyWaveConfig>();

        private EnemyWaveConfig mCurrentWave = null;

        public enum RoomStates
        {
            Close,
            PlayerIn,
            Unlocked,
        }

        public RoomStates State { get; set; } = RoomStates.Close;

        public RoomConfig Config { get; private set; } = new RoomConfig();
        public Room WithConfig(RoomConfig roomConfig)
        {
            Config = roomConfig;
            return this;
        }


        private void Start()
        {
            if (Config.RoomType == RoomTypes.Init)
            {
                foreach (var door in mDoors)
                {
                    door.State.ChangeState(Door.States.IdleOpen);
                }
            }
            else if (Config.RoomType == RoomTypes.Normal)
            {
                var wavesCount = Random.Range(1, 3 + 1);

                for (int i = 0; i < wavesCount; i++)
                {
                    mWaves.Add(new EnemyWaveConfig());
                }
            }
        }

        private void Update()
        {
            if(Time.frameCount % 30 == 0)
            {
                mEnemies.RemoveWhere(e => !e.GameObject);

                if (mEnemies.Count == 0)//敌人全部死亡
                {
                    if (State == RoomStates.PlayerIn)//房间状态为玩家已进入
                    {

                        if (mWaves.Count > 0)//有剩余波次继续生成敌人
                        {
                            GenerateEnemies();
                        }
                        else//没有波次 开门
                        {

                            State = RoomStates.Unlocked;

                            foreach (var door in mDoors)
                            {
                                door.State.ChangeState(Door.States.Open);
                            }
                        }
                    }

                }
            }
        }


		public void AddEnemyGeneratePos(Vector3 enemyGeneratePos)
		{
			mEnemyGeneratePoses.Add(enemyGeneratePos);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Global.currentRoom = this;
                if (Config.RoomType == RoomTypes.Normal)//进入普通房间时才出现门
                {
                    if (State == RoomStates.Close)
                    {
                        State = RoomStates.PlayerIn;

                        GenerateEnemies();

                        foreach (var door in mDoors)
                        {
                            door.State.ChangeState(Door.States.BattleClose);
                        }
                    }

                }
            }
        }

        //生成敌人方法
        void GenerateEnemies()
        {
            mWaves.RemoveAt(0);


            var enemyCount = UnityEngine.Random.Range(3, 5 + 1);

            var pos2Gen = mEnemyGeneratePoses
                .OrderByDescending(p => (Player.Default.Position2D() - p.ToVector2()).magnitude)
                .Take(enemyCount).ToList();//在mEnemyGeneratePoses敌人位置列表中，选取离玩家位置最远的enemyCount个位置的敌人

            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = Instantiate(LevelController.Default.Enemy.GameObject);
                enemy.transform.position = pos2Gen[i];
                enemy.gameObject.SetActive(true);
                    
                mEnemies.Add(enemy.GetComponent<IEnemy>());//每一个敌人都记录下来
            }

        }

        public void AddDoor(Door door)
        {
            mDoors.Add(door);
        }
    }
}
