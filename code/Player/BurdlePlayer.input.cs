using Sandbox;
using System;

namespace Burdle
{
	public partial class BurdlePlayer
	{

		public float JumpPower { get; set; } = 0.0f;

		public enum ThrowType
		{
			Jump,
			Throw
		}

		public ThrowType Action { get; set; }
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
			var isThrowButtonPressed = input.Down( InputButton.Attack2 ) || input.Down( InputButton.Attack2 );
			var isActionOccuring = false;

			if (isJumpButtonPressed || isThrowButtonPressed)
			{
				float delta = JumpAccelleration * RealTime.Delta;
				JumpPower = Math.Clamp( JumpPower + delta, 0, 1 );
				isActionOccuring = true;
			}

			if ( isJumpButtonPressed )
			{
				Action = ThrowType.Jump;
			} else if ( isThrowButtonPressed )
			{
				Action = ThrowType.Throw;
			}

			if ( JumpPower >= 0.01f && !(isActionOccuring) )
			{
				if ( Action == ThrowType.Jump )
				{
					BurdlePlayer.Jump( CameraMode.Rotation.Yaw(), JumpPower );
				} else if (Action == ThrowType.Throw)
				{
					BurdlePlayer.Throw( CameraMode.Rotation.Yaw(), JumpPower );
				}
				LastJumpPower = JumpPower;
				JumpPower = 0;
			}
		}

	}


}
