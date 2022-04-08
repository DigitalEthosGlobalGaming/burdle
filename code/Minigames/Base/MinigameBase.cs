using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class MinigameBase: Entity
	{
		public Dictionary<long, BurdlePlayer> Players { get; set; }

		public override void Spawn()
		{
			Transmit = TransmitType.Always;
			Players = new Dictionary<long, BurdlePlayer>();
		}

		public virtual void SpawnPlayer(BurdlePlayer player)
		{

		}

		public virtual void SpawnAllPlayers()
		{
			foreach ( var item in Players )
			{
				SpawnPlayer( item.Value );
			}
		}

		public virtual void Join( BurdlePlayer player)
		{
			Players[player.Client.PlayerId] = player;
		}
		public virtual void Leave( BurdlePlayer player )
		{
			Players.Remove( player.Client.PlayerId );
		}

		public virtual void Start()
		{

		}
		public virtual void End()
		{

		}
	}
}
