using QFramework.ProjectGungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelController : MonoBehaviour
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

    /*
    1 �ؿ�
    @ ����
    e ����
    # �յ�
    */

    //��ͼ���л�����ʼ����
    public List<string> InitRoom { get; set; } = new List<string>()
    {
        "1111111111111111111",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                  ",
        "1         @        ",
        "1                  ",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1111111111111111111",
    };
    
    //��ͨ���˷���
    public List<string> NormalRoom { get; set; } = new List<string>()
    {
        "1111111111111111111",
        "1                 1",
        "1  e           e  1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1        e        1",
        "                   ",
        "       e 1 e       ",
        "                   ",
        "1        e        1", 
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1  e           e  1",
        "1                 1",
        "1111111111111111111",
    };

    //���շ���
    public List<string> FinalRoom { get; set; } = new List<string>()
    {
        "1111111111111111111",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "                  1",
        "         #        1",
        "                  1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1                 1",
        "1111111111111111111",
    };
    private void Awake()
    {
        Player.gameObject.SetActive(false);
        Enemy.gameObject.SetActive(false);
    }



    void Start()
    {

        var currentRoomStartPosx = 0;//��¼��ǰ������ʼλ��
        GenerateRoom(currentRoomStartPosx, InitRoom);
        currentRoomStartPosx += InitRoom.First().Length + 2;//���Ϸ����ȣ���Ϊ��һ���������ʼλ��
        GenerateRoom(currentRoomStartPosx, NormalRoom);
        currentRoomStartPosx += InitRoom.First().Length + 2;//���Ϸ����ȣ���Ϊ��һ���������ʼλ��
        GenerateRoom(currentRoomStartPosx, FinalRoom); 

    }

    void GenerateRoom(int startPosX,List<string> roomCode)
    {
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
                    var enemy = Instantiate(Enemy);
                    enemy.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    enemy.gameObject.SetActive(true);
                }
                else if (code == '#')
                {
                    var final = Instantiate(Final);
                    final.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    final.gameObject.SetActive(true);
                }
            }

        }
    }

    void Update()
    {
        
    }
}
