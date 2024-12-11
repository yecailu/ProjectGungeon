using System.Collections.Generic;

namespace QFramework.ProjectGungeon
{
    public enum RoomTypes
    {
        Init,
        Normal,
        Final,

    }

    public class Config
    {

        /*
        1 地块
        @ 主角
        e 敌人
        d 门
        # 终点
        */

        //地图序列化，初始房间
        public static RoomConfig InitRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Init)
        .L("1111111111111111111")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 d")
        .L("1         @       d")
        .L("1                 d")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1111111111111111111");


        //普通敌人房间
        public static RoomConfig NormalRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Normal)
        .L("1111111111111111111")
        .L("1                 1")
        .L("1  e           e  1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1        e        1")
        .L("d                 d")
        .L("d      e 1 e      d")
        .L("d                 d")
        .L("1        e        1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1  e           e  1")
        .L("1                 1")
        .L("1111111111111111111");

        //最终房间
        public static RoomConfig FinalRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Final)
        .L("1111111111111111111")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("d                 1")
        .L("d        #        1")
        .L("d                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1111111111111111111");

    }
} 
