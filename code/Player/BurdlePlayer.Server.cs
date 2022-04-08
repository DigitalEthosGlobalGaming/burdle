﻿using Burdle;
using Degg.Cameras;
using Sandbox;
using System;

namespace Burdle
{
	public partial class BurdlePlayer : Sandbox.Player
	{

		public static BurdlePlayer GetCallerBurdlePlayer()
		{
			var client = ConsoleSystem.Caller;
			if ( client == null )
			{
				return null;
			}

			if ( ConsoleSystem.Caller.Pawn is BurdlePlayer player )
			{
				return player;
			}

			return null;
		}
		public static BurdleEntity GetCallerBurdle()
		{
			var client = ConsoleSystem.Caller;
			if ( client == null )
			{
				return null;
			}

			if ( ConsoleSystem.Caller.Pawn is BurdlePlayer player )
			{
				return player.Burdle;
			}

			return null;
		}

		[ServerCmd]
		public static void Jump( float yaw, float power )
		{
			var b = GetCallerBurdle();
			if ( b  == null)
			{
				return;
			}

			var jumpAngle = Angles.AngleVector( new Angles( 0, yaw, 0 ) );
			b.Jump( jumpAngle, power );
		}

	}


}