using System.Collections.Generic;
using UnityEngine;


namespace QFramework.ProjectGungeon
{
    public class Level2
    {
        public static LevelConfig Config = new LevelConfig()
            .NormalRooms(Lv2Rooms.NormalRooms)
            .Self(self =>
            {
                //难度配置 1-10难度逐渐提高
                self.PacingConfig = new List<int>()
                {
                    3,
                    2,
                    4,
                    1,
                    5,
                    4,
                    3,
                    3,
                    2,
                    2,
                    4,
                    5,
                    3,
                    2,
                    4,
                    5,
                    3,
                    3,
                    5,
                    3,
                    1,
                    2,
                    2,
                    2,
                    5,
                    3,
                    2,

                };
            })
            .Self(self =>
            {

                self.InitRoom
                        .Branch(node =>
                        {
                            node.Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Chest)
                                .Next(RoomTypes.Final);
                        })
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Shop, node =>
                        {
                            node.Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Chest);
                        })
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Normal, node =>
                        {
                            node.Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Chest);
                        })
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Normal);
                        

            });



    }
}
