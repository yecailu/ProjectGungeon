using UnityEngine;
using QFramework;

namespace QFramework.ProjectGungeon
{
	public partial class Door : ViewController
	{
		public enum States
		{
            IdleOpen,
			IdleClose,
			Open,
            BattleClose,
		}

		public int X { get; set; }
		public int Y { get; set; }

		public LevelController.DoorDirections Direction { get; set; }


		public FSM<States> State = new FSM<States>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && State.CurrentStateId == States.IdleClose)
            {
                State.ChangeState(States.Open);
                AudioKit.PlaySound("resources://DoorOpen");
            }
        }

        public void Awake()
        {
            State.State(States.IdleOpen)
                .OnEnter(() =>
                 {
                     SelfSpriteRenderer.sprite = DoorOpen;
                     SelfBoxCollider2D.Disable();
                 });

            State.State(States.IdleClose)
            .OnEnter(() =>
            {
                SelfBoxCollider2D.isTrigger = true;
                SelfSpriteRenderer.sprite = DoorClose;
            });

            State.State(States.Open)
				.OnEnter(() =>
				{
                    AudioKit.PlaySound("resources://DoorOpen");
                    SelfBoxCollider2D.Disable();
                    SelfSpriteRenderer.sprite = DoorOpen;
				});

            State.State(States.BattleClose)
               .OnEnter(() =>
               {
                   SelfBoxCollider2D.isTrigger = false;
                   SelfBoxCollider2D.Enable();
                   SelfSpriteRenderer.sprite = DoorClose;
               });

            State.StartState(States.IdleClose);
        }

    }
}
