using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System;
using System.Linq;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;
using UnityEditorInternal;

namespace QFramework.ProjectGungeon
{ 
	public partial class Room : ViewController
	{
        public DynaGrid<PathFindingHelper.TileNode> PathFindingGrid;//寻路
        public Vector3Int LB;
        public Vector3Int RT;
        //寻路方法
        public void PrepareAStarNodes()
        {
            if(Config.RoomType == RoomTypes.Final || Config.RoomType == RoomTypes.Normal)
            {
                PathFindingGrid = new DynaGrid<PathFindingHelper.TileNode>();

                for(var i = LB.x; i <= RT.x; i++)
                {
                    for(var j = LB.y; j <= RT.y; j++)
                    {
                        var walkable = LevelController.Default.WallTilemap.GetTile(new Vector3Int(i, j, 0)) == null;

                        PathFindingGrid[i, j] = new PathFindingHelper.TileNode(PathFindingGrid);
                        PathFindingGrid[i, j].Init(walkable, new PathFindingHelper.TileCoords()
                        {
                            Pos = new Vector3Int(i, j, 0)
                        }) ;

                    }
                }

                PathFindingGrid.ForEach(node => node.CacheNeighbors());
            }
        }

        public static EasyEvent<Room> OnRoomEnter = new EasyEvent<Room>();
		private List<Vector3> mEnemyGeneratePoses = new List<Vector3>();
        private List<Vector3> mShopItemGeneratePoses = new List<Vector3>();


        private List<Door> mDoors = new List<Door>();

        public List<Door> Doors => mDoors;

        public int ColorIndex { get; set; } = -1;

        private HashSet<IEnemy> mEnemies = new HashSet<IEnemy>();

        public HashSet<IEnemy> Enemies => mEnemies;

        public LevelController.RoomGenerateNode GenerateNode { get; set; }

        public HashSet<IPowerUp> PowerUps = new HashSet<IPowerUp>();

        //敌人波次配置，new EnemyWaveConfig(),每多一个波次就加一个
        private List<EnemyWaveConfig> mWaves = new List<EnemyWaveConfig>();

        private EnemyWaveConfig mCurrentWave = null;
         
        public enum RoomStates
        {
            Close,
            PlayerIn,
            Unlocked,
        }

        public FSM<RoomStates> State = new FSM<RoomStates>();

        public RoomConfig Config { get; private set; } = new RoomConfig();

        public Room WithConfig(RoomConfig roomConfig)
        {
            Config = roomConfig;
            return this;
        }


