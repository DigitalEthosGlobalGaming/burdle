using Degg.Cameras;
using Sandbox;

namespace Burdle
{
	public class GameStartRound: MinigameRound
	{
		public CinematicScene Transition { get; set; }

		public CinematicScene GetScene()
		{
			return Minigame?.LoadingScene;
		}

		public override void Tick()
		{
			base.Tick();
			var scene = GetScene();
			if ( scene?.IsValid() ?? false )
			{

				GetScene().Tick();
			}
		}
		public override void Spawn()
		{
			base.Spawn();
			SpawnBurdle = false;
			IsLoading = true;
			Duration = 30f;
		}

		public override void Start()
		{
			base.Start();
			var scene = GetScene();
			if ( scene?.IsValid() ?? false )
			{
				scene.Start();

			}
		}
		public override void Join( BurdlePlayer player )
		{
			base.Join( player );
			player.SetCamera<CinematicCamera>();

			player.DisabledControls = true;

			var scene = GetScene();
			if ( scene?.IsValid() ?? false )
			{
				scene.AddPlayer( player );
			}
		}

		public override void End( BurdlePlayer player )
		{
			player.SetCamera<FollowCamera>();
			var scene = GetScene();
			if ( scene?.IsValid() ?? false )
			{
				scene.RemovePlayer( player );
			}
		}
	}
}
