using Degg.Core;
using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class BurdleStore
	{
		public static List<BurdleStoreItem> GetItemsForSale( DeggPlayer p )
		{
			var items = new List<BurdleStoreItem>();
			items.Add( new BurdleHatStoreItem( "Leather Cap", 1f, "models/citizen_clothes/hat/hat_leathercap.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Tophat", 10f, "models/citizen_clothes/hat/hat.tophat.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Black Beret", 20f, "models/citizen_clothes/hat/hat_beret.black.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Red Beret", 50f, "models/citizen_clothes/hat/hat_beret.red.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Cool Hat", 100f, "models/citizen_clothes/hat/hat_cap.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Better Leather Cap", 1000f, "models/citizen_clothes/hat/hat_leathercapnobadge.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Service Hat", 2000f, "models/citizen_clothes/hat/hat_service.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Police Hat", 5000f, "models/citizen_clothes/hat/hat_uniform.police.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Mamoth Hat", 10000f, "models/citizen_clothes/hat/hat_woolly.vmdl" ) );
			items.Add( new BurdleHatStoreItem( "Wooly Bobble", 25000f, "models/citizen_clothes/hat/hat_woollybobble.vmdl" ) );

			return items;
		}
	}
}
