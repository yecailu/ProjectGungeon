using System.Collections.Generic;
using static QFramework.ProjectGungeon.LevelController;

namespace QFramework.ProjectGungeon
{

    public class LevelGenHelper
    {
        public static List<LevelController.DoorDirections> GetAvailableDirections(
            DynaGrid<LevelController.RoomGenerateNode> layoutGrid, int x, int y)
        {
            //获取扩散的方向

            var availabelDirections = new List<DoorDirections>();
            if (layoutGrid[x + 1, y] == null)
            {
                availabelDirections.Add(DoorDirections.Right);
            }
            if (layoutGrid[x - 1, y] == null)
            {
                availabelDirections.Add(DoorDirections.Left);
            }
            if (layoutGrid[x, y + 1] == null)
            {
                availabelDirections.Add(DoorDirections.Up);
            }
            if (layoutGrid[x, y - 1] == null)
            {
                availabelDirections.Add(DoorDirections.Down);
            }

            return availabelDirections;
        }


        public class DirectionWithCount
        {
            public LevelController.DoorDirections Direction;
            public int Count;
        }

        public static List<DirectionWithCount> Predict(
            DynaGrid<LevelController.RoomGenerateNode> layoutGrid, int x, int y)
        {
            var availableDirections = GetAvailableDirections(layoutGrid, x, y);

            var directions = new List<DirectionWithCount>();

            foreach(var availableDirection in availableDirections)
            {
                if(availableDirection == LevelController.DoorDirections.Up)
                {
                    var upNodeAvailableDirections = GetAvailableDirections(layoutGrid, x, y + 1);
                    directions.Add(new DirectionWithCount()
                    {
                        Count = upNodeAvailableDirections.Count,
                        Direction = availableDirection
                    });
                }
                if (availableDirection == LevelController.DoorDirections.Down)
                {
                    var downNodeAvailableDirections = GetAvailableDirections(layoutGrid, x, y - 1);
                    directions.Add(new DirectionWithCount()
                    {
                        Count = downNodeAvailableDirections.Count,
                        Direction = availableDirection
                    });
                }
                if (availableDirection == LevelController.DoorDirections.Left)
                {
                    var leftNodeAvailableDirections = GetAvailableDirections(layoutGrid, x - 1, y);
                    directions.Add(new DirectionWithCount()
                    {
                        Count = leftNodeAvailableDirections.Count,
                        Direction = availableDirection
                    });
                }
                if (availableDirection == LevelController.DoorDirections.Right)
                {
                    var rightNodeAvailableDirections = GetAvailableDirections(layoutGrid, x + 1, y);
                    directions.Add(new DirectionWithCount()
                    {
                        Count = rightNodeAvailableDirections.Count,
                        Direction = availableDirection
                    });
                }

                
            }

            return directions;
        }
    }

}
