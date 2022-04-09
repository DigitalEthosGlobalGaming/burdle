
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

		public override void FirstTouch(Entity other)
		{
			if (other is BurdleEntity)
			{
				var currentScore = other.Client.GetInt( "score", 0 );
				if (currentScore < Index)
				{
					other.Client.SetInt( "score", Index );
				}

				var current = GetGame<Racer>();
				if ( Index >= current.Platforms.Count -1)
				{
					current.SpawnPlayer( (BurdlePlayer) other.Owner );
				}
			}
		}
	}
}
