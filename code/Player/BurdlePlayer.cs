using Degg.Cameras;
using Sandbox;

namespace Burdle
{
	public partial class BurdlePlayer : Player
	{

		[Net]
		public BurdleEntity Burdle { get; set; }

		[Net]
		public MinigameBase Gamemode { get; set; }




		public PlayerUi HudPanel { get; protected set; }

		[BindComponent] public FollowCamera Camera { get; set; }

		public override void Respawn()
		{
			base.Respawn();

			// todo: not sure I like this setup, might prefer it like CarEntity
			// so the player is actually a normal terry instead of an invisible entity w/ controller
			SetModel( "models/parts/seats/dev_seat.vmdl" );
			EnableDrawing = false;
			EnableAllCollisions = true;

			Animator = new BurdleAnimator();

			CreateBurdle();
		}



		public BurdleEntity GetBurdle()
		{
			if (Burdle == null || !Burdle.IsValid())
			{
				CreateBurdle();
			}

			return Burdle;
		}

		public void JoinGame(MinigameBase game)
		{
			Gamemode = game;
			game.Join( this );
			
		}

		public void LeaveGame()
		{
			Gamemode.Leave( this );
			Gamemode = null;
		}

		public void CreateBurdle()
		{
			if (IsClient)
			{
				return;
			}

			if (Burdle != null)
			{
				Burdle.Delete();
			}

			if (Camera != null)
			{
				Components.Remove( Camera );
			}

			Camera = Components.Create<FollowCamera>();

			Burdle = Create<BurdleEntity>();
			Burdle.Owner = this;
		}
		public override void ClientSpawn()
		{
			base.ClientSpawn();
			CreateUi();
		}

		[Event.Hotload]
		public void CreateUi()
		{
			if ( IsClient )
			{
				if ( HudPanel?.IsValid() ?? false )
				{
					HudPanel.Delete();
				}

				HudPanel = new PlayerUi();
			}
		}
		public override void OnKilled()
		{
			base.OnKilled();

			EnableAllCollisions = false;
			EnableDrawing = false;
			Burdle.Delete();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

		}

		public override void Simulate( Client cl )
		{
			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );

			if ( Burdle.IsValid())
			{
				Position = Burdle.Position;
			}

			

			if (IsServer)
			{
				if (Input.Pressed(InputButton.Reload))
				{
					// this.Respawn();
				}
			}

			if ( Burdle != null )
			{
				Burdle.Simulate( cl );
			}

		}


	}


}
