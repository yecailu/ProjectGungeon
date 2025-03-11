using System.Collections.Generic;

namespace QFramework.ProjectGungeon
{
    public class SharedRooms
    {

        /*
        1 地块
        @ 主角
        e 敌人
        d 门
        c 宝箱
        b 箱子
        s 商店摊位
        y Color
        # 终点
        */

        //地图序列化，初始房间
        public static RoomConfig InitRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Init)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1              g  1")
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



        //最终房间
        public static List<RoomConfig> FinalRooms { get; set; } = new List<RoomConfig>()
        {
            new RoomConfig()
               .Type(RoomTypes.Final)
               .L("1111111111111d1111111111111")
               .L("1bbbb                     1")
               .L("1bbb                      1")
               .L("1b                        1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("d            e            d")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1                        b1")
               .L("1                       bb1")
               .L("1                      bbb1")
               .L("1                     bbbb1")
               .L("1111111111111d1111111111111"),

            new RoomConfig()
               .Type(RoomTypes.Final)
               .L("1111111111111d1111111111111")
               .L("1bb                     bb1")
               .L("1                         1")
               .L("1                         1")
               .L("1     11           11     1")
               .L("1     11           11     1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("d            e            d")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1         11111111        1")
               .L("1                         1")
               .L("1                         1")
               .L("1b                       b1")
               .L("1bb                     bb1")
               .L("1111111111111d1111111111111"),

            new RoomConfig()
               .Type(RoomTypes.Final)
               .L("1111111111111d1111111111111")
               .L("1b                        1")
               .L("1bb                       1")
               .L("1                         1")
               .L("1     1             1     1")
               .L("1     1             1     1")
               .L("1   111             111   1")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("d            e            d")
               .L("1                         1")
               .L("1                         1")
               .L("1                         1")
               .L("1   111             111   1")
               .L("1     1             1     1")
               .L("1     1             1     1")
               .L("1                         1")
               .L("1                        b1")
               .L("1                       bb1")
               .L("1111111111111d1111111111111"),
        };



        public static RoomConfig ChestRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Chest)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1        c        1")
        .L("1                 1")
        .L("1                 1")
        .L("d                 d")
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
        .L("1      s    s     1")
        .L("1                 1")
        .L("1                 1")
        .L("d    s   s   s    d")
        .L("1                 1")
        .L("1                 1")
        .L("1      s    s     1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("1                 1")
        .L("111111111d111111111");


        public static RoomConfig NextRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Next)
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
    }
}
