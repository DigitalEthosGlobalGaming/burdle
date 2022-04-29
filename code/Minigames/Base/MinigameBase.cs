using Degg.Cameras;
using Degg.Utils;
using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class MinigameBase: Entity
	{
		public Dictionary<long, BurdlePlayer> Players { get; set; }

		public List<ITickable> TickableChildren { get; set; }

		[Net]
		public Vector3 GamemodeCenter { get; set; }

		[Net]
		public float GameEndTime { get; set; }
		[Net]
		public float GameDuration { get; set; }

		[Net]
		public float GameStartTime { get; set; }

		public float WarmupTime { get; set; }

		public bool IsEnded = false;

		public CinematicScene LoadingScene { get; set; }

		public override void Spawn()
		{
			Name = "Game";
			Transmit = TransmitType.Always;
			Players = new Dictionary<long, BurdlePlayer>();
			TickableChildren = new List<ITickable>();
			if ( IsServer )
			{
				Init();
			}
		}

		public virtual void Init()
		{
			GamemodeCenter = new Vector3( 0, 0, 1000 );
		}
		public virtual Vector3 GetSpawnPosition(BurdlePlayer player)
		{
			return Vector3.Zero;
		}

		public virtual void SpawnPlayer(BurdlePlayer player)
		{
			GetRound();
			var spawn = GetSpawnPosition( player );
			var burdle = player.GetBurdle();
			if ( burdle?.IsValid() ?? false )
			{
				burdle.Velocity = Vector3.Zero;
				burdle.Position = spawn;
			}

			GetRound()?.OnPlayerSpawn( player );
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
			GetRound()?.Join( player );
			SpawnPlayer( player );
		}

		public virtual bool CanStart( )
		{
			return true;
		}
		public virtual void Leave( BurdlePlayer player )
		{
			Players.Remove( player.Client.PlayerId );
			GetRound()?.Leave( player );
		}


		[Event.Tick.Server]
		public void TickEvent()
		{
			Tick();
		}

		public virtual void Tick()
		{
			if (CurrentRound != null)
			{
				CurrentRound.Tick();
			}
			if ( TickableChildren  != null)
			{
				foreach(var i in TickableChildren)
				{
					i.Tick( Time.Delta, Time.Tick );
				}
			}
		}
		public virtual void Start()
		{
			SetupRounds();
			NextRound();
			SetupScoreboard();
		}

		public void End()
		{
			if (IsClient)
			{
				return;
			}
			if ( IsEnded )
			{
				return;
			}
			IsEnded = true;
			Cleanup();

			BurdleGame.CurrentGame.Minigames.RandomGame();
		}

		public virtual void Cleanup()
		{
			DeleteAllRounds();
			if ( LoadingScene?.IsValid() ?? false )
			{
				LoadingScene?.Delete();
			}
		}

		protected override void OnDestroy()
		{
			if ( IsServer )
			{
				Cleanup();
			}
			base.OnDestroy();

		}


	}
}
