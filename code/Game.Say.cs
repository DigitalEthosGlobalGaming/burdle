
using Degg;
using Degg.Util;
using Sandbox;
using Sandbox.UI;

namespace Burdle
{
	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class BurdleGame
	{
		[ServerCmd( "say" )]
		public static void Say( string message )
		{
			Assert.NotNull( ConsoleSystem.Caller );
			var caller = ClientUtil.GetCallingPawn<BurdlePlayer>();

			if ( !(caller?.IsValid() ?? false)) {
				return;
			}

			if (message.StartsWith("/"))
			{
				var parts = message.Split( " " );
				if (parts.Length > 0)
				{
					switch ( parts[0] )
					{
						case "/game":
							if ( parts.Length > 1 )
							{
								if ( parts[1] == "random" )
								{
									CurrentGame.Minigames.RandomGame();
								}
								else
								{
									CurrentGame.Minigames.StartGame( parts[1] );
								}
							}

							return;
						case "/refresh":
							caller.CreateBurdle();
							return;
						case "/restart":
							CurrentGame.Minigames.RestartGame();
							return;
						default:
							break;
					}
				} else { }
			} else
			{
				// todo - reject more stuff
				if ( message.Contains( '\n' ) || message.Contains( '\r' ) )
					return;

				Log.Info( $"{ConsoleSystem.Caller}: {message}" );
				ChatBox.AddChatEntry( To.Everyone, ConsoleSystem.Caller.Name, message, $"avatar:{ConsoleSystem.Caller.PlayerId}" );
			}

		}
	}

}
