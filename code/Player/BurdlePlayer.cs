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

		public bool UiIsSetup { get; set; } = false;

		public PlayerUi HudPanel { get; protected set; }

		[BindComponent] public FollowCamera Camera { get; set; }

		public override void Respawn()
		{
			base.Respawn();
			Transmit = TransmitType.Owner;
			EnableDrawing = false;
			EnableAllCollisions = false;

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

			if (Burdle?.IsValid() ?? false)
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
			Burdle.UpdatModel();
		}
		public override void ClientSpawn()
		{
			base.ClientSpawn();
		}

		[Event.Hotload]
		public void CreateUi()
		{
			if ( IsClient )
			{
				if ( !HudPanel.IsValid())
				{
					HudPanel = new PlayerUi(this);
				}				
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

		public override void FrameSimulate( Client cl )
		{
			base.FrameSimulate( cl );
			if ( Owner == Client && !UiIsSetup )
			{
				UiIsSetup = true;
				CreateUi();
			}
		}

		public override void Simulate( Client cl )
		{
			var controller = GetActiveController();
			controller?.Simulate( cl, this, GetActiveAnimator() );

			if ( Burdle.IsValid())
			{
				Position = Burdle.Position;
			}

			if ( Burdle != null )
			{
				Burdle.Simulate( cl );
			}
		}
	}
}
