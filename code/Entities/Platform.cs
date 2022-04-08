using Sandbox;


namespace Burdle
{
	public partial class Platform: ModelEntity
	{
		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Always;
		}
		public void SetModelAndPhysics(string model)
		{
			this.SetModel( model );
			this.SetupPhysicsFromModel( PhysicsMotionType.Static );
		}

		public override void StartTouch( Entity other )
		{
			if (other is BurdleEntity burdle)
			{
				burdle.CanJump = true;
			}
		}
	}
}
