using Degg;
using Degg.Util;
using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class MinigameBase
	{
		[Net, Change]
		public float LastScoreTick { get;set; }

		public void OnLastScoreTickChanged(float previous, float next)
		{
			Event.Run( "ui.scoreboard.refresh" );
		}

		[Net]
		public Dictionary<string, float> Scoreboard { get; set; }
		public void SetupScoreboard()
		{
			Scoreboard = new Dictionary<string, float>();
			foreach(var kv in Players)
			{
				kv.Value.SetScore( 0 );
			}
		}

		public virtual float GetScore( BurdleEntity entity )
		{
			if ( entity.Owner is BurdlePlayer player )
			{
				return GetScore( player );
			}
			return 0;
		}

		public virtual float GetScore( BurdlePlayer player )
		{
			if ( player?.IsValid() ?? false )
			{
				if ( player?.Client?.IsValid() ?? false )
				{
					return player.Client.GetValue<float>( "score", 0 );
				}
			}
			return 0;
		}

		public virtual void SetScore( BurdleEntity entity, float score )
		{
			if ( entity.Owner is BurdlePlayer player )
			{
				SetScore( player, score );
			}
		}

		public virtual void SetScore( BurdlePlayer player, float score )
		{
			SetScore( player, score, true );
		}
		public virtual void SetScore( BurdlePlayer player, float score, bool placeInScoreboard = true )
		{
			if ( IsClient )
			{
				AdvLog.Warning( "Tried to call set score on client" );
				return;
			}

			if ( player.IsValid() )
			{
				player.Client.SetValue( "score", score );
				if ( placeInScoreboard )
				{
					SetScore( player.Client.Name, score );
				}
			}
		}

		public virtual float GetScore(string name)
		{
			if (Scoreboard.ContainsKey(name))
			{
				return Scoreboard[name];
			}
			return 0;
		}

		public virtual void SetScore(string name, float amount)
		{

			if (Scoreboard == null)
			{
				Scoreboard = new Dictionary<string, float>();
			}

			Scoreboard[name] = amount;
			LastScoreTick = Time.Tick;
		}

		public void SetPlayerScoresFromTeam()
		{
			foreach(var kv in Players)
			{
				var team = kv.Value?.MyTeam;
				if ( team?.IsValid() ?? false)
				{
					var score = GetScore( team?.Name );
					SetScore( kv.Value, score, false);
				}
			}
		}

		public virtual float AddScore( string name, float amount )
		{
			var score = GetScore( name ) + amount;
			SetScore( name, score);
			return score;
		}

		public virtual float AddScore(BurdleEntity entity, float amount)
		{
			if ( entity.Owner is BurdlePlayer player )
			{
				return AddScore( player, amount );
			}
			return 0;
		}
		public virtual float AddScore( BurdlePlayer entity, float amount )
		{
			if (IsClient)
			{
				AdvLog.Warning( "Tried to call add score on client" );
				return GetScore(entity);
			}
			if (entity?.IsValid() ?? false)
			{
				var score = GetScore( entity );
				score = score + 1;
				SetScore( entity, score );
				return score;
			}
			return 0;
	
		}





	}
}
