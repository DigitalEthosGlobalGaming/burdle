using Degg.Util;
using Sandbox;
using System.Text.Json;

namespace Burdle
{
	public partial class BurdleHat: ModelEntity
	{
		public void SetFromBurdleHatStoreItem(BurdleStoreItem item)
		{
			var model = item.Get( "model", "models/citizen_clothes/hat/hat_leathercap.vmdl" );
			this.SetModel( model );
		}
	}
}
