using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static QFramework.ProjectGungeon.RoomConfig;

namespace QFramework.ProjectGungeon
{

    public partial class LevelController : ViewController
    {
        public TileBase Wall0;
        public TileBase Wall1;
        public TileBase Wall2;
        public TileBase Wall3;

        public TileBase wall
        {
            get
            {
                var wallIndex = Random.Range(0, 3 + 1);

                if (wallIndex == 0)
                    return Wall0;
                if (wallIndex == 1)
                    return Wall1;
                if (wallIndex == 2)
                    return Wall2;
                if (wallIndex == 3)
                    return Wall3;

                return Wall0;

            }
        }

        public TileBase Floor0;
        public TileBase Floor1;
        public TileBase Floor2;
        public TileBase Floor3;

        public TileBase Floor
        {
            get
            {
                if(Global.CurrentLevel == Level1.Config)
                {
                    return RandomUtility.Choose(
                        Floor0,
                        Floor0,
                        Floor0,
                        Floor0,
                        Floor0,
                        Floor0,
                        Floor0,
                        Floor1,
                        Floor1,
                        Floor2,
                        Floor2,
                        Floor3);

                }

                if (Global.CurrentLevel == Level2.Config)
                {
                    return RandomUtility.Choose(
                        Floor10,
                        Floor10,
                        Floor10,
                        Floor10,
                        Floor10,
                        Floor10,
                        Floor10,
                        Floor11,
                        Floor11,
                        Floor12,
                        Floor12,
                        Floor13);

                }

                if (Global.CurrentLevel == Level3.Config)
                {
                    return RandomUtility.Choose(
                        Floor20,
                        Floor20,
                        Floor20,
                        Floor20,
                        Floor20,
                        Floor20,
                        Floor20,
                        Floor21,
                        Floor21,
                        Floor22,
                        Floor22,
                        Floor23);

                }

                if (Global.CurrentLevel == Level4.Config)
                {
                    return RandomUtility.Choose(
                        Floor30,
                        Floor30,
                        Floor30,
                        Floor30,
                        Floor30,
                        Floor30,
                        Floor30,
                        Floor31,
                        Floor31,
                        Floor32,
                        Floor32,
                        Floor33);

                }

                return Wall0;

            }
        }


        public Tilemap WallTilemap;
        public Tilemap FloorTilemap;


        public Player Player;
        public IEnemy Enemy => RandomUtility.Choose(EnemyA, EnemyB, EnemyC).GetComponent<IEnemy>();

        public Final Final;



        public static LevelController Default;

        private void Awake()
        {
            Player.gameObject.SetActive(false);
            Enemy.GameObject.SetActive(false);

            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;
        }

        public class RoomGenerateNode
        {
            public int X { get; set; }
            public int Y { get; set; }

            public HashSet<DoorDirections> Directions { get; set; }

            public RoomNode Node;
            public RoomConfig Config { get; set; }
        }

        public enum DoorDirections
        {
            Up,
            Down,
            Left,
            Right
        
        }

