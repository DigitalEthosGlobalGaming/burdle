using Degg.Cameras;
using Sandbox;

namespace Burdle
{
	public partial class BurdlePlayer
	{
		public float AddScore(float amount)
		{
			if ( Gamemode?.IsValid() ?? false )
			{
				return Gamemode.AddScore( this, amount );
			}
			return 0;
		}
		public void SetScore( float amount )
		{
			if ( Gamemode?.IsValid() ?? false )
			{
				Gamemode.SetScore( this, amount );
			}
		}
		public float GetScore()
		{
			if ( Gamemode?.IsValid() ?? false )
			{
				return Gamemode.GetScore( this );
			}
			return 0;
		}

		public float GetScoreboardScore()
		{
			if ( Gamemode?.IsValid() ?? false )
			{
				var name = Name;
				return Gamemode.GetScore( name );
			}
			return 0;
		}
	}
}
