
using Degg;
using Degg.Core;
using Degg.Utils;
using Degg.Websocket;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Burdle
{
	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class BurdleGame : AdvGame
	{

		public static DeggSocket WS { get; set; }
		public static BurdleGame CurrentGame {get;set;}

		public bool HasSetupWebsocket { get; set; } = false;
		[Net]
		public MinigameController Minigames { get; set; }

		[Net]
		public BurdleStore Store { get; set; }
		public BurdleGame()
		{
			Transmit = TransmitType.Always;
			CurrentGame = this;
			Store = new BurdleStore();
			Minigames = Create<MinigameController>();
			if ( !Minigames.Current.IsValid() )
			{
				Minigames.RandomGame();
			}
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			if (WS?.IsConnected() ?? false)
			{
				SyncClient( client );
			}
		}

		public void SyncClient(Client client)
		{
			Log.Info( client.PlayerId);
			new PlayerJoinEvent( client );
			DeggPlayer.LoadAndCreate<BurdlePlayer, BurdlePlayerSave>( client, "burdle_v2", ( BurdlePlayer pawn ) => {
				try
				{
					pawn.JoinGame( Minigames.Current );
				}
				catch ( Exception exception )
				{
					Log.Info( exception );
				}
			} );
		}


		public void OnDeggConnect()
		{
			foreach ( var client in Client.All )
			{
				SyncClient( client );
			}
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );
			new PlayerLeaveEvent( cl );
		}
	
		[Event.Tick]
		public void Tick()
		{
			TickableCollection.Global.Tick();
			if (IsServer)
			{
				if ( HasSetupWebsocket == false )
				{
					HasSetupWebsocket = true;
					SetupWebsocket( "", "", () =>
					{
						this.OnDeggConnect();
					} );
				}
			}
		}
		public static void StartGame<T>() where T: MinigameBase, new()
		{
			CurrentGame.Minigames.StartGame<T>();
		}

		public static List<BurdlePlayer> GetAllPlayers()
		{
			var players = All.OfType<BurdlePlayer>();
			return players.ToList();
		}
	}

}
