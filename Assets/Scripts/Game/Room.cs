using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System;
using System.Linq;

namespace QFramework.ProjectGungeon
{
	public partial class Room : ViewController
	{
		private List<Vector3> mEnemyGeneratePoses = new List<Vector3>();

        private List<Door> mDoors = new List<Door>();

        private HashSet<Enemy> mEnemies = new HashSet<Enemy>();

        //���˲������ã�new EnemyWaveConfig(),ÿ��һ�����ξͼ�һ��
        private List<EnemyWaveConfig> mWaves = new List<EnemyWaveConfig>()
        {
            new EnemyWaveConfig(),
            new EnemyWaveConfig(),
            new EnemyWaveConfig(),



        };

        private EnemyWaveConfig mCurrentWave = null;

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

                if (mEnemies.Count == 0)//����ȫ������
                {
                    if (State == RoomStates.PlayerIn)//����״̬Ϊ����ѽ���
                    {

                        if (mWaves.Count > 0)//��ʣ�ನ�μ������ɵ���
                        {
                            GenerateEnemies();
                        }
                        else//û�в��� ����
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
                if (Config.RoomType == RoomTypes.Normal)//������ͨ����ʱ�ų�����
                {
                    if (State == RoomStates.Close)
                    {
                        State = RoomStates.PlayerIn;

                        GenerateEnemies();

                        foreach (var door in mDoors)
                        {
                            door.Show();
                        }
                    }

                }
            }
        }

        //���ɵ��˷���
        void GenerateEnemies()
        {
            mWaves.RemoveAt(0);


            var enemyCount = UnityEngine.Random.Range(3, 5 + 1);

            var pos2Gen = mEnemyGeneratePoses
                .OrderByDescending(p => (Player.Default.Position2D() - p.ToVector2()).magnitude)
                .Take(enemyCount).ToList();//��mEnemyGeneratePoses����λ���б��У�ѡȡ�����λ����Զ��enemyCount��λ�õĵ���

            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = Instantiate(LevelController.Default.Enemy);
                enemy.transform.position = pos2Gen[i];
                enemy.gameObject.SetActive(true);
                    
                mEnemies.Add(enemy);//ÿһ�����˶���¼����
            }

        }

        public void AddDoor(Door door)
        {
            mDoors.Add(door);
        }
    }
}
