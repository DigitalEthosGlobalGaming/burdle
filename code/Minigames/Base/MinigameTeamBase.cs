using Degg.Util;
using Degg.Utils;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Burdle
{

	[Library]
	public partial class MinigameTeamBase : MinigameBase
	{
		public HashSet<Team> Teams { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			Teams = new HashSet<Team>();
		}
		public T AddTeam<T>(string name ) where T: Team, new()
		{
			var t = new T();
			t.Name = name;
			return (T) AddTeam( t );
		}

		public void ShuffleTeams()
		{

			foreach ( var kv in Players )
			{
				var team = kv.Value?.MyTeam;
				if ( team?.IsValid() ?? false )
				{
					team.Leave( kv.Value );
				}
			}

			foreach (var kv in Players )
			{
				var team = GetTeamToJoin();
				if ( team != null )
				{
					team.Add( kv.Value );
				}
			}
						
			SpawnAllPlayers();
		}

		public Team AddTeam( string name )
		{
			return AddTeam<Team>( name );
		}

		public Team AddTeam(Team t)
		{
			Teams.Add( t );
			return t;
		}
		public Team GetTeam(string name)
		{
			return GetTeam<Team>( name );
		}

		public T GetTeam<T>(string name) where T: Team
		{
			return Teams.OfType<T>().FirstOrDefault((item) => item.Name == name );
		}

		public Team GetTeamToJoin()
		{
			List<Team> teams = new List<Team>();
			var minimumPlayers = int.MaxValue;
			foreach(var team in Teams)
			{
				var count = team.Players.Count;
				if (team.Players.Count < minimumPlayers )
				{
					teams = new List<Team>();
					minimumPlayers = team.Players.Count;
					teams.Add( team );
				} else if (team.Players.Count == minimumPlayers)
				{
					teams.Add( team );
				}
			}

			return Rand.FromList(teams);
		}

		public override Vector3 GetSpawnPosition(BurdlePlayer player)
		{
			if (player?.MyTeam?.IsValid() ?? false)
			{
				var position =  player.MyTeam.GetRandomSpawn();

				return position;
			}

			return Vector3.Zero;
		}
		public override void Join( BurdlePlayer player )
		{
			if ( player?.MyTeam?.IsValid() ?? false )
			{
				Log.Info( "Already in team" );
				return;
			}

			var team = GetTeamToJoin();
			team.Add( player );

			player.Client.SetValue( "team", team.Name );
			base.Join( player );

		}

		public override void End()
		{
			if ( Teams != null )
			{
				foreach ( var team in Teams )
				{
					team.Delete();
				}
			}
			base.End();
		}

		public override void Leave( BurdlePlayer player )
		{
			if ( player?.IsValid() ?? false )
			{

				if ( player.MyTeam?.IsValid() ?? false )
				{
					player.MyTeam.Leave( player );
				}

				player.MyTeam = null;
			}

			base.Leave( player );
		}
	}
}
