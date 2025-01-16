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
        public static EasyEvent<Room> OnRoomEnter = new EasyEvent<Room>();
		private List<Vector3> mEnemyGeneratePoses = new List<Vector3>();

        private List<Door> mDoors = new List<Door>();

        public List<Door> Doors => mDoors;

        public int ColorIndex { get; set; } = -1;

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
                            var wave = mWaves.First();
                            mWaves.RemoveAt(0);
                            GenerateEnemies(wave);
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
                                        cashedPowerUp.SpriteRenderer.transform.Translate(
                                            cashedPowerUp.SpriteRenderer.NormalizedDirection2DTo(Player.Default) * 
                                            Time.fixedDeltaTime * 5);
                                    }).UnRegisterWhenGameObjectDestroyed(cashedPowerUp.SpriteRenderer.gameObject);
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
                Global.CurrentRoom = this;
                OnRoomEnter.Trigger(this);
                if (Config.RoomType == RoomTypes.Normal)//������ͨ����ʱ�ų�����
                {
                    if (State == RoomStates.Close)
                    {
                        State = RoomStates.PlayerIn;



                        //���Waves
                        var difficultyLevel = Global.CurrentPacing.Dequeue();
                        var difficultyScore = 10 + difficultyLevel * 3;
                        var waveCount = 0;
                        if (difficultyLevel <= 3)
                        {
                            waveCount = Random.Range(1, difficultyLevel + 1);
                        }
                        else
                        {
                            waveCount = Random.Range(difficultyLevel / 3, difficultyLevel / 2);
                        }

                        for (int i = 0; i < waveCount; i++)
                        {
                            var targetScore = difficultyScore / waveCount + Random.Range(-difficultyScore / 10 * 2 + 1,
                                difficultyScore / 20 * 2 + 1 + 1);
                            var waveConfig = new EnemyWaveConfig();

                            while(targetScore > 0 && waveConfig.EnemyNames.Count < mEnemyGeneratePoses.Count)
                            {
                                var enemyScore = EnemyFactory.GenTargetEnemyScore();
                                targetScore -= enemyScore;
                                waveConfig.EnemyNames.Add(EnemyFactory.EnemyByScore(enemyScore));
                            }

                            mWaves.Add(waveConfig);
                            
                        }

                        var wave = mWaves.First();
                        mWaves.RemoveAt(0);
                        GenerateEnemies(wave);

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
        void GenerateEnemies(EnemyWaveConfig waveConfig)
        {
            var pos2Gen = mEnemyGeneratePoses
                .OrderByDescending(p => (Player.Default.Position2D() - p.ToVector2()).magnitude)
                .ToList();//��mEnemyGeneratePoses����λ���б��У�ѡȡ�����λ����Զ��enemyCount��λ�õĵ���

            foreach (var enemyName in waveConfig.EnemyNames)
            {
                //�����������ɵ���
                var enemy =EnemyFactory.EnemyByName(enemyName)
                    .GameObject.Instantiate()
                    .Position2D(pos2Gen.GetAndRemoveRandomItem())
                    .Show()
                    .GetComponent<IEnemy>();

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
