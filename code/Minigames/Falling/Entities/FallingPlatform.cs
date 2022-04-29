
using Degg.Utils;
using Sandbox;

namespace Burdle
{
	public partial class FallingPlatform : Platform, ITickable
	{

		public const float Duration = 5f;
		public float DeleteStartTime { get; set; }
		public int Index { get; set; }
		public TickableCollection ParentCollection { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

		public override void Spawn()
		{
			base.Spawn();
			EnableShadowCasting = false;
		}

		public override void FirstTouch( BurdleEntity entity )
		{
			if ( DeleteStartTime == 0)
			{
				DeleteStartTime = Time.Now;
			}

		}

		public void Tick( float delta, float currentTick )
		{
			if ( DeleteStartTime != 0 )
			{
				var now = Time.Now - DeleteStartTime;
				var percentage = now / Duration;
				RenderColor = Color.Lerp( Color.White, Color.Transparent, percentage );
				if ( percentage > 1 )
				{
					Delete();
				}
			}
		}
	}
}
