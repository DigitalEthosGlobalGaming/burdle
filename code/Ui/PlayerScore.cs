using Degg.Ui.Elements;
using Sandbox;
using Sandbox.UI;

namespace Burdle
{
	public class PlayerScore : PlayerPanel<BurdlePlayer>
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

			if ( Player?.IsValid() ?? false )
			{
				var chats = Player.Client.GetValue<float>( "chats" ).ToString();
				if ( Score != null )
				{
					var score = Player.GetScore();
					var newScore = true;
					if ( LastScore != score && score > 0 )
					{
						NextScoreTransition = Time.Now + 1f;
						LastScore = (int)score;
						newScore = true;
					}
					else if ( NextScoreTransition <= Time.Now )
					{
						newScore = false;
					}

					if ( ScoreContainer != null )
					{
						ScoreContainer.SetClass( "score-new", newScore );
					}
					Score.Text = $"{score.ToString()} - {chats}";
				}
			}
		}
	}
}
