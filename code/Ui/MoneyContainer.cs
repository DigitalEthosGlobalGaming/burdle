using Degg.Ui.Elements;
using Sandbox;
using Sandbox.UI;

namespace Burdle
{
	public class MoneyContainer : PlayerPanel<BurdlePlayer>
	{
		public Label Score { get; set; }
		public Panel ScoreContainer { get; set; }

		public int LastScore { get; set; } = 0;
		public float NextScoreTransition { get; set; } = 0;

		public MoneyContainer()
		{
			SetTemplate( "Ui/MoneyContainer.html" );
			StyleSheet.Load( "Ui/MoneyContainer.scss" );
		}

		public override void Tick()
		{
			
			base.Tick();

			if ( Player?.IsValid() ?? false )
			{
				var saveData = Player.GetSaveData<BurdlePlayerSave>();
				if ( saveData != null)
				{
					if ( Score != null )
					{
						var score = saveData.Burds;
						score = score.Floor();
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
							ScoreContainer.SetClass( "value-new", newScore );
						}
						Score.Text = $"${score.ToString()}";
					}
				}				
			}
		}
	}
}
