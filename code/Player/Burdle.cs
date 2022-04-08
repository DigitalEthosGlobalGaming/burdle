using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Burdle
{
	public partial class BurdleEntity: ModelEntity
	{

		public float JumpPower { get; set; } = 250f;
		public bool CanJump { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			Position = Position + (Vector3.Up * 100f);
			Transmit = TransmitType.Always;
			SetModel( "degg/monsters/chicken.vmdl" );
			Scale = 0.5f;
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if (IsServer) {
				var distance = Velocity.Distance( Vector3.Zero );
				if ( distance < 5f)
				{
					TryToUpRight();
				}
			}			
		}
		public void TryToUpRight()
		{
			var diff = (Vector3.Up - Rotation.Up);
			var distance = diff.Distance( Vector3.Zero );

			if ( distance < 1f)
			{
				return;
			}
			Velocity = Vector3.Up * JumpPower;
			ApplyLocalAngularImpulse( diff * JumpPower * 1.5f);

		}

		public virtual void Move()
		{
			Log.Info( "Test" );
		}
		public void Jump(Vector3 direction, float amount)
		{
			if ( !CanJump )
			{
				return;
			}
			CanJump = false;
			var jumpHeight = 2f;
			var force = JumpPower;

			direction = Vector3.Up * jumpHeight + direction;
			Velocity = direction * amount * force;
			ApplyLocalAngularImpulse( Velocity );
		}
	}
}
