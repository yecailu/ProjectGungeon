using System.Collections.Generic;
using UnityEngine;


namespace QFramework.ProjectGungeon 
{
    public class Level3
    {
        public static LevelConfig Config = new LevelConfig()
            .NormalRooms(Lv3Rooms.NormalRooms)
            .Self(self =>
            {
                //难度配置
                self.PacingConfig = new List<int>()
                {
                    3,
                    5,
                    2,
                    6,
                    5,
                    2,
                    3,
                    4,
                    6,
                    4,
                    2,
                    4,
                    3,
                    5,
                    6,
                    2,
                    6,
                    5,
                    2,
                    3,
                    4,
                    6,
                    4,
                    2,
                    4,
                    3,
                    5,
                    6,
                    3,
                    5,
                    2,
                    6,
                    5,
                    2,
                    3,
                    4,
                    6,
                    4,
                    2,
                    4,
                    3,
                    5,
                    6,

                };
            })
            .Self(self =>
            {
                self.InitRoom
                       .Branch(node =>
                       {
                           node.Next(RoomTypes.Normal)
                               .Next(RoomTypes.Normal)
                               .Next(RoomTypes.Normal)
                               .Next(RoomTypes.Chest)
                               .Next(RoomTypes.Final)
                               .Next(RoomTypes.Next);
                       })
                       .Next(RoomTypes.Normal)
                       .Next(RoomTypes.Normal)
                       .Next(RoomTypes.Normal)
                       .Next(RoomTypes.Shop, node =>
                       {
                           node.Next(RoomTypes.Normal)
                               .Next(RoomTypes.Normal)
                               .Next(RoomTypes.Normal)
                               .Next(RoomTypes.Normal)
                               .Next(RoomTypes.Chest);
                       })
                       .Next(RoomTypes.Normal)
                       .Next(RoomTypes.Normal)
                       .Next(RoomTypes.Normal, node =>
                       {
                           node.Next(RoomTypes.Normal)
                               .Next(RoomTypes.Normal)
                               .Next(RoomTypes.Normal)
                               .Next(RoomTypes.Normal)
                               .Next(RoomTypes.Chest);
                       })
                       .Next(RoomTypes.Normal)
                       .Next(RoomTypes.Normal)
                       .Next(RoomTypes.Normal);

            });
        


    }
}
