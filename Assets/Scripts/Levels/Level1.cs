using UnityEngine;


namespace QFramework.ProjectGungeon 
{
    public class Level1
    {
        public static LevelConfig Config = new LevelConfig()
            .Self(self =>
            {
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
                        .Next(RoomTypes.Final);
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
                        .Next(RoomTypes.Final);
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
                        .Next(RoomTypes.Final);
                }
            });
        


    }
}
