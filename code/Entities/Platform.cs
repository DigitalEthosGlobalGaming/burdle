using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class Platform: BurdleModelEntity
	{
		public override void Spawn()
		{
			base.Spawn();
			SetModelAndPhysics( "degg/models/simple/platform.vmdl" );
		}
	}
}
