
using Sandbox;
using Sandbox.UI;


namespace Degg.Ui.Elements
{
	public partial class PlayerPanel<T> : Panel where T: Entity
	{

		protected T Player { get; set; }


		public T GetPlayer()
		{
			return Player;
		}
		public A GetPlayer<A>() where A: T
		{
			if (GetPlayer() is A player)
			{
				return player;
			}
			return null;
		}
		public void SetPlayer(T player)
		{
			Player = player;
			OnPlayerSet();
		}
		public virtual void OnPlayerSet()
		{

		}
	}
}
