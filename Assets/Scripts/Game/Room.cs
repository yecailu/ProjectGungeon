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

        public HashSet<IPowerUp> PowerUps = new HashSet<IPowerUp>();

        //���˲������ã�new EnemyWaveConfig(),ÿ��һ�����ξͼ�һ��
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

                State = RoomStates.Unlocked;
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
                            if(Config.RoomType == RoomTypes.Normal)
                            {
                                foreach (var powerUp in PowerUps.Where(p => p.GetType() == typeof(Coin)))
                                {
                                    var cashedPowerUp = powerUp;
                                    ActionKit.OnFixedUpdate.Register(() =>
                                    {
                                        //��ҳ���ҷ���
                                        cashedPowerUp.SprirerRenderer.transform.Translate(
                                            cashedPowerUp.SprirerRenderer.NormalizedDirection2DTo(Player.Default) * 
                                            Time.fixedDeltaTime * 5);
                                    }).UnRegisterWhenGameObjectDestroyed(cashedPowerUp.SprirerRenderer.gameObject);
                                }
                            }

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
                if (Config.RoomType == RoomTypes.Normal)//������ͨ����ʱ�ų�����
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
                else
                {
                    State = RoomStates.Unlocked;
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
                var enemyGameObject = Instantiate(LevelController.Default.Enemy.GameObject);
                var enemy = enemyGameObject.GetComponent<IEnemy>();
                enemyGameObject.transform.position = pos2Gen[i];
                enemyGameObject.gameObject.SetActive(true);
                enemy.Room = this;
                    
                mEnemies.Add(enemy);//ÿһ�����˶���¼����
            }

        }

        public void AddDoor(Door door)
        {
            mDoors.Add(door);
        }

        public void AddPowerUp(IPowerUp powerUp)
        {
            PowerUps.Add(powerUp);
            powerUp.Room = this;
        }
    }
}