        private void Start()
        {
            State.State(RoomStates.Close)
                .OnEnter(() =>
                {
                    if (Config.RoomType == RoomTypes.Init)
                    {
                        foreach (var door in mDoors)
                        {
                            door.State.ChangeState(Door.States.IdleOpen);
                        }

                        State.ChangeState(RoomStates.Unlocked);


                    }

                });



            State.State(RoomStates.PlayerIn)
                .OnEnter(() =>
                {
                    if (Config.RoomType == RoomTypes.Normal)
                    {
                        //填充Waves
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

                            while (targetScore > 0 && waveConfig.EnemyNames.Count < mEnemyGeneratePoses.Count)
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

                    if (Config.RoomType == RoomTypes.Final)
                    {
                        var boss = EnemyFactory.Default.BossF/*EnemyByName(Global.BossList.GetAndRemoveRandomItem())*/
                        .GameObject
                        .Position2D(mEnemyGeneratePoses.GetRandomItem())
                        .Show()
                        .GetComponent<IEnemy>();

                        boss.Room = this;
                        this.Enemies.Add(boss);

                        foreach (var door in mDoors)
                        {
                            door.State.ChangeState(Door.States.BattleClose);
                        }
                    }

                    if (Config.RoomType == RoomTypes.Shop)
                    {
                        var takeCount = Random.Range(4, 6 + 1);
                        var normalShopItem = ShopSystem.CalculateNormalShopItems();

                        // 生成美观位置 ---------------------------
                        var positions = GenerateBeautifulPositions(takeCount);

                        var cenPos = transform.position;
                        // 选择上述任一方案实现这个方法

                        LevelController.Default.Booth.Instantiate()
                        .Position2D(cenPos)
                        .Show();

                        foreach (var pos in positions)
                        {
                            var item = normalShopItem.GetRandomItem();
                            LevelController.Default.ShopItem.Instantiate()
                                 .Position2D(pos)
                                .Self(self =>
                                {
                                    self.Room = this;
                                    self.ItemPrice = item.Item2;
                                    self.PowerUp = item.Item1;
                                    self.UpdateView();

                                }) .Show();
                        }

                        State.ChangeState(RoomStates.Unlocked);
                    }

                    // 位置生成方法示例（环形方案）
                    List<Vector2> GenerateBeautifulPositions(int count)
                    {
                        var positions = new List<Vector2>();
                        Vector2 center = transform.position;
                        float radius = 4f;

                        for (int i = 0; i < count; i++)
                        {
                            float angle = i * Mathf.PI * 2 / count;
                            positions.Add(center + new Vector2(
                                Mathf.Cos(angle) * radius,
                                Mathf.Sin(angle) * radius
                            ));
                        }

                        // 添加整体随机旋转
                        float randomRot = Random.Range(0, 360f);
                        return positions.Select(p => RotatePoint(p, center, randomRot)).ToList();
                    }

                    // 辅助方法：旋转点
                    Vector2 RotatePoint(Vector2 point, Vector2 center, float degrees)
                    {
                        Vector2 dir = point - center;
                        dir = Quaternion.Euler(0, 0, degrees) * dir;
                        return center + dir;
                    }

                    //if (Config.RoomType == RoomTypes.Shop)
                    //{
                    //    var takeCount = 8;/*Random.Range(2, 5 + 1);*/
                    //    var normalShopItem = ShopSystem.CalculateNormalShopItems();

                    //    for (int i = 0; i < takeCount; i++)
                    //    {
                    //        var item = normalShopItem.GetRandomItem();
                    //        var pos = mShopItemGeneratePoses.GetAndRemoveRandomItem();

                    //        LevelController.Default.ShopItem.Instantiate()
                    //            .Position2D(pos)
                    //            .Self(self =>
                    //            {
                    //                self.Room = this;
                    //                self.ItemPrice = item.Item2;
                    //                self.PowerUp = item.Item1;
                    //                self.UpdateView();

                    //            })
                    //            .Show();
                    //    }

                    //    //必须生成一个钥匙
                    //    var key = normalShopItem.First(i =>
                    //    i.Item1.SpriteRenderer == PowerUpFactory.Default.Key.SpriteRenderer);
                    //    LevelController.Default.ShopItem.Instantiate()
                    //            .Position2D(mShopItemGeneratePoses.GetAndRemoveRandomItem())
                    //    .Self(self =>
                    //    {
                    //        self.Room = this;
                    //        self.ItemPrice = key.Item2;
                    //        self.PowerUp = key.Item1;
                    //        self.UpdateView();

                    //    })
                    //    .Show();

                    //    State.ChangeState(RoomStates.Unlocked);
                    //}

                    if (Config.RoomType == RoomTypes.Chest)
                    {
                        State.ChangeState(RoomStates.Unlocked);
                    }

                    if(Config.RoomType == RoomTypes.Next)
                    {
                        State.ChangeState(RoomStates.Unlocked);
                    }

                })
                .OnUpdate(() =>
                {
                    if (Time.frameCount % 30 == 0)
                    {

                        if (mEnemies.Count == 0)//敌人全部死亡
                        {


                            if (mWaves.Count > 0)//有剩余波次继续生成敌人
                            {
                                var wave = mWaves.First();
                                mWaves.RemoveAt(0);
                                GenerateEnemies(wave);
                            }
                            else//没有波次 开门
                            {
                                if (Config.RoomType == RoomTypes.Normal)
                                {
                                    foreach (var powerUp in PowerUps.Where(p => p.GetType() == typeof(Coin)))
                                    {
                                        var cashedPowerUp = powerUp;
                                        ActionKit.OnFixedUpdate.Register(() =>
                                        {
                                            //金币朝玩家飞行
                                            cashedPowerUp.SpriteRenderer.transform.Translate(
                                                cashedPowerUp.SpriteRenderer.NormalizedDirection2DTo(Player.Default) *
                                                Time.fixedDeltaTime * 10);
                                        }).UnRegisterWhenGameObjectDestroyed(cashedPowerUp.SpriteRenderer.gameObject);
                                    }



                                    State.ChangeState(RoomStates.Unlocked);

                                    foreach (var door in mDoors)
                                    {
                                        door.State.ChangeState(Door.States.Open);
                                    }

                                }
                                else if (Config.RoomType == RoomTypes.Final)
                                {
                                    State.ChangeState(RoomStates.Unlocked);

                                    foreach (var door in mDoors)
                                    {
                                        door.State.ChangeState(Door.States.Open);
                                    }
                                }
                            }


                        }
                    }
                });



            State.State(RoomStates.Unlocked);
            State.StartState(RoomStates.Close);
           
        }

        private void Update()
        {
            State.Update();
        }


		public void AddEnemyGeneratePos(Vector3 enemyGeneratePos)
		{
			mEnemyGeneratePoses.Add(enemyGeneratePos);

        }

        public void AddShopItemGeneratePos(Vector3 shopItemGeneratePos)
        {
            mShopItemGeneratePoses.Add(shopItemGeneratePos);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Global.CurrentRoom = this;  
                OnRoomEnter.Trigger(this);

                if (State.CurrentStateId == RoomStates.Close)
                {
                    State.ChangeState(RoomStates.PlayerIn);

                }

                
            }
        }

        //生成敌人方法
        void GenerateEnemies(EnemyWaveConfig waveConfig)
        {
            var pos2Gen = mEnemyGeneratePoses
                .OrderByDescending(p => (Player.Default.Position2D() - p.ToVector2()).magnitude)
                .ToList();//在mEnemyGeneratePoses敌人位置列表中，选取离玩家位置最远的enemyCount个位置的敌人

            foreach (var enemyName in waveConfig.EnemyNames)
            {
                //根据名字生成敌人
                var enemy =EnemyFactory.EnemyByName(enemyName)
                    .GameObject.Instantiate()
                    .Position2D(pos2Gen.GetAndRemoveRandomItem())
                    .Show()
                    .GetComponent<IEnemy>();

                enemy.Room = this;
                mEnemies.Add(enemy);//每一个敌人都记录下来
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
