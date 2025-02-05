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
        s 商店摊位
        y Color
        # 终点
        */

        //地图序列化，初始房间
        public static RoomConfig InitRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Init)
        .L("111111111d111111111")
        .L("1                 1")
        .L("1  y   y  y  y  g 1")
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
        .L("1   cccccccccccc  1")
        .L("1   cccccccccccc  1")
        .L("1   cccccccccccc  1")
        .L("111111111d111111111");



        //最终房间
        public static RoomConfig FinalRoom { get; set; } = new RoomConfig()
        .Type(RoomTypes.Final)
         .L("1111111111111d1111111111111")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("d            #e           d")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1                         1")
         .L("1111111111111d1111111111111");



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

    }
}
