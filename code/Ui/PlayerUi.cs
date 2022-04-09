
using Sandbox;
using Sandbox.UI;


namespace Burdle
{
	public partial class PlayerUi : HudEntity<RootPanel>
	{

		public static PlayerUi Current { get; set; }
		public Panel ScoreUi { get; set; }
		public Panel TimerUi { get; set; }
		public PlayerUi()
		{
			Current = this;
			RootPanel.StyleSheet.Load( "/Degg/Ui/Styles/base.scss" );
			RootPanel.AddChild<ChatBox>();
			Setup();


		}

		[Event.Hotload]
		public void Setup()
		{
			ScoreUi?.Delete( true );
			TimerUi?.Delete( true );
			TimerUi = RootPanel.AddChild<GameTimerUi>();
			ScoreUi = RootPanel.AddChild<PlayerScore>();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			foreach ( var item in Children )
			{
				if ( item.IsValid() )
				{
					item.Delete();
				}
			}
		}
	}
}
