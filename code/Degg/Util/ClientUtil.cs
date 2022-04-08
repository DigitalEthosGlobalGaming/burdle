using Sandbox;

namespace Degg
{
	public class ClientUtil
	{
		public static T GetPawn<T>() where T: Pawn
		{
			var client = Local.Client;
			if (client != null && client.Pawn is T pawn)
			{
				return pawn;
			}
			return null;
		}
		public static Pawn GetPawn()
		{
			return GetPawn<Pawn>();
		}
	}
}
