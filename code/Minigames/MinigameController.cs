using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Burdle
{
	public partial class MinigameController: Entity
	{
		[Net]
		public MinigameBase Current { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Always;
		}
		public T StartGame<T>() where T : MinigameBase, new()
		{
			return (T) StartGame( new T() );
		}

		public void RandomGame()
		{
			var games = new List<string>();
			games.Add( "Racer" );
			games.Add( "Platformer" );
			var game = Rand.FromList( games );
			StartGame( game );
		}
		public MinigameBase StartGame(string game)
		{
			var newGame = Library.Create<MinigameBase>( game );
			return StartGame( newGame );
		}

		public void AddAllPlayersToGames()
		{
			if (Current != null)
			{
				var players = All.OfType<BurdlePlayer>();
				foreach ( var player in players.ToList() )
				{
					player.JoinGame( Current );
				}
			}
		}

		public MinigameBase StartGame( MinigameBase game)
		{
			if ( !(game?.IsValid() ?? false))
			{
				return null;
			}
			if (Current.IsValid())
			{
				Current.Delete();
			}

			Current = game;

			Current.Start();
			AddAllPlayersToGames();
			Current.SpawnAllPlayers();

			return game;
		}
	}
}
