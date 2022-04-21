using Degg.Cameras;
using Sandbox;

namespace Burdle
{
	public class GameEndRound: MinigameRound
	{
		public float LoadingFocusDistance { get; set; } = 100f;

		public override void Spawn()
		{
			base.Spawn();
			SpawnBurdle = false;
			IsLoading = true;
			Duration = 1f;
		}
		public override void Join( BurdlePlayer player )
		{
			base.Join( player );
			player.DisabledControls = true;
			var camera = player.SetCamera<OrbitCamera>();
			camera.TargetPosition = Minigame.GamemodeCenter;
			camera.Distance = LoadingFocusDistance;
			camera.Height = LoadingFocusDistance;
			camera.OrbitSpeed = 0.5f;
		}

		public override void End( BurdlePlayer player )
		{
			player.SetCamera<FollowCamera>();
		}
	}
}
