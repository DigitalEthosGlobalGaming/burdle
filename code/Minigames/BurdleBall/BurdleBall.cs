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

		public Timer PlayerCheckerTimer { get; set; }

		public Vector3 CenterPosition { get; set; }

		public bool IsOver { get; set; }

		public float MinimumHeight { get; set; }

		public override bool CanStart()
		{
			return false;
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
		}

		public override void Start()
		{
			base.Start();
			AddTeam( "Red" );
			AddTeam( "Blue" );

			GameDuration = 60f * 4;

			Name = "Burdle Ball";
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

		public override void End()
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

			base.End();
		}

		protected override void OnDestroy()
		{
			End();
			base.OnDestroy();
		}

		public void CreatePlatforms()
		{
			Platforms = new List<Platform>();

			var width = 4;
			var height = 8;
			var startPosition = new Vector3( 0, 0, 500 );
			
			var scale = 2;

			var redTeam = GetTeam( "Red" );
			var blueTeam = GetTeam( "Blue" );
			var tileSpacer = 105f;

			for ( int i = 0; i < (width * height); i++ )
			{
				var x = i % height;
				var y = i / height;

				var xPos = x * tileSpacer * scale;
				var yPos = y * tileSpacer * scale;

				var position = startPosition + (Vector3.Forward * xPos) + (Vector3.Right * yPos); 
				if (x == 0)
				{
					redTeam.AddSpawn( position + (Vector3.Up * 100f) );
				} else if ( x == (height - 1) )
				{
					blueTeam.AddSpawn( position + (Vector3.Up * 100f) );
				}

				var platform = Create<Platform>();
				platform.Position = position;
				Platforms.Add( platform );
				platform.Scale = scale;
			}

			CenterPosition = startPosition;
			CenterPosition = CenterPosition + (Vector3.Forward * (height * (tileSpacer * scale) / 2));
			CenterPosition = CenterPosition + (Vector3.Right * (width * (tileSpacer * scale) / 2));
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

			var redScore = 0;
			var blueScore = 0;

			SetScore( "Red", redScore );
			SetScore( "Blue", blueScore );

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
