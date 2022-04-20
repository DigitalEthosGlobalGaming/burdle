using Degg.Ui.Elements;
using Sandbox.UI;

namespace Burdle
{
	public class HatPreview : PlayerPanel<BurdlePlayer>
	{
		public BurdleStoreItem Item { get; set; }

		public Label HatPrice { get; set; }
		public Label HatName { get; set; }

		public bool IsBought { get; set; }

		public HatPreview()
		{
			SetTemplate( "/Store/Ui/HatsPage/HatPreview.html" );
			StyleSheet.Load( "/Store/Ui/HatsPage/HatPreview.scss" );
		}

		public string GetEquippedHat()
		{
			return null;
		}

		protected override void OnClick( MousePanelEvent e )
		{
			base.OnClick( e );
			
			var player = GetClientPawn<BurdlePlayer>();
			if ( IsBought )
			{
				player.EquipItem(Item);
			} else
			{
				player.BuyItem( Item );
			}
		}
		public void SetItem( BurdleStoreItem item, bool purchased)
		{
			IsBought = purchased;

			if ( IsBought )
			{
				AddClass( "bought" );
				HatPrice.Text = $"Bought";
			} else
			{
				HatPrice.Text = $"${item.Value}";
			}
			Item = item;

			HatName.Text = item.Name;
		}

	}
}
