﻿
using Sandbox;
using System;

namespace Degg.Cameras {
	public class FollowCamera : CameraMode
	{
		// should only need TargetRotation but I'm shit
		public Angles TargetAngles;
		Rotation TargetRotation;

		private float Distance = 150.0f;
		private float TargetDistance = 150.0f;

		public float MinDistance => 100.0f;
		public float MaxDistance => 300.0f;
		public float DistanceStep => 10.0f;

		public override void Build( ref CameraSetup camSetup )
		{
			base.Build( ref camSetup );
			camSetup.Position = Position;
			camSetup.Rotation = Rotation;
		}

		public override void Update()
		{
			if ( !Entity.IsValid() ) return;

			Position = Entity.Position + Vector3.Up * (24 + (Entity.Scale));
			TargetRotation = Rotation.From( TargetAngles );

			Rotation = Rotation.Slerp( Rotation, TargetRotation, RealTime.Delta * 10.0f );
			TargetDistance = TargetDistance.LerpTo( Distance, RealTime.Delta * 5.0f );
			Position += Rotation.Backward * TargetDistance;

			FieldOfView = 80.0f;
		}

		public override void BuildInput( InputBuilder input )
		{

			Distance = Math.Clamp( Distance + (-input.MouseWheel * DistanceStep), MinDistance, MaxDistance );

			TargetAngles.yaw += input.AnalogLook.yaw;

			if ( !input.Down( InputButton.Attack1 ) )
				TargetAngles.pitch += input.AnalogLook.pitch;

			TargetAngles = TargetAngles.Normal;

			if ( !input.Down( InputButton.Attack1 ) )
				TargetAngles.pitch = TargetAngles.pitch.Clamp( 0, 89 );
		}
	}

}