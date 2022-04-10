using Sandbox;

namespace Burdle
{
	public partial class AreaControlPlatform : Platform
	{

		public AreaControl.Teams Team { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			SetModelAndPhysics( "degg/models/simple/platform.vmdl" );
		}

		public override void StartTouch( Entity other )
		{
			base.StartTouch( other );

			if ( other is BurdleEntity ent )
			{				
				var current = GetGame<AreaControl>();
				var newTeam = current.GetTeam( ent );

				if ( Team != newTeam )
				{
					Team = newTeam;
					if ( Team == AreaControl.Teams.Blue )
					{
						RenderColor = Color.Lerp( Color.Blue, Color.White, 0.5f );
						RenderDirty();
					}
					else
					{
						RenderColor = Color.Lerp( Color.Red, Color.White, 0.5f );
						RenderDirty();
					}
				}
			}
			
		}


	}
}
