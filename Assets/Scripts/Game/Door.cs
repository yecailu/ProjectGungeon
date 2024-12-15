using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Door : ViewController
	{
		public enum States
		{
			Close,
			Open,
		}

		public int X { get; set; }
		public int Y { get; set; }

		public LevelController.DoorDirections Direction { get; set; }


		public FSM<States> State = new FSM<States>();
        public void Awake()
        {
			State.State(States.Open)
				.OnEnter(() =>
				{
                    GetComponent<BoxCollider2D>().isTrigger = true;
					GetComponent<SpriteRenderer>().sprite = DoorOpen;
				})
                .OnExit(() =>
                {
                    AudioKit.PlaySound("resources://DoorOpen");
                }); 

            State.State(States.Close)
				.OnEnter(() =>
				{
					GetComponent<BoxCollider2D>().isTrigger = false;
					GetComponent<SpriteRenderer>().sprite = DoorClose;
				})
				.OnExit(() =>
				{
					AudioKit.PlaySound("resources://DoorOpen");
				});

            State.StartState(States.Open);
        }

    }
}
