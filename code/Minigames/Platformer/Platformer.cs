using Degg.Util;
using Sandbox;

namespace Burdle
{
	public partial class Platformer: MinigameBase
	{

		public Platform SpawnPlatform { get; set; }
		public Platform NextPlatform { get; set; }

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
			PlatformSpawnTime = 3f;
			End();
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

		public void CreatePlatforms()
		{
			var scale = Rand.Float( 0.2f, 0.75f );
			if (!SpawnPlatform.IsValid())
			{
				SpawnPlatform = Create<Platform>();
				SpawnPlatform.Position = Vector3.Up * 1000f;
				SpawnPlatform.SetModelAndPhysics( "models/room.vmdl" );
				SpawnPlatform.Scale = scale;
			} else { 
				SpawnPlatform.Delete();
				SpawnPlatform = NextPlatform;
			}

			NextPlatform = new Platform();
			NextPlatform.SetModelAndPhysics( "models/room.vmdl" );

			var direction = Rand.Float( 0, 360 );

			NextPlatform.Scale = scale;
			var distance = Rand.Float( 500 * SpawnPlatform.Scale, 750 * SpawnPlatform.Scale );

			var rotation = Rotation.FromAxis( Vector3.Up, direction );
			var position = rotation.Forward * distance;

			NextPlatform.Rotation = rotation;



			NextPlatform.Position = SpawnPlatform.Position + position;

			NextPlatformSpawn = Time.Now + 5f;

			foreach ( var kv in Players )
			{
				var player = kv.Value;
				if ( player.Position.z < (SpawnPlatform.Position.z - 100f) )
				{
					SpawnPlayer( player );
				} else
				{
					player.Client.SetInt( "score", player.Client.GetInt("score", 0) + 1 );
				}
			}
		}

		[Event.Tick.Server]
		public void Tick()
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

			SpawnPlatform.RenderColor = Color.Lerp(Color.Red, Color.White, percentage);



		}
	}
}
