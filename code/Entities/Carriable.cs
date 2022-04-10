using Sandbox;

namespace Burdle
{
	public partial class Carriable : BurdleModelEntity
	{
		public BurdleEntity CarriedBy { get; set; }
		public bool CanBeStolen { get; set; }

		public float CanPickupNext { get; set; }

		public void Throw( Vector3 direction )
		{

			Position = Parent.Position + (Vector3.Up * 10f) + (Vector3.Forward * 20f);
			CanPickupNext = Time.Now + 0.5f;
			Parent = null;
			CarriedBy = null;
			this.Velocity = direction;
			ApplyLocalAngularImpulse( Velocity );

		}

		public void Throw(Vector3 direction, float force)
		{
			Throw( direction * force );
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (CarriedBy?.IsValid() ?? false)
			{
				CarriedBy.CarriedItem = null;
			}
		}

		public override void StartTouch( BurdleEntity entity )
		{
			if ( CanPickupNext >= Time.Now)
			{
				return;
			}

			if ( !(CarriedBy?.IsValid() ?? false) ||  (CarriedBy != entity && CanBeStolen))
			{
				CarriedBy = entity;
				Parent = CarriedBy;
				entity.CarriedItem = this;
				Rotation = Parent.Rotation;
				Position = Parent.Position + (Rotation.Up * 5f) + (Rotation.Forward * 10f);
			}
		}

	}
}
