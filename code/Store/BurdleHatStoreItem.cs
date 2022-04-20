
using Sandbox;

namespace Burdle
{
	public partial class BurdleHatStoreItem: BurdleStoreItem
	{
		public BurdleHatStoreItem() : base() { }
		public BurdleHatStoreItem( string name, float value, string model ): base("hat",name,value)
		{
			Name = name;
			Value = value;
			Set( "model", model );
		}
	}
}
