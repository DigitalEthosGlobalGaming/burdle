
using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Burdle
{
	public partial class Team : Entity
	{
		public Color TeamColour { get; set; }
		public HashSet<BurdlePlayer> Players { get; set; }

		public HashSet<Vector3> SpawnPositions { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			TeamColour = Color.Random;
			Players = new HashSet<BurdlePlayer>();
			SpawnPositions = new HashSet<Vector3>();
		}

		public bool IsInTeam(BurdlePlayer player)
		{
			return Players.Contains( player );
		}
		public void Add(BurdlePlayer player)
		{
			if (player?.IsValid() ?? false)
			{
				if ( player.MyTeam?.IsValid() ?? false)
				{
					player.MyTeam.Leave( player );
				}
			}

			Players.Add( player );
			player.MyTeam = this;
		}

		public Vector3 GetRandomSpawn()
		{
			return Rand.FromList( SpawnPositions.ToList() );
		}
		public void AddSpawn(Vector3 spawner)
		{
			SpawnPositions.Add( spawner );
		}

		public void RemoveSpawn(Vector3 pos)
		{
			SpawnPositions.RemoveWhere( ( item ) => item.Equals( pos ) );
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			foreach(var player in Players)
			{
				if ( player?.IsValid() ?? false)
				{
					Leave( player );
				}
			}
		}



		public void Leave(BurdlePlayer player)
		{
			Players.Remove( player );
			player.MyTeam = null;
		}

	}
}
