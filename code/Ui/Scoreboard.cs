using Degg.Ui.Elements;
using Degg.Util;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace Burdle
{
	public class Scoreboard : PlayerPanel<BurdlePlayer>
	{
		public Label CurrentGame { get; set; }
		public Panel ScoreContainer { get; set; }

		public bool IsOpened { get; set; }

		public List<Panel> Scores { get; set; }

		public BurdlePlayer Owner { get; set; }

		public int LastScore { get; set; } = 0;
		public float NextScoreTransition { get; set; } = 0;

		public Scoreboard()
		{
			SetTemplate( "Ui/Scoreboard.html" );
			StyleSheet.Load( "Ui/Scoreboard.scss" );
		}

		[Event( "ui.scoreboard.refresh" )]
		public void RefreshScores()
		{
			var items = new PriorityQueue<string, float>();
			var gamemode = GetPlayer()?.Gamemode;
			var scores = gamemode.Scoreboard;
			CurrentGame.Text = gamemode.Name;
			if (scores != null)
			{
				foreach(var score in scores)
				{
					items.Enqueue( score.Key, score.Value );
				}
			}
			if ( Scores != null )
			{
				foreach ( var scorePanel in Scores )
				{
					scorePanel.Delete( true );
				}
			}

			Scores = new List<Panel>();
			ScoreContainer.DeleteChildren( true );

			var position = items.Count;
			while (items.TryDequeue(out var name, out var amount))
			{				
				var panel = ScoreContainer.AddChild<Panel>();
				panel.AddClass( "score-item" );

				var namePanel = panel.AddChild<Panel>();
				namePanel.AddClass( "amount" );
				var nameLabel = namePanel.AddChild<Label>();
				nameLabel.AddClass( "name" );
				nameLabel.SetText( $"{position.ToString()} | {name}");

				var scorePanel = panel.AddChild<Panel>();
				scorePanel.AddClass( "amount" );
				var scoreLabel = scorePanel.AddChild<Label>();
				scoreLabel.SetText( amount.ToString() );
				position = position - 1;
			}

		}

		[Event.PreRender]
		public void Prerender()
		{
			if ( Player?.IsValid() ?? false )
			{
				var isMenuPressed = AdvInput.Down( InputButton.Score, InputButton.Menu );
				if ( IsOpened != isMenuPressed )
				{
					RefreshScores();
				}
				IsOpened = isMenuPressed;
				SetClass( "open", isMenuPressed );
			}
		}

	}
}
