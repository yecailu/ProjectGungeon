using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class UIMap : ViewController
	{
        private void OnEnable()
        {
            Global.UIOpened = true;
            Time.timeScale = 0;//��ͣ
            MapItemRoot.DestroyChildren();//ɾ��MapItemRoot����������

            Global.RoomGrid.ForEach((x,y,room) =>
            {
                if(room.State.CurrentStateId == Room.RoomStates.Unlocked)
                {
                    MapItem.InstantiateWithParent(MapItemRoot)
                    .WithDate(room)
                    .LocalPosition(x * 60, y * 60)
                    .Show();
                }

                if(room == Global.CurrentRoom)
                {
                    MapItemRoot.LocalPosition(-x * 60, -y * 60);
                }

            });

        }

        private void OnDisable()
        {
            Time.timeScale = 1;//�ָ�
            Global.UIOpened = false;
        }
    }
}
