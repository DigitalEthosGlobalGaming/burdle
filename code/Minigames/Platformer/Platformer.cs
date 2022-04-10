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
			var burdle = player.GetBurdle();
			burdle.GiveRandomHat();
			burdle.Velocity = Vector3.Zero;
			burdle.Position = SpawnPlatform.Position + Vector3.Up * 50f;
			player.Client.SetInt( "score", 0 );
		}
		public override void Start()
		{
			base.Start();
			GameDuration = 60f * 3;
			PlatformSpawnTime = 3f;
			End();
			Name = "Target Burd";
			CreatePlatforms();
		}

		public override void End()
		{
			SpawnPlatform?.Delete();
			NextPlatform?.Delete();
		}

		protected override void OnDestroy()
		{
			End();
			base.OnDestroy();			
		}

		public override void SetScore( string name, float score )
		{
			var current = GetScore( name );
			if (score > current)
			{
				base.SetScore( name, score );
			}
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
			NextPlatform.SetModelAndPhysics( "degg/models/simple/platform.vmdl" );

			var direction = Rand.Float( 0, 360 );

			NextPlatform.Scale = scale;
			var distance = Rand.Float( 160 * SpawnPlatform.Scale, 190 * SpawnPlatform.Scale );

			var rotation = Rotation.FromAxis( Vector3.Up, direction );
			var position = rotation.Forward * distance;

			NextPlatform.Rotation = rotation;



			NextPlatform.Position = SpawnPlatform.Position + position;

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
