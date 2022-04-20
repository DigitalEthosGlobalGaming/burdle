
using Degg.Ui.Elements;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace Burdle
{
	public partial class PlayerUi : HudEntity<RootPanel>
	{

		public static PlayerUi Current { get; set; }
		public List<Panel> Panels { get; set; }
		public BurdlePlayer Player { get; set; }
		public PlayerUi(BurdlePlayer player )
		{
			Current = this;
			Player = player;
			RootPanel.StyleSheet.Load( "/Degg/Ui/Styles/base.scss" );
			RootPanel.AddChild<ChatBox>();
			Panels = new List<Panel>();
			Setup();
		}

		[Event.Hotload]
		public void Setup()
		{
			if ( Panels != null )
			{
				foreach ( var i in Panels )
				{
					i.Delete( true );
				}
			}

			AddPanel<GameTimerUi>();
			AddPanel<Burdle.Scoreboard>();
			AddPanel<PlayerScore>();
			AddPanel<MoneyContainer>();
			AddPanel<StoreUi>();
		}

		public T AddPanel<T>() where T: Panel, new()
		{
			var newElement =  RootPanel.AddChild<T>();		
			if ( newElement is PlayerPanel<BurdlePlayer> playerPanel)
			{
				playerPanel.SetPlayer( Player );
			}
			Panels.Add( newElement );
			return newElement;
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
