
using Degg.Utils;
using Sandbox;

namespace Burdle
{
	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class BurdleGame : Game
	{
		public static BurdleGame CurrentGame {get;set;}
		[Net]
		public MinigameController Minigames { get; set; }
		public BurdleGame()
		{
			CurrentGame = this;
			Minigames = Create<MinigameController>();

		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );
			var pawn = new BurdlePlayer();

			client.Pawn = pawn;

			if(!Minigames.Current.IsValid())
			{
				Minigames.RandomGame();
			}

			pawn.JoinGame( Minigames.Current );
		}

		[Event.Tick]
		public void Tick()
		{
			TickableCollection.Global.Tick();
		}
		public static void StartGame<T>() where T: MinigameBase, new()
		{
			CurrentGame.Minigames.StartGame<T>();
		}
	}

}
