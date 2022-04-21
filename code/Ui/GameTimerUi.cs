using Degg.Ui.Elements;
using Sandbox;
using Sandbox.UI;
using System;

namespace Burdle
{
	public class GameTimerUi : PlayerPanel<BurdlePlayer>
	{
		public Label TimeLeft { get; set; }

		public GameTimerUi()
		{
			SetTemplate( "Ui/GameTimerUi.html" );
			StyleSheet.Load( "Ui/GameTimerUi.scss" );
		}

		public override void Tick()
		{
			base.Tick();
			var player = GetClientPawn();
			var current = player?.Gamemode;
			if ( current != null )
			{
				var timeLeft = current.GameEndTime - Time.Now;
				if ( timeLeft  < 0)
				{
					timeLeft = 0;
				}
				var hasTimer = timeLeft > 0;
				SetClass( "hidden", !hasTimer );
				if ( hasTimer )
				{
					var difference = Math.Round( timeLeft );
					var seconds = Math.Floor( difference % 60 );
					var minutes = Math.Floor( difference / 60 );
					TimeLeft.Text = $"{minutes}:{seconds}";
				}
			}

		}

	}
}
