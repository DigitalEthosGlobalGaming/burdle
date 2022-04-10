using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class MinigameBase: Entity
	{
		public Dictionary<long, BurdlePlayer> Players { get; set; }

		[Net]
		public float GameEndTime { get; set; }
		[Net]
		public float GameDuration { get; set; }

		[Net]
		public float GameStartTime { get; set; }

		public override void Spawn()
		{
			Name = "Game";
			Transmit = TransmitType.Always;
			Players = new Dictionary<long, BurdlePlayer>();
		}
		public virtual Vector3 GetSpawnPosition(BurdlePlayer player)
		{
			return Vector3.Zero;
		}

		public virtual void SpawnPlayer(BurdlePlayer player)
		{
			var spawn = GetSpawnPosition( player );
			var burdle = player.GetBurdle();
			if ( burdle?.IsValid() ?? false )
			{
				burdle.Velocity = Vector3.Zero;
				burdle.Position = spawn;
			}
		}

		public virtual void SpawnAllPlayers()
		{
			foreach ( var item in Players )
			{
				SpawnPlayer( item.Value );
			}
		}

		public void SetGameDuration(float a)
		{
			GameDuration = a;
		}

		public virtual void Join( BurdlePlayer player)
		{
			Players[player.Client.PlayerId] = player;
			SpawnPlayer( player );
		}

		public virtual bool CanStart( )
		{
			return true;
		}
		public virtual void Leave( BurdlePlayer player )
		{
			Players.Remove( player.Client.PlayerId );
		}


		[Event.Tick.Server]
		public void TickEvent()
		{
			Tick();
		}

		public virtual void Tick()
		{
			if ( GameDuration > 0) {
				if (GameStartTime == -1 ) {
					GameStartTime = Time.Now;
					GameEndTime = GameStartTime + GameDuration;
				}

				if (GameEndTime < Time.Now)
				{
					BurdleGame.CurrentGame.Minigames.RandomGame();
				}
			}
		}
		public virtual void Start()
		{
			GameStartTime = -1;
			GameEndTime = -1;
			SetupScoreboard();
		}
		public virtual void End()
		{

		}
	}
}
