using Degg.Util;
using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class TargetPlatform: Platform
	{
		public override void FirstTouch(BurdleEntity entity )
		{
			entity.AddScore( 1 );
		}
	}
}
