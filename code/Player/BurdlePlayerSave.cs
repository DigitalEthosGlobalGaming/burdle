using Degg.Data;
using Sandbox;
using System.Collections.Generic;
using System.Text.Json;

namespace Burdle
{
	public partial class BurdlePlayerSave : DeggPlayerSave
	{
		[Net]
		public float Burds { get; set; }
		[Net]
		public float TotalJumps { get; set; }

		[Net,Change]
		public string ItemsData { get; set; }
		public List<BurdleStoreItem> Items { get; set; }

		public void OnItemsDataChanged(string previous, string next)
		{
			Items = JsonSerializer.Deserialize<List<BurdleStoreItem>>( next );
			Event.Run( "player.items.update" );
		}



		public void AddItem(BurdleStoreItem item)
		{
			var newList = new List<BurdleStoreItem>();
			foreach(var i in Items)
			{
				newList.Add( i );
			}
			newList.Add( item );
			Items = newList;
			ItemsData = JsonSerializer.Serialize<List<BurdleStoreItem>>( Items );
		}

		public List<BurdleStoreItem> GetItems()
		{
			if (Items == null && ItemsData != null)
			{
				Items = JsonSerializer.Deserialize<List<BurdleStoreItem>>( ItemsData );
			}
			return Items;
		}
	}
}