        void Start()
        {
            Room.Hide();


            var layout = Global.CurrentLevel.InitRoom;

            var layoutGrid = new DynaGrid<RoomGenerateNode>();


            bool GenerateLayoutBFS(RoomNode roomNode, DynaGrid<RoomGenerateNode> layoutGrid, int predictWeight = 0)
            {
                //广度优先遍历
                var queue = new Queue<RoomGenerateNode>();
                queue.Enqueue(new RoomGenerateNode()
                {
                    X = 0,
                    Y = 0,
                    Node = roomNode,
                    Directions = new HashSet<DoorDirections>(),
                    Config = SharedRooms.InitRoom,

                }); 

                while (queue.Count > 0)
                {
                    var generateNode = queue.Dequeue();

                    if (layoutGrid[generateNode.X, generateNode.Y] != null)
                    {
                        Debug.Log("冲突了");
                        return false;
                    }
                    else
                    {
                        layoutGrid[generateNode.X, generateNode.Y] = generateNode;
                    }

                    //获取扩散的方向

                    var availabelDirections = 
                        LevelGenHelper.GetAvailableDirections(layoutGrid, generateNode.X, generateNode.Y);
                    

                    if(generateNode.Node.Children.Count > availabelDirections.Count)
                    {
                        Debug.Log("房间生成冲突");
                        return false;
                    }

                    var directions = LevelGenHelper.Predict(layoutGrid, generateNode.X, generateNode.Y);
                    //大到小排序
                    directions.Sort((a, b) =>{ return b.Count - a.Count; });

                    var predictGenerate = false;

                    if(Random.Range(0, 100) < predictWeight)
                    {
                        //进行最优解生成
                        predictGenerate = true;
                    }
                    else
                    {
                        //随机选择扩散方向
                        predictGenerate = false;
                    }


                    foreach(var roomNodeChild in generateNode.Node.Children)
                    {
                        //随机选择一个扩散方向
                        var nextRoomDirection = predictGenerate 
                            ? directions.First().Direction : availabelDirections.GetAndRemoveRandomItem();
                        if (predictGenerate)
                        {
                            directions.RemoveAt(0);
                        }

                        //生成房间
                        if (nextRoomDirection == DoorDirections.Right)
                        {
                            generateNode.Directions.Add(DoorDirections.Right);
                            queue.Enqueue(new RoomGenerateNode()
                            {
                                X = generateNode.X + 1,
                                Y = generateNode.Y,
                                Node = roomNodeChild,
                                Config = RoomConfigByType(roomNodeChild.RoomType),
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Left,
                                }
                            });
                        }
                        else if (nextRoomDirection == DoorDirections.Left)
                        {
                            generateNode.Directions.Add(DoorDirections.Left);
                            queue.Enqueue(new RoomGenerateNode()
                            {
                                X = generateNode.X - 1,
                                Y = generateNode.Y,
                                Node = roomNodeChild,
                                Config = RoomConfigByType(roomNodeChild.RoomType),
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Right,
                                }
                            });
                        }
                        else if (nextRoomDirection == DoorDirections.Up)
                        {
                            generateNode.Directions.Add(DoorDirections.Up);
                            queue.Enqueue(new RoomGenerateNode()
                            {
                                X = generateNode.X,
                                Y = generateNode.Y + 1,
                                Node = roomNodeChild,
                                Config = RoomConfigByType(roomNodeChild.RoomType),
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Down,
                                }
                            });
                        }
                        else if (nextRoomDirection == DoorDirections.Down)
                        {
                            generateNode.Directions.Add(DoorDirections.Down);
                            queue.Enqueue(new RoomGenerateNode()
                            {
                                X = generateNode.X,
                                Y = generateNode.Y - 1,
                                Node = roomNodeChild,
                                Config = RoomConfigByType(roomNodeChild.RoomType),
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Up,
                                }
                            });
                        }
                    }
                    
                }

                return true;

            }

            var predictWeight = 0;

            
            while(!GenerateLayoutBFS(layout, layoutGrid, predictWeight))
            {  
                print("重新生成");
                predictWeight++;
                
                layoutGrid.Clear();
            }

           

            Global.RoomGrid = new DynaGrid<Room>();

            //解决不同尺寸房间中心点位置问题
            var maxRoomWidth = 0;
            var maxRoomHeigth = 0;
            layoutGrid.ForEach(d =>
            {
                if(maxRoomWidth < d.Config.Width)
                {
                    maxRoomWidth = d.Config.Width;
                }

                if (maxRoomHeigth < d.Config.Height)
                {
                    maxRoomHeigth = d.Config.Height;
                }
            });


            layoutGrid.ForEach((x, y, generateNode) =>
            {
                var room = GenerateRoomByNode(x, y, generateNode);
                Global.RoomGrid[x, y] = room;

            });

            var currentRoomStartPosx = 0;//记录当前房间起始位置

            
            
            Room GenerateRoomByNode(int x, int y, RoomGenerateNode node)
            {
                var roomPosX = x * (maxRoomWidth + 2);
                var roomPosY = y * (maxRoomHeigth + 2);
                return GenerateRoom(roomPosX, roomPosY, node);
            }

            void GenerateCorridor()
            {
                Global.RoomGrid.ForEach((x, y, room) =>
                {
                    foreach(var door in room.Doors)
                    {
                        if (door.Direction == DoorDirections.Left)
                        {   
                            var dstRoom = Global.RoomGrid[x - 1, y];
                            var dstDoor = dstRoom.Doors.First(d => d.Direction == DoorDirections.Right);

                            for(int i = door.X; i <= dstDoor.X; i++)
                            {
                                FloorTilemap.SetTile(new Vector3Int(i, door.Y, 0), Floor);
                                FloorTilemap.SetTile(new Vector3Int(i, door.Y + 1, 0), Floor);
                                FloorTilemap.SetTile(new Vector3Int(i, door.Y - 1, 0), Floor);
                                WallTilemap.SetTile(new Vector3Int(i, door.Y + 2, 0), wall);
                                WallTilemap.SetTile(new Vector3Int(i, door.Y - 2, 0), wall);
                            }
                         
                        }
                        else if (door.Direction == DoorDirections.Right)
                        {
                            var dstRoom = Global.RoomGrid[x + 1, y];
                            var dstDoor = dstRoom.Doors.First(d => d.Direction == DoorDirections.Left);

                            for (int i = door.X; i <= dstDoor.X; i++)
                            {
                                FloorTilemap.SetTile(new Vector3Int(i, door.Y, 0), Floor);
                                FloorTilemap.SetTile(new Vector3Int(i, door.Y + 1, 0), Floor);
                                FloorTilemap.SetTile(new Vector3Int(i, door.Y - 1, 0), Floor);
                                WallTilemap.SetTile(new Vector3Int(i, door.Y + 2, 0), wall);
                                WallTilemap.SetTile(new Vector3Int(i, door.Y - 2, 0), wall);
                            }
                        }
                        else if (door.Direction == DoorDirections.Up)
                        {
                            var dstRoom = Global.RoomGrid[x, y + 1];
                            var dstDoor = dstRoom.Doors.First(d => d.Direction == DoorDirections.Down);

                            for (int i = door.Y; i <= dstDoor.Y; i++)
                            {
                                FloorTilemap.SetTile(new Vector3Int(door.X, i, 0), Floor);
                                FloorTilemap.SetTile(new Vector3Int(door.X + 1, i, 0), Floor);
                                FloorTilemap.SetTile(new Vector3Int(door.X - 1, i, 0), Floor);
                                WallTilemap.SetTile(new Vector3Int(door.X + 2, i, 0), wall);
                                WallTilemap.SetTile(new Vector3Int(door.X - 2, i, 0), wall);
                            }
                        }
                        else if (door.Direction == DoorDirections.Down)
                        {
                            var dstRoom = Global.RoomGrid[x, y - 1];
                            var dstDoor = dstRoom.Doors.First(d => d.Direction == DoorDirections.Up);

                            for (int i = door.Y; i <= dstDoor.Y; i++)
                            {
                                FloorTilemap.SetTile(new Vector3Int(door.X, i, 0), Floor);
                                FloorTilemap.SetTile(new Vector3Int(door.X + 1, i, 0), Floor);
                                FloorTilemap.SetTile(new Vector3Int(door.X - 1, i, 0), Floor);
                                WallTilemap.SetTile(new Vector3Int(door.X + 2, i, 0), wall);
                                WallTilemap.SetTile(new Vector3Int(door.X - 2, i, 0), wall);
                            }
                        }
                    }

                });

            }

            GenerateCorridor();







        }

        Room GenerateRoom(int centerCellPosX, int centerCellPosY, RoomGenerateNode node)
        {
            var roomCode = node.Config.Codes;
             
            //房间宽高
            var roomWidth = node.Config.Width;
            var roomHeight = node.Config.Height;

            var startCellPosX = centerCellPosX - roomWidth / 2;
            var startCellPosY = centerCellPosY - roomHeight / 2;
            var startPosX = startCellPosX + 0.5f;
            var startPosY = startCellPosY + 0.5f;
            //房间中心点
            var roomPosX = centerCellPosX + 0.5f;
            var roomPosY = centerCellPosY + 0.5f;

            //生成房间 
            var room = Room.InstantiateWithParent(this)
                .WithConfig(node.Config)
                .Position(roomPosX, roomPosY)
                .Show();

            room.GenerateNode = node;

            room.SelfBoxCollider2D.size = new Vector2(roomWidth - 5, roomHeight - 5);//Collider2D适配房间大小



            //字符串生成地图
            for (var i = 0; i < roomCode.Count; i++)
            {
                var rowCode = roomCode[i];
                for (int j = 0; j < rowCode.Length; j++)
                {
                    var code = rowCode[j];

                    var x = startCellPosX + j;
                    var y = startCellPosY + roomCode.Count - i;

                    FloorTilemap.SetTile(new Vector3Int(x, y, 0), Floor);//绘制地面

                    if (code == '1')
                    {
                        WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//绘制墙壁

                    }
                    else if (code == '@')
                    {
                        var player = Instantiate(Player);
                        player.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                        player.gameObject.SetActive(true);

                        //摄像机赋给主角
                        Global.Player = player;
                    }
                    else if (code == 'e')
                    {
                        var enemyGeneratePos = new Vector3(x + 0.5f, y + 0.5f, 0);
                        room.AddEnemyGeneratePos(enemyGeneratePos); 
                        
                    }
                    else if (code == '#')
                    {
                        var final = Instantiate(Final);
                        final.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                        final.Show();
                    }
                    else if (code == 'd')
                    {
                 
                        var doorDistance = new Vector2(x + 0.5f, y + 0.5f) - new Vector2(roomPosX, roomPosY);
                        var cachedX = x;
                        var cachedY = y;
                        if (doorDistance.x.Abs() > doorDistance.y.Abs())//不是在左边就是在右边
                        {
                            if (doorDistance.x > 0)//在右边
                            {

                                if (node.Directions.Contains(DoorDirections.Right))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                        .LocalScaleX(3)
                                        .Position2D(new Vector3(x, y, 0))
                                        .Show();
                                    door.X = x;
                                    door.Y = y;
                                    door.Direction = DoorDirections.Right;
                                    door.LocalRotation(Quaternion.Euler(0, 0, 90));
                                    room.AddDoor(door);
                                  

                                    //等待一帧再打开
                                    ActionKit.NextFrame(() =>
                                    {
                                        WallTilemap.SetTile(new Vector3Int(cachedX, cachedY + 1, 0), null);
                                        WallTilemap.SetTile(new Vector3Int(cachedX, cachedY - 1, 0), null);
                                    }).Start(this);

                                   
                                }
                                else
                                {
                                    WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//绘制墙壁
                                }
                            }
                            else
                            {
                                if (node.Directions.Contains(DoorDirections.Left))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                        .LocalScaleX(3)
                                        .Position2D(new Vector3(x, y, 0))
                                        .Show();
                                    door.X = x;
                                    door.Y = y;
                                    door.Direction = DoorDirections.Left;
                                    door.LocalRotation(Quaternion.Euler(0, 0, 90));
                                    room.AddDoor(door);
                                    ActionKit.NextFrame(() =>
                                    {
                                        WallTilemap.SetTile(new Vector3Int(cachedX, cachedY + 1, 0), null);
                                        WallTilemap.SetTile(new Vector3Int(cachedX, cachedY - 1, 0), null);
                                    }).Start(this);
                                }
                                else
                                {
                                    WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//绘制墙壁
                                }
                            }

                        }
                        else
                        {
                            if (doorDistance.y > 0)//在右边
                            {
                                if (node.Directions.Contains(DoorDirections.Up))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                        .LocalScaleX(3)
                                        .Position2D(new Vector3(x, y, 0))
                                        .Show();
                                    door.X = x;
                                    door.Y = y;
                                    door.Direction = DoorDirections.Up;
                                    room.AddDoor(door);
                                    ActionKit.NextFrame(() =>
                                    {
                                        WallTilemap.SetTile(new Vector3Int(cachedX + 1, cachedY, 0), null);
                                        WallTilemap.SetTile(new Vector3Int(cachedX - 1, cachedY, 0), null);
                                    }).Start(this);
                                }
                                else
                                {
                                    WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//绘制墙壁
                                }
                            }
                            else
                            {
                                if (node.Directions.Contains(DoorDirections.Down))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                        .LocalScaleX(3)
                                        .Position2D(new Vector3(x, y, 0))
                                        .Show();
                                    door.X = x;
                                    door.Y = y;
                                    door.Direction = DoorDirections.Down;
                                    room.AddDoor(door); 
                                    ActionKit.NextFrame(() =>
                                    {
                                        WallTilemap.SetTile(new Vector3Int(cachedX + 1, cachedY, 0), null);
                                        WallTilemap.SetTile(new Vector3Int(cachedX - 1, cachedY, 0), null);
                                    }).Start(this);
                                }
                                else
                                {
                                    WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//绘制墙壁
                                }
                            }

                        }
                        
                    }
                    else if (code == 'c')
                    {
                       var chest = Chest.InstantiateWithParent(room)
                       .Position2D(new Vector3(x, y, 0))
                       .Show();

                        room.AddPowerUp(chest);
                    }
                    else if (code == 's')
                    {
                        room.AddShopItemGeneratePos(new Vector3(x, y, 0));

                    }
                    else if (code == 'y')
                    {
                        var color = PowerUpFactory.Default.PowerUpColor.Instantiate()
                            .Position2D(new Vector2(x, y))
                            .Show();

                        room.AddPowerUp(color);

                    }
                    else if (code == 'g')
                    {
                        GunBase.Instantiate()
                            .Position2D(new Vector2(x, y))
                            .Show();
                    }
                    else if (code == 'b')
                    {
                        var box = Box.InstantiateWithParent(room)
                        .Position2D(new Vector3(x, y, 0))
                        .Show();

                        room.AddPowerUp(box);
                    }


                }

            }

            room.LB = new Vector3Int(startCellPosX, startCellPosY, 0);
            room.RT = new Vector3Int(startCellPosX + roomWidth, startCellPosY + roomHeight, 0);
            room.PrepareAStarNodes();
            return room;
        }

        public RoomConfig RoomConfigByType(RoomTypes roomType)
        {
            if (roomType == RoomTypes.Init)
            {
                return SharedRooms.InitRoom;
            }
            else if (roomType == RoomTypes.Normal)
            {
                return Global.CurrentLevel.NormalRoomTemplates.GetRandomItem();
            }
            else if (roomType == RoomTypes.Chest)
            {
                return SharedRooms.ChestRoom;

            }
            else if (roomType == RoomTypes.Shop)
            {
                return SharedRooms.ShopRoom;

            }
            else if (roomType == RoomTypes.Final)
            {
                return SharedRooms.FinalRooms.GetRandomItem();
            }
            else if (roomType == RoomTypes.Next)
            {
                return SharedRooms.NextRoom;
            }

            return null;
        }
    }
}
