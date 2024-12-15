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

            var currentRoomStartPosx = 0;//记录当前房间起始位置

            
            void GenerateRoomByNode(RoomNode node)
            {
                if (node.RoomType == RoomTypes.Init)
                {
                    GenerateRoom(currentRoomStartPosx, Config.InitRoom);
                    currentRoomStartPosx += Config.InitRoom.Codes.First().Length + 2;//加上房间宽度，作为下一个房间的起始位置
                }
                else if (node.RoomType == RoomTypes.Normal)
                {
                    GenerateRoom(currentRoomStartPosx, Config.NormalRooms.GetRandomItem());
                    currentRoomStartPosx += Config.InitRoom.Codes.First().Length + 2;//加上房间宽度，作为下一个房间的起始位置
                }
                else if (node.RoomType == RoomTypes.Chest)
                {
                    GenerateRoom(currentRoomStartPosx, Config.ChestRoom);
                    currentRoomStartPosx += Config.InitRoom.Codes.First().Length + 2;//加上房间宽度，作为下一个房间的起始位置

                }
                else if (node.RoomType == RoomTypes.Final)
                {
                    GenerateRoom(currentRoomStartPosx, Config.FinalRoom);
                }

                foreach(var child in node.Children)
                {
                    GenerateRoomByNode(child);//递归
                }
            }

            void GenerateCorridor(int roomCount)
            {
                //房间宽高
                var roomWidth = Config.InitRoom.Codes.First().Length;
                var roomHeight = Config.InitRoom.Codes.Count;

                for (int index = 0; index < roomCount - 1; index++)//生成四个房间连通的瓷砖
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

            GenerateRoomByNode(layout);
            GenerateCorridor(7);


          




        }

        void GenerateRoom(int startPosX, RoomConfig roomConfig)
        {
            var roomCode = roomConfig.Codes;

            //房间宽高
            var roomWidth = roomCode[0].Length;
            var roomHeight = roomCode.Count;

            //房间中心点
            var roomPosX = startPosX + roomWidth * 0.5f;
            var roomPosY = 0.5f + roomHeight * 0.5f;

            //生成房间
            var room = Room.InstantiateWithParent(this)
                .WithConfig(roomConfig)
                .Position(roomPosX, roomPosY)
                .Show();

            room.SelfBoxCollider2D.size = new Vector2(roomWidth - 2, roomHeight - 2);//Collider2D适配房间大小



            //字符串生成地图
            for (var i = 0; i < roomCode.Count; i++)
            {
                var rowCode = roomCode[i];
                for (int j = 0; j < rowCode.Length; j++)
                {
                    var code = rowCode[j];

                    var x = startPosX + j;
                    var y = roomCode.Count - i;

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
                        final.gameObject.SetActive(true);
                    }
                    else if (code == 'd')
                    {
                        var door = Door.InstantiateWithParent(room)
                            .Position2D(new Vector3(x, y, 0))
                            .Hide();

                        room.AddDoor(door);
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
