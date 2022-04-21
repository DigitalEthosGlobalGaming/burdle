using Degg.Cameras;
using Degg.Util;
using Degg.Utils;
using Sandbox;
using System;
using System.Collections.Generic;

namespace Burdle
{
	[Library]
	public partial class Racer: MinigameBase
	{

		public List<RacePlatform> Platforms { get; set; }

		public RacePlatform WinningPlatform { get; set; }
		public float NextPlatformSpawn { get; set; }
		public float PreviousPlatformSpawn { get; set; }
		public float PlatformSpawnTime { get; set; }
		public Timer PlayerCheckerTimer { get; set; }

		public bool IsOver { get; set; }

		public float MinimumHeight { get; set; }
		public override void SpawnPlayer(BurdlePlayer player)
		{
			var burdle = player.GetBurdle();

			var currentScore = (int) player.GetScore();
			if ( currentScore <= 0)
			{
				currentScore = 0;
			}

			var platform = Platforms[currentScore];

			if ( currentScore >= Platforms.Count -1)
			{
				platform = WinningPlatform;
			}

			burdle.Velocity = Vector3.Zero;
			burdle.Position = platform.Position + Vector3.Up * 50f;
		}
		public override void Init()
		{
			base.Init();
			Name = "Buuurrrrrdle";
			var round = AddRound<MinigameRound>();
			round.Name = Name;
			round.Duration = 60 * 5f;
		}

		public override void Join( BurdlePlayer player )
		{
			base.Join( player );
			var burdle = player.GetBurdle();
			player.SetScore( 0 );
		}

		public override void Start()
		{
			CreatePlatforms();
			base.Start();
			PlayerCheckerTimer = new Timer( CheckPlayers, 1000f );
			PlayerCheckerTimer.Start();
			
			foreach ( var kv in Players )
			{
				var player = kv.Value;
				if ( player.IsValid )
				{
					player.Client.SetInt( "score", 0 );
					player.Respawn();
				}
			}



		}
		public override void Cleanup()
		{
			base.Cleanup();
			if ( PlayerCheckerTimer != null)
			{
				PlayerCheckerTimer.Delete();
			}
			if ( Platforms != null )
			{
				foreach ( var item in Platforms )
				{
					if ( item.IsValid() )
					{
						item.Delete();
					}
				}
			}
			if (WinningPlatform.IsValid())
			{
				WinningPlatform.Delete();
			}
		}

		public void CreatePlatforms()
		{
			LoadingScene = new CinematicScene();

			Platforms = new List<RacePlatform>();
			WinningPlatform = new RacePlatform();
			WinningPlatform.Index = -1;
			WinningPlatform.Position = new Vector3( -100f, 0, 1000 );
			WinningPlatform.RenderColor = new Color( 0, 0.5f, 0,0.5f);
			WinningPlatform.Scale = 10;

			var tiles = Rand.Int( 5, 20);
			var currentPosition = new Vector3( 0, 0, 500 );
			MinimumHeight = float.MaxValue;
			var maxHeight = float.MinValue;

			for ( int i = 0; i < tiles; i++ )
			{
				var distance = Rand.Float( 150, 300 );
				var scale = Rand.Float( 0.5f, 2 );
				var direction = Rand.Float( -100 * scale, 100 * scale );
				var height = Rand.Float( -100, 100 );
				currentPosition.Abs();
				currentPosition = currentPosition + (Vector3.Forward * distance) + (Vector3.Right * direction) + (Vector3.Up * height);
				currentPosition.z = Math.Abs( currentPosition.z );

				var platform = Create<RacePlatform>();
				platform.Position = currentPosition;
				platform.Scale = scale;
				Platforms.Add( platform );
				platform.Index = i;

				if ( currentPosition.z < MinimumHeight )
				{
					MinimumHeight = currentPosition.z;
				}

				if (currentPosition.z > maxHeight )
				{
					maxHeight = currentPosition.z;
				}

				if (i == 0)
				{
					platform.RenderColor = Color.Blue;
				} else if (i == tiles - 1)
				{
					platform.RenderColor = Color.Green;
				}
			}

			var firstPlatform = Platforms[0];
			var lastPlatform = Platforms[Platforms.Count - 1];

			var transition = LoadingScene.AddTransition<MovementTransition>();
			transition.Duration = 5f;
			transition.Target = firstPlatform;
			maxHeight = maxHeight + 100f;
			transition.StartPosition = lastPlatform.Position.WithZ( maxHeight );;
			transition.EndPosition = firstPlatform.Position.WithZ( maxHeight );
			LoadingScene.AddTransition( transition );
		}

		public override void OnRoundStart( MinigameRound r )
		{
			base.OnRoundStart( r );
			if (r is GameStartRound round)
			{
				round.Duration = 5f;
			}
		}

		public void CheckPlayers( Timer t )
		{
			var winningPlayers = 0;
			var numberOfPlayers = 0;
			
			foreach ( var kv in Players )
			{
				var player = kv.Value;
				if ( player.IsValid )
				{				
					numberOfPlayers = numberOfPlayers + 1;

					if ( player.GetScore() >= Platforms.Count - 1 )
					{
						winningPlayers = winningPlayers + 1;
					}
					if ( player.Position.z < (MinimumHeight - 100f) )
					{
						SpawnPlayer( player );
					}
				}
			}

			if ( numberOfPlayers > 0 && numberOfPlayers == winningPlayers)
			{
				BurdleGame.CurrentGame.Minigames.RandomGame();
			}
		}
	}
}
