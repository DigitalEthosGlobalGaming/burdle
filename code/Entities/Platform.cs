using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class Platform: ModelEntity
	{
		public List<Entity> HasTouched { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Always;
			HasTouched = new List<Entity>();
		}
		public void SetModelAndPhysics(string model)
		{
			this.SetModel( model );
			this.SetupPhysicsFromModel( PhysicsMotionType.Static );
		}

		public override void StartTouch( Entity other )
		{
			if (other is BurdleEntity burdle)
			{
				burdle.CanJump = true;
				StartTouch( burdle );
			}
			if ( HasTouched != null )
			{
				if ( !HasTouched.Contains( other ) )
				{
					HasTouched.Add( other );
					FirstTouch( other );
				}
			}
		}

		public virtual void StartTouch( BurdleEntity other )
		{

		}
		public virtual void FirstTouch(Entity other)
		{
			if (other is BurdleEntity ent)
			{
				FirstTouch( ent );
			}
		}

		public virtual void FirstTouch( BurdleEntity other )
		{
		}

		public T GetGame<T>() where T: MinigameBase
		{
			return (T) BurdleGame.CurrentGame.Minigames.Current;
		}
	}
}
