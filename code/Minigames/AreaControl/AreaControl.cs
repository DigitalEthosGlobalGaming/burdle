using Degg.Util;
using Degg.Utils;
using Sandbox;
using System;
using System.Collections.Generic;

namespace Burdle
{
	[Library]
	public partial class AreaControl : MinigameBase
	{

		public enum Teams {
			None,
			Blue,
			Red
		}
		public List<AreaControlPlatform> Platforms { get; set; }

		public List<BurdleEntity> RedTeam { get; set; }
		public List<BurdleEntity> BlueTeam { get; set; }

		public Timer PlayerCheckerTimer { get; set; }

		public bool IsOver { get; set; }

		public float MinimumHeight { get; set; }

		public override void SpawnPlayer( BurdlePlayer player )
		{
			var burdle = player.GetBurdle();
			var client = player.Client;

			var currentTeam = client.GetInt( "team", 0 );

			if ( currentTeam == 0 ) {

				List<BurdleEntity> Team = null;

				var teamRandom = Rand.Float();

				if ( teamRandom > 0.5f )
				{
					Team = RedTeam;
				}
				else
				{
					Team = BlueTeam;
				}

				var redCount = RedTeam.Count;
				var blueCount = BlueTeam.Count;

				if ( redCount != blueCount )
				{
					if ( redCount < blueCount )
					{
						Team = RedTeam;
					} else
					{
						Team = BlueTeam;
					}
				}


				if ( Team == BlueTeam )
				{
					burdle.RenderColor = new Color( 255, 255, 255 );
					client.SetInt( "team", (int)Teams.Blue );
				} else
				{
					burdle.RenderColor = new Color( 255, 255, 255 );
					client.SetInt( "team", (int)Teams.Red );
				}
				Team.Add( burdle );
			}

			burdle.Velocity = Vector3.Zero;
			burdle.Position = Rand.FromList( Platforms ).Position + Vector3.Up * 250f;
		}

		public override bool CanStart()
		{
			if (BurdleGame.GetAllPlayers().Count >=2)
			{
				return true;
			}
			return false;
		}

		public override void Join( BurdlePlayer player )
		{
			base.Join( player );
			var burdle = player.GetBurdle();
			burdle.GiveRandomHat();
			player.Client.SetInt( "team", 0 );
		}

		public override void Start()
		{
			base.Start();
			GameDuration = 60f * 4;
			End();
			Name = "Burdle Control";
			RedTeam = new List<BurdleEntity>();
			BlueTeam = new List<BurdleEntity>();
			PlayerCheckerTimer = new Timer( CheckPlayers, 1000f );
			PlayerCheckerTimer.Start();

			CreatePlatforms();
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

		public override void End()
		{
			if ( PlayerCheckerTimer != null )
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
		}

		protected override void OnDestroy()
		{
			End();
			base.OnDestroy();
		}

		public void CreatePlatforms()
		{
			Platforms = new List<AreaControlPlatform>();

			var tiles = Rand.Int( 4, 8 );
			var startPosition = new Vector3( 0, 0, 500 );
			MinimumHeight = 400f;

			for ( int i = 0; i < (tiles * tiles); i++ )
			{
				var x = i % tiles;
				var y = i / tiles;
				var xPos = x * 300;
				var yPos = y * 300;

				var position = startPosition + (Vector3.Forward * xPos) + (Vector3.Right * yPos) + (Vector3.Up * Rand.Float( 0, 100 )); 

				var platform = Create<AreaControlPlatform>();
				platform.Position = position;
				Platforms.Add( platform );
				platform.Scale = 2;
				
			}
		}

		public Teams GetTeam(BurdleEntity ent)
		{
			if (RedTeam.Contains( ent ) )
			{
				return Teams.Red;
			}
			return Teams.Blue;
		}


		public void CleanTeams()
		{
			RedTeam.RemoveAll( ( item ) =>
			{
				return !(item.Owner.IsValid() && item.Owner.Client.IsValid());
			} );
			BlueTeam.RemoveAll( ( item ) =>
			 {
				 return !(item.Owner.IsValid() && item.Owner.Client.IsValid()); 
			 } );
		}
		public void CheckPlayers( Timer t )
		{
			CleanTeams();

			var redScore = 0;
			var blueScore = 0;

			foreach ( var platform in Platforms )
			{
				if ( platform.Team == Teams.Red)
				{
					redScore = redScore + 1;
				} else if (platform.Team == Teams.Blue)
				{
					blueScore = blueScore + 1;
				}
			}

			SetScore( "Red", redScore );
			SetScore( "Blue", blueScore );

			foreach ( var kv in Players )
			{
				var player = kv.Value;
				if ( player.IsValid )
				{
					if (player.Client.GetInt("team") == (int)Teams.Red) {
						player.SetScore(redScore);
					} else
					{
						player.SetScore( blueScore );
					}

					if ( player.Position.z < (MinimumHeight - 100f) )
					{
						SpawnPlayer( player );
					}
				}
			}
		}

		public override void SetScore( BurdlePlayer player, float score )
		{
			if ( IsClient )
			{
				AdvLog.Warning( "Tried to call set score on client" );
				return;
			}

			if ( player.IsValid() )
			{
				player.Client.SetValue( "score", score );
			}
		}
	}
}
