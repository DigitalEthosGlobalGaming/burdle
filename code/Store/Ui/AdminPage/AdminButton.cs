using Degg.Ui.Elements;
using Sandbox.UI;

namespace Burdle
{
	public class AdminButton : PlayerPanel<BurdlePlayer>
	{
		public BurdleStoreItem Item { get; set; }
		public string Command { get; set; }
		public Label CommandName { get; set; }

		public bool IsBought { get; set; }

		public AdminButton()
		{
			SetTemplate( "/Store/Ui/AdminPage/AdminButton.html" );
			StyleSheet.Load( "/Store/Ui/AdminPage/AdminButton.scss" );

		}

		public void SetData(string name, string command)
		{
			CommandName.Text = name;
			Command = command;
		}
	

		protected override void OnClick( MousePanelEvent e )
		{
			base.OnClick( e );
			BurdleGame.Say( $"/{Command}" );
		}
		

	}
}
