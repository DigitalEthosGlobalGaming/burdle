using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Burdle
{
	public partial class MinigameController: Entity
	{
		[Net]
		public MinigameBase Current { get; set; }

		public string CurrentName { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Always;
		}
		public T StartGame<T>() where T : MinigameBase, new()
		{
			return (T) StartGame( new T() );
		}

		public MinigameBase RestartGame()
		{
			return StartGame( CurrentName );
		}

		public void RandomGame(int count = 0)
		{
			if ( count >= 100)
			{
				throw new System.Exception( "Error starting game couldn't find a random game to start in 100 goes." );
			}
			var games = new List<string>();
			games.Add( "Racer" );
			games.Add( "Platformer" );
			games.Add( "AreaControl" );
			var game = Rand.FromList( games );
			var newGame = StartGame( game );
			if ( newGame  == null)
			{
				RandomGame( count + 1 );
			}
		}
		public MinigameBase StartGame(string game)
		{
			var newGame = Library.Create<MinigameBase>( game );
			CurrentName = game;
			return StartGame( newGame );
		}

		public void AddAllPlayersToGames()
		{
			if (Current != null)
			{
				var players = All.OfType<BurdlePlayer>();
				foreach ( var player in players.ToList() )
				{
					Log.Info( player );
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
			if (!game.CanStart())
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
