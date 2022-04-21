using Degg.Util;
using Degg.Utils;
using Sandbox;
using System;
using System.Collections.Generic;

namespace Burdle
{
	[Library]
	public partial class BurdleBall : MinigameTeamBase
	{
		public List<Platform> Platforms { get; set; }

		public Ball GameBall {get;set;}

		public Team BlueTeam { get; set; }
		public Team RedTeam { get; set; }

		public Timer PlayerCheckerTimer { get; set; }

		public Vector3 CenterPosition { get; set; }

		public bool IsOver { get; set; }

		public float MinimumHeight { get; set; }

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
			SetPlayerScoresFromTeam();
		}

		public override void Init()
		{
			base.Init();
			Name = "Burdle Ball";
			var round = AddRound<MinigameRound>();
			round.Name = Name;
			round.Duration = 60 * 5f;
		}

		public override void Start()
		{
			base.Start();
			AddTeam( "Red" );
			AddTeam( "Blue" );
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

			ResetBall();
		}

		public override void Cleanup()
		{
			if ( PlayerCheckerTimer != null )
			{
				PlayerCheckerTimer.Delete();
			}
			GameBall?.Delete();
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

		public void CreatePlatforms()
		{
			Platforms = new List<Platform>();

			var width = 4;
			var height = 8;
			var startPosition = new Vector3( 0, 0, 500 );
			
			var scale = 1.5f;

			var redTeam = GetTeam( "Red" );
			var blueTeam = GetTeam( "Blue" );
			var spacerAmount = 5f;
			var tileSpacer = 105f;
			var tileHeight = 100f * scale;

			for ( int y = 0; y < (height / 2); y++ )
			{
				var yPos = (tileSpacer / 2 * scale) + (tileSpacer * 2 * y * scale);
				var xPos = -(tileHeight / 2) - 5f;
				var zPos = ((tileHeight *2) / 2) + 10f;

				// First edge
				var position = startPosition + (Vector3.Forward * yPos) + ( Vector3.Right * xPos) + (Vector3.Up * zPos);
				var platform = Create<Platform>();
				platform.Rotation = Rotation.From( new Angles(180,0,90) );
				platform.Position = position;
				Platforms.Add( platform );
				platform.Scale = scale;
				platform.SetType( Platform.PlatformTypes.p2x2Thin);
				platform.RenderColor = Color.White.WithAlpha( 0.5f );

				// Second Edge
				xPos = -(tileHeight / 2) + (width * tileSpacer) * scale - 10f;
				position = startPosition + (Vector3.Forward * yPos) + (Vector3.Right * xPos) + (Vector3.Up * zPos);
				platform = Create<Platform>();
				platform.Rotation = Rotation.From( new Angles( 180, 0, 90 ) );
				platform.Position = position;
				Platforms.Add( platform );
				platform.Scale = scale;
				platform.SetType( Platform.PlatformTypes.p2x2Thin );
				platform.RenderColor = Color.White.WithAlpha( 0.5f );
			}

			for ( int i = 0; i < (width * height); i++ )
			{
				var x = i % height;
				var y = i / height;

				var xPos = x * tileSpacer * scale;
				var yPos = y * tileSpacer * scale;

				var position = startPosition + (Vector3.Forward * xPos) + (Vector3.Right * yPos); 


				var platform = Create<Platform>();
				platform.Position = position;
				Platforms.Add( platform );
				platform.Scale = scale;

				if ( x == 0 )
				{
					redTeam.AddSpawn( position + (Vector3.Up * 100f) );
					platform.RenderColor = Color.Lerp( Color.White, Color.Red, 0.5f );
				}
				else if ( x == (height - 1) )
				{
					blueTeam.AddSpawn( position + (Vector3.Up * 100f) );
					platform.RenderColor = Color.Lerp( Color.White, Color.Blue, 0.5f );
				}
			}

			CenterPosition = startPosition;
			var centerX = Vector3.Right * ((width * (tileHeight + spacerAmount) / 2) - ((tileHeight + spacerAmount) / 2));
			var centerY = Vector3.Forward * ((height * (tileHeight + spacerAmount) / 2) - ((tileHeight + spacerAmount) / 2));
			CenterPosition = startPosition + centerX + centerY;

			var redTarget = Create<Target>();
			redTarget.Scale = scale;
			redTarget.Position = centerX + startPosition + (Vector3.Up * (tileHeight / 2)) - (Vector3.Forward * tileHeight / 2);
			redTarget.RenderColor = Color.Red.WithAlpha( 0.5f );
			redTarget.Rotation = Rotation.From( new Angles( 0, 90, 90 ) );
			redTarget.SetType( Platform.PlatformTypes.p2x1Thin );
			redTarget.MyTeam = redTeam;
			Platforms.Add( redTarget );

			var blueTarget = Create<Target>();
			blueTarget.Scale = scale;
			blueTarget.Position = centerX + (Vector3.Forward * tileHeight / 2) + startPosition + (Vector3.Up * (tileHeight / 2)) + (centerY * 2);
			blueTarget.RenderColor = Color.Blue.WithAlpha(0.5f);
			blueTarget.Rotation = Rotation.From( new Angles( 0, 90, 90 ) );
			blueTarget.SetType( Platform.PlatformTypes.p2x1Thin );
			blueTarget.MyTeam = blueTeam;
			Platforms.Add( blueTarget );

			SetScore( blueTeam.Name, 0 );
			BlueTeam = blueTeam;
			SetScore( redTeam.Name, 0 );
			RedTeam = redTeam;
		}

		public void ResetBall()
		{
			GameBall?.Delete();
			GameBall = new Ball();
			GameBall.Position = Vector3.One * CenterPosition;
			GameBall.Velocity = Vector3.Up * 500f;
		}

		public void CheckPlayers( Timer t )
		{
			foreach ( var kv in Players )
			{
				var player = kv.Value;
				if ( player.IsValid )
				{
					if ( player.Position.z < (MinimumHeight - 100f) )
					{
						SpawnPlayer( player );
					}
				}
			}

			if ( GameBall?.IsValid() ?? false)
			{
				if ( GameBall.Position.z < 0 )
				{
					ResetBall();
				}
			}
		}

		public void OnScore( Target target )
		{
			var scoringTeam = BlueTeam;
			if (target.MyTeam == BlueTeam) {
				scoringTeam = RedTeam;
			}

			AddScore( scoringTeam.Name, 1 );
			SetPlayerScoresFromTeam();
			ResetBall();
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
