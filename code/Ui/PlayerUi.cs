
using Sandbox;
using Sandbox.UI;


namespace Burdle
{
	public partial class PlayerUi : HudEntity<RootPanel>
	{

		public Panel HudPanel { get; set; }
		public PlayerUi()
		{
			RootPanel.StyleSheet.Load( "/Degg/Ui/Styles/base.scss" );
			RootPanel.AddChild<ChatBox>();
			SetHudPanel<PlayerScore>();
		}

		public virtual T SetHudPanel<T>() where T : Panel, new()
		{
			HudPanel?.Delete( true );
			HudPanel = RootPanel.AddChild<T>();
			return (T)HudPanel;
		}
	}
}
