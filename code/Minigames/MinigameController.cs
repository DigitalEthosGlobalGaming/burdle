using Sandbox;
using System.Linq;

namespace Burdle
{
	public partial class MinigameController: Entity
	{
		[Net]
		public MinigameBase Current { get; set; }

		
		public MinigameController()
		{

		}
		public T StartGame<T>() where T : MinigameBase, new()
		{
			return (T) StartGame( new T() );
		}

		public void AddAllPlayersToGames()
		{
			if (Current != null)
			{
				var players = All.OfType<BurdlePlayer>();
				foreach ( var player in players )
				{
					player.JoinGame( Current );
				}
			}
		}

		public MinigameBase StartGame( MinigameBase game)
		{
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
