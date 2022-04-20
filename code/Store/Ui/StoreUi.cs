using Degg.Ui.Elements;
using Degg.UI.Elements;
using Degg.Util;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace Burdle
{
	public class StoreUi : PlayerPanel<BurdlePlayer>
	{
		NavPanel Nav { get; set; }

		public bool Opened {get;set;}

		public StoreUi()
		{
			SetTemplate( "/Store/Ui/StoreUi.html" );
			StyleSheet.Load( "/Store/Ui/StoreUi.scss" );

			AddClass( "store-ui" );
			Opened = false;
		}


		public void OpenMenu()
		{
			Player.SetControlsDisabled( true );
			Opened = true;
			Nav?.Delete(true);
			Nav = AddChild<NavPanel>();
			Nav.AddPage<HatsPage>( "Hats" );
		}

		public void CloseMenu()
		{
			Player.SetControlsDisabled( false );
			Opened = false;
		}

		[Event.PreRender]
		public void Prerender()
		{
			if ( AdvInput.Pressed( InputButton.Slot1, InputButton.Slot1 ) )
			{
				Opened = !Opened;

				if ( Opened )
				{
					OpenMenu();
				}
				else
				{
					CloseMenu();
				}
			}

			SetClass( "open", Opened );
		}

	}
}
