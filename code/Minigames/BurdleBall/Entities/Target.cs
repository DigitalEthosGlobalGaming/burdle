using Sandbox;

namespace Burdle
{
	public partial class Target : Platform
	{
		public override void Spawn()
		{
			base.Spawn();
			SetModelAndPhysics( "models/citizen_props/beachball.vmdl", PhysicsMotionType.Dynamic );
			Rotation = Rotation.FromAxis( Vector3.Forward, 90f );
		}


	}
}
