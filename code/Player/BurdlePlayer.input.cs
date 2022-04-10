using Sandbox;
using System;

namespace Burdle
{
	public partial class BurdlePlayer
	{

		public float JumpPower { get; set; } = 0.0f;
		public float JumpAccelleration { get; set; } = 1f;
		public float LastJumpPower { get; set; } = 0.0f;

		public override void BuildInput( InputBuilder input )
		{
			Host.AssertClient();


			Event.Run( "buildinput", input );
			CameraMode?.BuildInput( input );

			if ( CameraMode  == null)
			{
				return;
			}
			JumpAccelleration = 1f;
			var isJumpButtonPressed = input.Down( InputButton.Attack1 ) || input.Down( InputButton.Jump );

			if ( isJumpButtonPressed )
			{
				float delta = JumpAccelleration * RealTime.Delta;
				JumpPower = Math.Clamp( JumpPower + delta, 0, 1 );
			}

			if ( JumpPower >= 0.01f && !isJumpButtonPressed )
			{
				BurdlePlayer.Jump( CameraMode.Rotation.Yaw(), JumpPower );
				LastJumpPower = JumpPower;
				JumpPower = 0;
			}
		}

	}


}
