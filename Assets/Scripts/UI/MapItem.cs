using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class MapItem : ViewController
	{
		private Room mRoom;

		public MapItem WithDate(Room room)
		{
			mRoom = room;
			UpdateView();
			return this;
		}

		void UpdateView()
		{
			LeftDoor.Hide();
			RightDoor.Hide(); 
            UpDoor.Hide();
			DownDoor.Hide();

			foreach (var direction in mRoom.GenerateNode.Directions)
			{
				if (direction == LevelController.DoorDirections.Right)
				{
					RightDoor.Show();
				}

				if (direction == LevelController.DoorDirections.Left)
				{
					LeftDoor.Show();
				}

				if (direction == LevelController.DoorDirections.Up)
				{
					UpDoor.Show();
				}

				if (direction == LevelController.DoorDirections.Down)
				{
					DownDoor.Show();
				}

			}

			if(mRoom.GenerateNode.Node.RoomType == RoomTypes.Init)
			{
				TypeText.text = "∆ º";
			}
			else if (mRoom.GenerateNode.Node.RoomType == RoomTypes.Chest)
            {
                TypeText.text = "±¶œ‰";
            }
			else
			{
                TypeText.Hide();

            }

			IconGroup.Hide();
			Icon.Hide();

			if(Global.currentRoom == mRoom)
			{
                TypeText.text = "Œ“";
                TypeText.Show();

            }

        }

	}
}
