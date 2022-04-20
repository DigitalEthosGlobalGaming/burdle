using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class BurdlePlayer
	{
		public void BuyItem( BurdleStoreItem item )
		{
			if ( IsClient )
			{
				var data = item.Serialize();
				BuyItemCmd( data );
			}
			else
			{
				var saveData = GetSaveData<BurdlePlayerSave>();
				if ( HasItem(item))
				{
					Log.Info( "Already has item" );
					return;
				}
				if (saveData.Burds >= item.Value)
				{
					saveData.Burds = saveData.Burds - item.Value;
					saveData.Dirty = true;
					GiveItem( item );
				}
			}
		}

		public bool HasItem( BurdleStoreItem item )
		{
			var saveData = GetSaveData<BurdlePlayerSave>();
			if ( saveData != null )
			{
				var existingItem = saveData.GetItems().Find( ( existingItem ) =>
				{
					return existingItem.Name == item.Name && existingItem.Type == item.Type;
				} );

				return existingItem != null;
			}
			return false;
		}

		public void GiveItem(BurdleStoreItem item)
		{
			var saveData = GetSaveData<BurdlePlayerSave>();
			saveData.AddItem(item);

			switch ( item.Type )
			{
				case "hat":
					break;			
			}

			saveData.Save();
		}

		public void EquipItem(BurdleStoreItem item)
		{
			if ( IsClient )
			{
				var data = item.Serialize();
				EquipItemCmd( data );
			} else
			{
				var burdle = GetBurdle();
				

				switch ( item.Type )
				{
					case "hat":
						var hat = new BurdleHat();
						hat.SetFromBurdleHatStoreItem( item );
						burdle.EquipHat( hat );
						break;
				}
			}
		}

		[ServerCmd]
		public static void BuyItemCmd(string data)
		{
			var item = BurdleStoreItem.Deserialize( data );

			var player = GetCallerBurdlePlayer();
			if ( player?.IsValid() ?? false )
			{
				player.BuyItem( item );
				return;
			}
			
		}

		[ServerCmd]
		public static void EquipItemCmd( string data )
		{
			var item = BurdleStoreItem.Deserialize( data );
			var player = GetCallerBurdlePlayer();
			if ( player?.IsValid() ?? false )
			{
				player.EquipItem( item );
				return;
			}

		}

		public float GiveBurds( float amount)
		{
			var saveData = GetSaveData<BurdlePlayerSave>();
			if ( saveData != null )
			{
				saveData.TotalJumps = saveData.TotalJumps + 1;
				saveData.Burds = saveData.Burds + amount;
				saveData.Save();
				return saveData.Burds;
			}
			return 0;
		}

	}


}
