using Sandbox;
using Sandbox.UI;
using System;

namespace Burdle
{
	public class GameTimerUi : Panel
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
			var current = BurdleGame.CurrentGame?.Minigames?.Current;
			if ( current != null )
			{
				var hasTimer = current.GameEndTime > 0;
				SetClass( "hidden", !hasTimer );
				if ( hasTimer )
				{
					var difference = Math.Round( current.GameEndTime - Time.Now );
					var seconds = Math.Floor( difference % 60 );
					var minutes = Math.Floor( difference / 60 );
					TimeLeft.Text = $"{minutes}:{seconds}";
				}
			}

		}

	}
}
