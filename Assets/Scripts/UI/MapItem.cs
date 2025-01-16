using UnityEngine;
using QFramework;
using System.Xml.Schema;

namespace QFramework.ProjectGungeon
{
	public partial class MapItem : ViewController
	{
		private Room mRoom;

        private void Start()
        {
			Button.onClick.AddListener(() =>
			{
				FindAnyObjectByType<UIMap>().Hide();//隐藏地图
				Global.Player.Position2D(mRoom.Position2D() + Vector2.one);//传送
			});
        }

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
				TypeText.text = "起始";
			}
			else
			{
                TypeText.Hide();

            }

			IconGroup.Hide();
			Icon.Hide();


            if (mRoom.GenerateNode.Node.RoomType == RoomTypes.Chest)
            {
				if (mRoom.PowerUps.Count > 0)
				{
					foreach (var roomPowerUp in mRoom.PowerUps)
					{
                        Icon.InstantiateWithParent(IconGroup)
                        .Self(self =>
                        {
                            self.sprite = roomPowerUp.SpriteRenderer.sprite;
                        })
                        .Show();

                        IconGroup.Show();
                    }

				}

            }

            if (Global.CurrentRoom == mRoom)
			{
                TypeText.text = "我";
                TypeText.Show();

            }

        }

	}
}
