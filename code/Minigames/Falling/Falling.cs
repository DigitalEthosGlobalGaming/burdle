using Degg.Cameras;
using Degg.Util;
using Degg.Utils;
using Sandbox;
using System;
using System.Collections.Generic;

namespace Burdle
{
	[Library]
	public partial class Falling: MinigameBase
	{

		public List<FallingPlatform> Platforms { get; set; }

		public List<Vector3> SpawnPoints { get; set; }

		public Platform EndPlatform { get; set; }
		public bool IsOver { get; set; }

		public float MinimumHeight { get; set; }

		public Timer PlayerCheckerTimer { get; set; }
		public override void Init()
		{
			base.Init();
			Name = "Falllling";
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
			if (EndPlatform.IsValid())
			{
				EndPlatform.Delete();
			}
		}

		public void CreatePlatforms()
		{
			SpawnPoints = new List<Vector3>();
			LoadingScene = new CinematicScene();

			Platforms = new List<FallingPlatform>();

			var tiles = 7;
			var tileSize = 105f;
			MinimumHeight = float.MaxValue;
			var maxHeight = float.MinValue;
			var scale = 2f;

			for ( int x = 0; x < tiles; x++ )
			{
				for ( int y = 0; y < tiles; y++ )
				{
					for ( int z = 0; z < tiles; z++ )
					{
						var position = new Vector3( x, y, z );
						position = position * (tileSize * scale);

						var platform = Create<FallingPlatform>();
						platform.Position = position;
						platform.Scale = scale;
						Platforms.Add( platform );
						TickableChildren.Add( platform );
						if (z == tiles -1)
						{
							SpawnPoints.Add( position + Vector3.Up * 50f );
						}
					}
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

			EndPlatform = new Platform();
			EndPlatform.Position = new Vector3( (tiles * tileSize) / 2, (tiles * tileSize) / 2, -500f );
			MinimumHeight = -200f;
			EndPlatform.RenderColor = new Color( 0, 0.5f, 0, 0.5f );
			EndPlatform.Scale = tiles * 2;
		}

		public override Vector3 GetSpawnPosition( BurdlePlayer player )
		{
			return Rand.FromList( SpawnPoints );
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
