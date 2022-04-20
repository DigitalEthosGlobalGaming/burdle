
using Degg;
using Degg.Util;
using Degg.Websocket;
using Sandbox;
using Sandbox.UI;
using System;

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
		[Event.Hotload]
		public void SetupWebsocket(string username = "", string password = "", Action callback = null)
		{
			if ( IsServer )
			{
				if ( WS == null )
				{
					WS = new Degg.Websocket.DeggSocket( "ws://sbox.de-gg.com" );
			
				}
				if ( username != "" && password != "" ) {
					WS.SetCredentials( username, password );
				} else if (username == "" && password == "")
				{
					WS.SetCredentials( "degg-public", "password" );
				}
				WS.Reconnect(() => {
					if ( callback != null )
					{
						callback();
					}
				},
				() => {
					if ( callback != null )
					{
						callback();
					}
				} );

				
			}
		}
	}

}
