using Degg.Util;
using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Burdle
{
	[Library]
	public partial class Platformer: MinigameBase
	{

		public TargetPlatform SpawnPlatform { get; set; }
		public TargetPlatform NextPlatform { get; set; }

		public float NextPlatformSpawn { get; set; }
		public float PreviousPlatformSpawn { get; set; }
		public float PlatformSpawnTime { get; set; }
		public override void SpawnPlayer(BurdlePlayer player)
		{
			base.SpawnPlayer( player );
			player.Client.SetInt( "score", 0 );
		}

		public override Vector3 GetSpawnPosition( BurdlePlayer player )
		{
			return SpawnPlatform.Position + Vector3.Up * 50f;
		}

		public override void Init()
		{
			base.Init();
			PlatformSpawnTime = 3f;
			Name = "Target Burd";
			var round = AddRound<MinigameRound>();
			round.Name = Name;
			round.Duration = 60 * 5f;
		}
		public override void Start()
		{
			base.Start();
			CreatePlatforms();
		}

		public override void Cleanup()
		{			
			SpawnPlatform?.Delete();
			NextPlatform?.Delete();
		}
		public override void SetScore( string name, float score )
		{
			var current = GetScore( name );
			if (score > current)
			{
				base.SetScore( name, score );
			}
		}


		public override void OnRoundStart( MinigameRound r )
		{
			base.OnRoundStart( r );
			NextPlatformSpawn = Time.Now + 5f;
		}

		public void CreatePlatforms()
		{
			var scale = Rand.Float( 0.5f, 3f );
			if (!SpawnPlatform.IsValid())
			{
				SpawnPlatform = Create<TargetPlatform>();
				SpawnPlatform.Position = Vector3.Up * 1000f;
				SpawnPlatform.SetModelAndPhysics( "degg/models/simple/platform.vmdl" );
				SpawnPlatform.Scale = scale;
			} else { 
				SpawnPlatform.Delete();
				SpawnPlatform = NextPlatform;
			}

			NextPlatform = new TargetPlatform();

			var direction = Rand.Float( 0, 360 );

			NextPlatform.Scale = scale;
			var distance = Rand.Float( 160 * SpawnPlatform.Scale, 190 * SpawnPlatform.Scale );

			var rotation = Rotation.FromAxis( Vector3.Up, direction );
			var position = rotation.Forward * distance;

			NextPlatform.Rotation = rotation;

			NextPlatform.Position = SpawnPlatform.Position + position;
			NextPlatform.SetModelAndPhysics( "degg/models/simple/platform.vmdl" );

			NextPlatformSpawn = Time.Now + 5f;

			foreach ( var kv in Players )
			{
				var player = kv.Value;
				if ( player.IsValid )
				{
					if ( player.Position.z < (SpawnPlatform.Position.z - 100f) )
					{
						SpawnPlayer( player );
					}
				}
			}
		}

		public override void Tick()
		{
			base.Tick();
			if (IsGameLoading())
			{
				return;
			}

			var now = Time.Now;
			if ( now > NextPlatformSpawn )
			{
				PreviousPlatformSpawn = now;
				CreatePlatforms();
			}

			var total = NextPlatformSpawn - PreviousPlatformSpawn;
			var diff = NextPlatformSpawn - now;

			var percentage = diff / total;
			if ( SpawnPlatform.IsValid )
			{
				SpawnPlatform.RenderColor = Color.Lerp( Color.Red, Color.White, percentage );
			}
		}
	}
}
