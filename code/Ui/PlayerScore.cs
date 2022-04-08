using Sandbox;
using Sandbox.UI;

namespace Burdle
{
	public class PlayerScore : Panel
	{
		public Label Score { get; set; }

		public PlayerScore()
		{
			SetTemplate( "Ui/PlayerScore.html" );
			StyleSheet.Load( "Ui/PlayerScore.scss" );
		}

		public override void Tick()
		{
			base.Tick();
			if ( Score != null )
			{
				Score.Text = Local.Client.GetInt( "score", 0 ).ToString();
			}

		}

	}
}
