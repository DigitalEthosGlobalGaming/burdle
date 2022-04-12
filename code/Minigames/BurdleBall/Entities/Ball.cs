using Sandbox;
using System;

namespace Burdle
{
	public partial class Ball : Carriable
	{


		public float MaxSpeed { get; set; } = 1000.0f;
		public float SpeedMul { get; set; } = 0.95f;

		public override void Spawn()
		{
			base.Spawn();
			CanJumpWhileHolding = false;
			SetModelAndPhysics( "models/citizen_props/beachball.vmdl", PhysicsMotionType.Dynamic );
			PhysicsBody.SetSurface( "rubber" );
		}

		protected override void OnPhysicsCollision( CollisionEventData eventData )
		{
			var speed = eventData.PreVelocity.Length;
			var direction = Vector3.Reflect( eventData.PreVelocity.Normal, eventData.Normal.Normal ).Normal;
			Velocity = direction * MathF.Min( speed * SpeedMul, MaxSpeed );
		}
	}
}
