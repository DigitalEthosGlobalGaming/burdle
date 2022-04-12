using Sandbox;

namespace Burdle
{
	public partial class Target : Platform
	{
		public Team MyTeam { get; set; }
		public override void Spawn()
		{
			base.Spawn();
		}

		public override void StartTouch( Entity other )
		{
			base.StartTouch( other );
			var game = GetGame<BurdleBall>();
			if ( game != null ) {
				if ( other is Ball )
				{
					game.OnScore( this );
				}
			}
		}
	}
}
