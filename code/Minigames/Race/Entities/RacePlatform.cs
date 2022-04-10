
using Sandbox;

namespace Burdle
{
	public partial class RacePlatform: Platform
	{
		public int Index { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			SetModelAndPhysics( "degg/models/simple/platform.vmdl" );
		}

		public override void FirstTouch( BurdleEntity entity )
		{
				var currentScore = entity.GetScore();
				if (currentScore < Index)
				{
					entity.SetScore( Index );
				}

				var current = GetGame<Racer>();
				if ( Index >= current.Platforms.Count -1)
				{
					current.SpawnPlayer( (BurdlePlayer)entity.Owner );
				}
		}
	}
}
