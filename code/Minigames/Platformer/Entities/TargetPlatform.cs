using Degg.Util;
using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class TargetPlatform: Platform
	{
		public override void FirstTouch(Entity other)
		{
			if (other is BurdleEntity)
			{
				other.Client.AddInt( "score",  1 );
			}
		}
	}
}
