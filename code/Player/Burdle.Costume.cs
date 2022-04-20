using Sandbox;

namespace Burdle
{
	public partial class BurdleEntity
	{
		[Net]
		public ModelEntity Hat { get; set; }

		public void EquipCostume( BurdleStoreItem item )
		{
			if (item is BurdleHatStoreItem storeHat)
			{
				var hat = new BurdleHat();
				hat.SetFromBurdleHatStoreItem( storeHat );
				EquipHat( hat );
			}
		}
		public void EquipHat(BurdleHat hat)
		{
			UpdatModel();

			if ( Hat.IsValid() )
			{
				Hat.Delete();
			}

			hat.SetParent( this );
			hat.Transmit = TransmitType.Always;
			Hat = hat;
			Hat.Rotation = Rotation;
			Hat.Position = this.Position - (Rotation.Down * -50f);
		}
	}
}
