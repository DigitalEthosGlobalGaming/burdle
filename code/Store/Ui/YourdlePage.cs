using Degg.Ui.Elements;
using Degg.UI.Elements;
using Degg.Util;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace Burdle
{
	public class YourdlePage : PlayerPanel<BurdlePlayer>
	{
		public bool Opened {get;set;}

		public YourdlePage()
		{
			SetTemplate( "/Store/Ui/YourdlePage.html" );
			StyleSheet.Load( "/Store/Ui/YourdlePage.scss" );
			var preview = AddChild<ModelPreview>();
			preview.AddClass( "preview" );
			Log.Info( preview );
		}


	}
}
