using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

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

            var currentRoomStartPosx = 0;//��¼��ǰ������ʼλ��
            GenerateRoom(currentRoomStartPosx, Config.InitRoom);
            currentRoomStartPosx += Config.InitRoom.Codes.First().Length + 2;//���Ϸ����ȣ���Ϊ��һ���������ʼλ��
            GenerateRoom(currentRoomStartPosx, Config.NormalRoom);
            currentRoomStartPosx += Config.InitRoom.Codes.First().Length + 2;//���Ϸ����ȣ���Ϊ��һ���������ʼλ��
            GenerateRoom(currentRoomStartPosx, Config.FinalRoom);

        }

        void GenerateRoom(int startPosX, RoomConfig roomConfig)
        {
            var roomCode = roomConfig.Codes;

            //������
            var roomWidth = roomCode[0].Length;
            var roomHeight = roomCode.Count;

            //�������ĵ�
            var roomPosX = startPosX + roomWidth * 0.5f;
            var roomPosY = 0.5f + roomHeight * 0.5f;

            //���ɷ���
            var room = Room.InstantiateWithParent(this)
                .WithConfig(roomConfig)
                .Position(roomPosX, roomPosY)
                .Show();

            room.SelfBoxCollider2D.size = new Vector2(roomWidth - 2, roomHeight - 2);//Collider2D���䷿���С



            //�ַ������ɵ�ͼ
            for (var i = 0; i < roomCode.Count; i++)
            {
                var rowCode = roomCode[i];
                for (int j = 0; j < rowCode.Length; j++)
                {
                    var code = rowCode[j];

                    var x = startPosX + j;
                    var y = roomCode.Count - i;

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
                        var door = Door.InstantiateWithParent(room)
                            .Position2D(new Vector3(x, y, 0))
                            .Hide();

                        room.AddDoor(door);
                    }
                }

            }
        }

        void Update()
        {

        }
    }
}
