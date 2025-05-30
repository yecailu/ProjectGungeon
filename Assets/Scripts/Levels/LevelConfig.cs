using System.Collections.Generic;
using static QFramework.ProjectGungeon.RoomConfig;

namespace QFramework.ProjectGungeon
{
    [System.Serializable]
    public class LevelConfig
    {
        // 新增唯一标识字段
        public string LevelID;

        public static string Name;

        public RoomNode InitRoom = new RoomNode(RoomTypes.Init);

        public List<int> PacingConfig = new List<int>();

        public List<RoomConfig> NormalRoomTemplates { get; set; }

        public LevelConfig NormalRooms(List<RoomConfig> normalRooms)
        {
            NormalRoomTemplates = normalRooms;
            return this;
        }

    }
}