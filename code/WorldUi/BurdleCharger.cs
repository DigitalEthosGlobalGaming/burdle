
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Burdle
{
	public partial class BurdleCharger : WorldPanel
	{
		public float Charge { get; set; }
		public bool IsCharging { get; set; }
		public BurdleEntity Owner { get; init; } = null;
		public BurdlePlayer Player { get; init; } = null;
		public Panel Inner { get; set; }
		public BurdleCharger( BurdleEntity owner )
		{
			SetTemplate( "/WorldUi/BurdleCharger.html" );
			StyleSheet.Load( "/WorldUi/BurdleCharger.scss" );
			var size = 750;
			PanelBounds = new Rect( -(size / 2f), -(size / 2), size, size );

			Owner = owner;
			if (owner.Owner is BurdlePlayer player)
			{
				Player = player;
			}
		}


		[Event.PreRender]
		public void MoveCharger()
		{
			if (!(Owner?.IsValid() ?? false))
			{
				return;
			}

			// Don't tidy up here, it's the owner's responsibility
			if ( !Owner.IsValid() )
				return;


			Rotation = Rotation.FromAxis( Vector3.Left, -90f ) ;
			Position = Owner.Position +  (Vector3.Up * 1f);

			if ( Player != null )
			{
				var charge = Player.JumpPower;
				SetClass( "disabled", !Owner.CanJump );
				if (charge > 0 && Owner.CanJump)
				{
					Style.Opacity = 1f;
				}
				else
				{
					Style.Opacity = 0f;
				}
				if ( Inner != null )
				{

					int amount = (int)(charge * 100);
					Inner.Style.Set( "width", $"{amount}%" );
					Inner.Style.Set( "height", $"{amount}%" );

				}
			} else {
					Style.Opacity = 0f;
				}
			

		}

		public override void Tick()
		{
			base.Tick();

		}
	}
}
