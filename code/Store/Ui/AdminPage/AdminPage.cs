using Degg.Ui.Elements;
using Degg.UI.Elements;
using Degg.Util;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace Burdle
{
	public class AdminPage : PlayerPanel<BurdlePlayer>
	{
		List<AdminButton> Items { get; set; }

		public AdminPage()
		{
			SetTemplate( "/Store/Ui/HatsPage.html" );
			StyleSheet.Load( "/Store/Ui/HatsPage.scss" );
			Items = new List<AdminButton>();
			SetupItems();


		}

		public void SetupItems()
		{
			DeleteChildren( true );

			Items = new List<AdminButton>();

			var commands = new Dictionary<string, string>();
			commands.Add( "Restart Current Game", "restart" );

			foreach(var i in MinigameController.GetGamesList())
			{
				commands.Add( $"Play {i}", $"game {i}" );
			}

			foreach ( var kv in commands )
			{
				var item = AddChild<AdminButton>();
				item.SetData( kv.Key, kv.Value );
				Items.Add( item );

			}
		}

	}
}
