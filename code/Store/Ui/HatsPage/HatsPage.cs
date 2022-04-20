using Degg.Ui.Elements;

using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public class HatsPage : PlayerPanel<BurdlePlayer>
	{
		List<HatPreview> Items { get; set; }

		public HatsPage()
		{
			SetTemplate( "/Store/Ui/HatsPage/HatsPage.html" );
			StyleSheet.Load( "/Store/Ui/HatsPage/HatsPage.scss" );
			AddClass( "hats-page" );
			Items = new List<HatPreview>();

			SetupItems();


		}

		[Event( "player.items.update")]
		public void SetupItems()
		{
			DeleteChildren( true );

			Items = new List<HatPreview>();

			var player = GetClientPawn<BurdlePlayer>();
			var storeItems = BurdleStore.GetItemsForSale( player );
			
			foreach ( var storeItem in storeItems )
			{
				var isPurchased = player.HasItem( storeItem );
				var item = AddChild<HatPreview>();
				if (storeItem.Type == "hat") { 
					item.SetItem( storeItem, isPurchased );
				}
			}
		}

	}
}
