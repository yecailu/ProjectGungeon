using System.Collections.Generic;

namespace QFramework.ProjectGungeon
{
    public enum RoomTypes
    {
        Init,
        Normal,
        Chest,
        Shop,
        Final,
        

    }

    public class EnemyWaveConfig
    {

    }

    public class Config
    {

        /*
        1 地块
        @ 主角
        e 敌人
        d 门
        c 宝箱
        s 商店摊位
        # 终点
        */

        //地图序列化，初始房间
        public static RoomConfig InitRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Init)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("d         @       d")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("111111111d111111111");


        //普通敌人房间
        public static List<RoomConfig> NormalRooms = new List<RoomConfig>()
        {
            new RoomConfig()
        .Type(RoomTypes.Normal)
        .L("1111111111111111111")
        .L("1                 1")
        .L("1  e           e  1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1        e        1")
        .L("1                 1")
        .L("d      e 1 e      d")
        .L("1                 1")
        .L("1        e        1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1  e           e  1")
        .L("1                 1")
        .L("111111111d111111111"),

             new RoomConfig()
        .Type(RoomTypes.Normal)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1  1           1  1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1    e       e    1")
        .L("1      e   e      1")
        .L("1        e        1")
        .L("d      e   e      d")
        .L("1        e        1")
        .L("1      e   e      1")
        .L("1    e       e    1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1  1           1  1")
        .L("1                 1")
        .L("111111111d111111111"),

             new RoomConfig()
        .Type(RoomTypes.Normal)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1  11         11  1")
        .L("1  1           1  1")
        .L("1    e       e    1")
        .L("1                 1")
        .L("1                 1")
        .L("1     e     e     1")
        .L("1                 1")
        .L("d        e        d")
        .L("1                 1")
        .L("1     e     e     1")
        .L("1                 1")
        .L("1                 1")
        .L("1    e       e    1")
        .L("1  1           1  1")
        .L("1  11         11  1")
        .L("1                 1")
        .L("111111111d111111111"),




    };

        //最终房间
        public static RoomConfig FinalRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Final)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("d        #        d")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("111111111d111111111");



        public static RoomConfig ChestRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Chest)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("d        c        d")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("111111111d111111111");

        public static RoomConfig ShopRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Shop)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("d        s        d")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("111111111d111111111");

    }


} 
