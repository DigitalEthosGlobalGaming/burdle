
using Sandbox;

namespace Degg
{
	public partial class AdvGame : Game
	{
		[ClientRpc]
		public static void TriggerClientEvent( string name )
		{
			Log.Info( name );
			Event.Run( name );
		}
	}
}
