using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                var wallIndex = Random.Range(0, 3 + 1);

                if (wallIndex == 0)
                    return Floor0;
                if (wallIndex == 1)
                    return Floor1;
                if (wallIndex == 2)
                    return Floor2;
                if (wallIndex == 3)
                    return Floor3;

                return Floor0;

            }
        }


        public Tilemap WallTilemap;
        public Tilemap FloorTilemap;


        public Player Player;
        public Enemy Enemy;

        public Final Final;



        public static LevelController Default;

        private void Awake()
        {
            Player.gameObject.SetActive(false);
            Enemy.gameObject.SetActive(false);

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


            var layout = new RoomNode(RoomTypes.Init);
                layout.Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Chest)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Normal)
                .Next(RoomTypes.Final);

            var layoutGrid = new DynaGrid<RoomGenerateNode>();


            void GenerateLayoutBFS(RoomNode roomNode, DynaGrid<RoomGenerateNode> layoutGrid)
            {
                //������ȱ���
                var queue = new Queue<RoomGenerateNode>();
                queue.Enqueue(new RoomGenerateNode()
                {
                    X = 0,
                    Y = 0,
                    Node = roomNode,
                    Directions = new HashSet<DoorDirections>(),

                });

                while (queue.Count > 0)
                {
                    var generateNode = queue.Dequeue();

                    layoutGrid[generateNode.X, generateNode.Y] = generateNode;

                    //��ȡ��ɢ�ķ���

                    var availabelDirections = new List<DoorDirections>();
                    if (layoutGrid[generateNode.X + 1, generateNode.Y] == null)
                    {
                        availabelDirections.Add(DoorDirections.Right);
                    }
                    if (layoutGrid[generateNode.X - 1, generateNode.Y] == null)
                    {
                        availabelDirections.Add(DoorDirections.Left);
                    }
                    if (layoutGrid[generateNode.X, generateNode.Y + 1] == null)
                    {
                        availabelDirections.Add(DoorDirections.Up);
                    }
                    if (layoutGrid[generateNode.X, generateNode.Y - 1] == null)
                    {
                        availabelDirections.Add(DoorDirections.Down);
                    }

                    foreach(var roomNodeChild in generateNode.Node.Children)
                    {
                        //���ѡ��һ����ɢ����
                        var nextRoomDirection = availabelDirections.GetRandomItem();

                        //���ɷ���
                        if (nextRoomDirection == DoorDirections.Right)
                        {
                            generateNode.Directions.Add(DoorDirections.Right);
                            queue.Enqueue(new RoomGenerateNode()
                            {
                                X = generateNode.X + 1,
                                Y = generateNode.Y,
                                Node = roomNodeChild,
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
                                Directions = new HashSet<DoorDirections>()
                                {
                                    DoorDirections.Up,
                                }
                            });
                        }
                    }
                    
                }
            }

            GenerateLayoutBFS(layout, layoutGrid);

            layoutGrid.ForEach((x, y, generateNode) =>
            {
                GenerateRoomByNode(x, y, generateNode);

            });

            var currentRoomStartPosx = 0;//��¼��ǰ������ʼλ��

            
            void GenerateRoomByNode(int x, int y, RoomGenerateNode node)
            {
                var roomPosX = x * (Config.InitRoom.Codes.First().Length + 2);
                var roomPosY = y * (Config.InitRoom.Codes.Count + 2); 
                if (node.Node.RoomType == RoomTypes.Init)
                {
                    GenerateRoom(roomPosX, roomPosY, Config.InitRoom, node);
                }
                else if (node.Node.RoomType == RoomTypes.Normal)
                {
                    GenerateRoom(roomPosX, roomPosY, Config.NormalRooms.GetRandomItem(), node);
                }
                else if (node.Node.RoomType == RoomTypes.Chest)
                {
                    GenerateRoom(roomPosX, roomPosY, Config.ChestRoom, node);

                }
                else if (node.Node.RoomType == RoomTypes.Final)
                {
                    GenerateRoom(roomPosX, roomPosY, Config.FinalRoom, node);
                }

            }

            void GenerateCorridor(int roomCount)
            {
                //������
                var roomWidth = Config.InitRoom.Codes.First().Length;
                var roomHeight = Config.InitRoom.Codes.Count;

                for (int index = 0; index < roomCount - 1; index++)//�����ĸ�������ͨ�Ĵ�ש
                {
                    currentRoomStartPosx = index * (roomWidth + 2);
                    var doorStartPosX = currentRoomStartPosx + roomWidth - 1;
                    var doorStartPosY = 0 + roomHeight / 2 + 1;
                    for (int i = 0; i < 2; i++)
                    {
                        FloorTilemap.SetTile(new Vector3Int(doorStartPosX + i + 1, doorStartPosY, 0), Floor);
                        FloorTilemap.SetTile(new Vector3Int(doorStartPosX + i + 1, doorStartPosY + 1, 0), Floor);
                        FloorTilemap.SetTile(new Vector3Int(doorStartPosX + i + 1, doorStartPosY - 1, 0), Floor);
                        WallTilemap.SetTile(new Vector3Int(doorStartPosX + i + 1, doorStartPosY + 2, 0), wall);
                        WallTilemap.SetTile(new Vector3Int(doorStartPosX + i + 1, doorStartPosY - 2, 0), wall);

                    }
                }
            }

            //GenerateCorridor(7);


          




        }

        void GenerateRoom(int startPosX, int startPosY, RoomConfig roomConfig, RoomGenerateNode node)
        {
            var roomCode = roomConfig.Codes;

            //������
            var roomWidth = roomCode[0].Length;
            var roomHeight = roomCode.Count;

            //�������ĵ�
            var roomPosX = startPosX + roomWidth * 0.5f;
            var roomPosY = startPosY + 0.5f + roomHeight * 0.5f;

            //���ɷ��� 
            var room = Room.InstantiateWithParent(this)
                .WithConfig(roomConfig)
                .Position(roomPosX, roomPosY)
                .Show();

            room.GenerateNode = node;

            room.SelfBoxCollider2D.size = new Vector2(roomWidth - 2, roomHeight - 2);//Collider2D���䷿���С



            //�ַ������ɵ�ͼ
            for (var i = 0; i < roomCode.Count; i++)
            {
                var rowCode = roomCode[i];
                for (int j = 0; j < rowCode.Length; j++)
                {
                    var code = rowCode[j];

                    var x = startPosX + j;
                    var y = startPosY + roomCode.Count - i;

                    FloorTilemap.SetTile(new Vector3Int(x, y, 0), Floor);//���Ƶ���

                    if (code == '1')
                    {
                        WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//����ǽ��

                    }
                    else if (code == '@')
                    {
                        var player = Instantiate(Player);
                        player.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                        player.gameObject.SetActive(true);

                        //�������������
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
                        final.gameObject.SetActive(true);
                    }
                    else if (code == 'd')
                    {
                        if (node.Directions.Contains(DoorDirections.Right))
                        {

                        }
                        var doorDistance = new Vector2(x + 0.5f, y + 0.5f) - new Vector2(roomPosX, roomPosY);

                        if (doorDistance.x.Abs() > doorDistance.y.Abs())//��������߾������ұ�
                        {
                            if (doorDistance.x > 0)//���ұ�
                            {
                                if (node.Directions.Contains(DoorDirections.Right))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                                        .Position2D(new Vector3(x, y, 0))
                                                        .Hide();

                                    room.AddDoor(door);
                                }
                                else
                                {
                                    WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//����ǽ��
                                }
                            }
                            else
                            {
                                if (node.Directions.Contains(DoorDirections.Left))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                                        .Position2D(new Vector3(x, y, 0))
                                                        .Hide();

                                    room.AddDoor(door);
                                }
                                else
                                {
                                    WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//����ǽ��
                                }
                            }

                        }
                        else
                        {
                            if (doorDistance.y > 0)//���ұ�
                            {
                                if (node.Directions.Contains(DoorDirections.Up))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                                        .Position2D(new Vector3(x, y, 0))
                                                        .Hide();

                                    room.AddDoor(door);
                                }
                                else
                                {
                                    WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//����ǽ��
                                }
                            }
                            else
                            {
                                if (node.Directions.Contains(DoorDirections.Down))
                                {
                                    var door = Door.InstantiateWithParent(room)
                                                        .Position2D(new Vector3(x, y, 0))
                                                        .Hide();

                                    room.AddDoor(door);
                                }
                                else
                                {
                                    WallTilemap.SetTile(new Vector3Int(x, y, 0), wall);//����ǽ��
                                }
                            }
                        }




                    }
                    else if (code == 'c')
                    {
                        Chest.InstantiateWithParent(room)
                       .Position2D(new Vector3(x, y, 0))
                       .Show();

                    }
                }

            }
        }

        void Update()
        {

        }
    }
}
