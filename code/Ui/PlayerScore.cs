using Sandbox;
using Sandbox.UI;

namespace Burdle
{
	public class PlayerScore : Panel
	{
		public Label Score { get; set; }
		public Panel ScoreContainer { get; set; }


		public int LastScore { get; set; } = 0;
		public float NextScoreTransition { get; set; } = 0;

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
				var score = Local.Client.GetInt( "score", 0 );
				var newScore = true;
				if (LastScore != score && score > 0)
				{
					NextScoreTransition = Time.Now + 1f;
					LastScore = score;
					newScore = true;
				} else if ( NextScoreTransition <= Time.Now )
				{
					newScore = false;
				}

				if ( ScoreContainer != null )
				{
					ScoreContainer.SetClass( "score-new", newScore );
				}
				Score.Text = score.ToString();
			}

		}

	}
}
