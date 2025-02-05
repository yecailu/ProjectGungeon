using System.Collections.Generic;
using UnityEngine;


namespace QFramework.ProjectGungeon 
{
    public class Level1
    {
        public static LevelConfig Config = new LevelConfig()
            .NormalRooms(Lv1Rooms.NormalRooms)
            .Self(self =>
            {
                //难度配置
                self.PacingConfig = new List<int>()
                {
                    1,
                    2,
                    1,
                    2,
                    3,
                    1,
                    2,
                    3,
                    2,
                    1,
                    1,
                    2,
                    3,
                    1,
                    1,

                };
            })
            .Self(self =>
            {
                //测试用
                self.InitRoom
                .Next(RoomTypes.Chest)
                .Next(RoomTypes.Shop);
                //.Next(RoomTypes.Final)
                //.Next(RoomTypes.Next);
                return;
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest)
                //.Next(RoomTypes.Chest);

                //return;

                var randomIndex = Random.Range(0, 2 + 1);

                if (randomIndex == 0)
                {
                    self.InitRoom
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Shop, node =>
                        {
                            node.Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Chest);
                        })
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Final)
                        .Next(RoomTypes.Next);
                }
                else if (randomIndex == 1)
                {
                    self.InitRoom
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Chest, node =>
                        {
                            node.Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Shop);
                        })
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Final)
                        .Next(RoomTypes.Next);
                }
                else if (randomIndex == 2)
                {
                    self.InitRoom
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Normal, node =>
                        {
                            node.Next(RoomTypes.Chest)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Normal)
                                .Next(RoomTypes.Shop);
                        })
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Normal)
                        .Next(RoomTypes.Final)
                        .Next(RoomTypes.Next);
                }
            });
        


    }
}
