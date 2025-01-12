using System.Collections.Generic;
using static QFramework.ProjectGungeon.RoomConfig;

namespace QFramework.ProjectGungeon
{
    public class LevelConfig
    {
        public RoomNode InitRoom = new RoomNode(RoomTypes.Init);

        public List<int> PacingConfig = new List<int>();

    }
}