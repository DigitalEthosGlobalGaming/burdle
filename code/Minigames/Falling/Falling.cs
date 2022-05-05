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
		public bool IsStarted { get; set; }		

		public float MinimumHeight { get; set; }

		public Timer PlayerCheckerTimer { get; set; }
		public override void Init()
		{
			base.Init();
			Name = "Falllling";
			var round = AddRound<MinigameRound>();
			round.Name = Name;
			round.Duration = 60 * 5f;
			IsStarted = false;
		}

		public override void Join( BurdlePlayer player )
		{
			base.Join( player );
			player.SetScore( 1 );
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
					player.Client.SetInt( "score", 1 );
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

			var tiles = 4;
			var tileZ = tiles * 3;
			var tileHeightSpace = 105f;
			var tileSize = 105f;
			MinimumHeight = float.MaxValue;
			var scale = 2f;

			for ( int x = 0; x < tiles; x++ )
			{
				for ( int y = 0; y < tiles; y++ )
				{
					for ( int z = 0; z < tileZ; z++ )
					{
						var position = new Vector3( x * (tileSize * scale), y * (tileSize * scale), z * tileHeightSpace );

						var platform = Create<FallingPlatform>();
						platform.Position = position;
						platform.Scale = scale;
						Platforms.Add( platform );
						TickableChildren.Add( platform );
						if (z == tileZ - 1)
						{
							SpawnPoints.Add( position + Vector3.Up * 250f );
						}
					}
				}
			}

			var cameraFocusPlatform = Platforms[(int)(Platforms.Count/2)];

			var transition = LoadingScene.AddTransition<MovementTransition>();
			transition.Duration = 7f;
			transition.Target = cameraFocusPlatform;
			transition.StartPosition = new Vector3( -1000, -1000, 0 );
			transition.EndPosition = new Vector3( 0,0 , (tileZ + 2) * tileHeightSpace );
			LoadingScene.AddTransition( transition );

			EndPlatform = new Platform();
			MinimumHeight = -750f;
			EndPlatform.Position = new Vector3( (tiles * tileSize) / 2, (tiles * tileSize) / 2, MinimumHeight / 2 );

			EndPlatform.RenderColor = new Color( 0, 0.5f, 0, 0.5f );
			EndPlatform.Scale = tiles * 2;


		}


		
		public override Vector3 GetSpawnPosition( BurdlePlayer player )
		{
			if (player.GetScore() == 0)
			{
				return EndPlatform.Position + (Vector3.Up * 100);
			}
			return Rand.FromList( SpawnPoints );
		}

		public override void SpawnPlayer( BurdlePlayer player )
		{
			base.SpawnPlayer( player );
			if (IsStarted)
			{
				UnFreezePlayers();
			} else
			{
				FreezePlayers();
			}
		}

		public override void OnRoundStart( MinigameRound r )
		{
			base.OnRoundStart( r );
			if (r is GameStartRound || r is GameEndRound)
			{
				IsStarted = false;
				FreezePlayers();
				r.Duration = 7f;
			} else { 
				IsStarted = true;
				UnFreezePlayers();
			}
		}

		public override void OnRoundEnd( MinigameRound r )
		{
			base.OnRoundEnd( r );
			if ( r is GameEndRound )
			{
				BurdleGame.CurrentGame.Minigames.RandomGame();
			}
		}

		public void CheckPlayers( Timer t )
		{
			var livingPlayers = 0;
			var numberOfPlayers = 0;
			var minimumSurvivingPlayers = 1;
			
			foreach ( var kv in Players )
			{
				var player = kv.Value;
				if ( player.IsValid )
				{				
					numberOfPlayers = numberOfPlayers + 1;
					if (player.GetScore() > 0)
					{
						livingPlayers = livingPlayers + 1;
					}

					if ( player.Position.z < 0 )
					{
						player.SetScore( 0 );
						if (player.Position.z < MinimumHeight)
						{							
							SpawnPlayer( player );
						}
					}
				}
			}

			if (numberOfPlayers <=1)
			{
				minimumSurvivingPlayers = 0;
			}

			if ( IsStarted && livingPlayers <= minimumSurvivingPlayers )
			{
				NextRound();
			}
		}
	}
}
