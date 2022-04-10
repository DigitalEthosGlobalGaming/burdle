using Sandbox;

namespace Burdle
{
	public partial class Ball : Carriable
	{
		public override void Spawn()
		{
			base.Spawn();
			SetModelAndPhysics( "models/citizen_props/beachball.vmdl", PhysicsMotionType.Dynamic );
		}
	}
}
