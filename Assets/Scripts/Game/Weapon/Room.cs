using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System;

namespace QFramework.ProjectGungeon
{
	public partial class Room : ViewController
	{
		private List<Vector3> mEnemyGeneratePoses = new List<Vector3>();

        private List<Door> mDoors = new List<Door>();

        private HashSet<Enemy> mEnemies = new HashSet<Enemy>();

        public enum RoomStates
        {
            Close,
            PlayerIn,
            Unlocked,
        }

        public RoomStates State { get; set; } = RoomStates.Close;

        public RoomConfig Config { get; private set; }
        public Room WithConfig(RoomConfig roomConfig)
        {
            Config = roomConfig;
            return this;
        }

        private void Update()
        {
            if(Time.frameCount % 30 == 0)
            {
                mEnemies.RemoveWhere(e => !e);

                if (mEnemies.Count == 0)
                {
                    if(State == RoomStates.PlayerIn)
                    {
                        State = RoomStates.Unlocked;

                        foreach (var door in mDoors)
                        {
                            door.Hide();
                        }
                    }

                }
            }
        }

        void Start()
		{
			// Code Here
		}

		public void AddEnemyGeneratePos(Vector3 enemyGeneratePos)
		{
			mEnemyGeneratePoses.Add(enemyGeneratePos);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (Config.RoomType == RoomTypes.Normal)//进入普通房间时才出现门
                {
                    if (State == RoomStates.Close)
                    {
                        State = RoomStates.PlayerIn;


                        foreach (var enemyGeneratePos in mEnemyGeneratePoses)
                        {
                            var enemy = Instantiate(LevelController.Default.Enemy);
                            enemy.transform.position = enemyGeneratePos;
                            enemy.gameObject.SetActive(true);

                            mEnemies.Add(enemy);//每一个敌人都记录下来
                        }

                        foreach (var door in mDoors)
                        {
                            door.Show();
                        }
                    }

                }
            }
        }

        public void AddDoor(Door door)
        {
            mDoors.Add(door);
        }
    }
}
