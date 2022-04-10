using Degg.Util;
using Sandbox;
using System;
using System.Collections.Generic;

namespace Burdle
{
	public partial class BurdleEntity: ModelEntity
	{

		public float JumpPower { get; set; } = 250f;
		public ModelEntity Hat { get; set; }

		[Net]
		public bool CanJump { get; set; }

		public string SoundToPlay { get; set; }
		public float SoundVolume { get; set; }

		public BurdleCharger BurdleUi { get; set; }


		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Always;
			Position = Position + (Vector3.Up * 100f);
			UpdatModel();
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();
			UpdatModel();
		}
		public override void OnClientActive()
		{
			base.OnClientActive();
			UpdatModel();
		}



		public void UpdatModel()
		{
			if ( IsServer )
			{
				Transmit = TransmitType.Always;
				SetModel( "degg/models/monsters/chicken.vmdl" );
				Scale = 0.5f;
				SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
			}
			CreateWorldUi(true);
		}
		public void GiveRandomHat()
		{
			UpdatModel();

			var hatsList = new List<string>();
			hatsList.Add( "models/citizen_clothes/hat/hat_leathercap.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_hardhat.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat.tophat.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_beret.black.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_beret.red.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_cap.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_leathercapnobadge.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_service.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_uniform.police.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_woolly.vmdl" );
			hatsList.Add( "models/citizen_clothes/hat/hat_woollybobble.vmdl" );

			if (Hat.IsValid())
			{
				Hat.Delete();
			}

			var hat = Create<ModelEntity>();
			hat.SetParent( this );
			hat.SetModel( Rand.FromList( hatsList ) );
			hat.Transmit = TransmitType.Always;
			Hat = hat;
			Hat.Rotation = Rotation;
			Hat.Position = this.Position - (Rotation.Down * -50f);
		}
		
		[Event.Tick.Server]
		public void Tick()
		{
			if ( (SoundToPlay ?? "") != "" )
			{

				var sound = PlaySound( SoundToPlay );
				sound.SetPitch( 1f );
				sound.SetVolume( Math.Clamp( SoundVolume,0f,1f ));
				SoundToPlay = "";
			}
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if (IsServer) {
				var distance = Velocity.Distance( Vector3.Zero );
				if ( distance < 5f)
				{
					TryToUpRight();
				}
			}	
		}

		public override void FrameSimulate( Client cl )
		{
			base.FrameSimulate( cl );
			CreateWorldUi();
		}
		public void TryToUpRight()
		{
			var diff = (Vector3.Up - Rotation.Up);
			var distance = diff.Distance( Vector3.Zero );

			if ( distance < 1f)
			{
				return;
			}

			SoundToPlay = "cute.grunt.short";
			SoundVolume = 0.5f;

			Velocity = Vector3.Up * JumpPower;
			ApplyLocalAngularImpulse( diff * JumpPower * 1.5f);

		}

		public BurdlePlayer GetPlayer()
		{
			if (Owner is BurdlePlayer player)
			{
				return player;
			}
			return null;
		}
		public T GetMinigame<T>() where T: MinigameBase
		{
			var player = GetPlayer();
			if (player.Gamemode is T)
			{
				return (T)player.Gamemode;
			}
			return null;
		}

		public virtual void Move()
		{
			Log.Info( "Test" );
		}
		public void Jump(Vector3 direction, float amount)
		{
			if ( !CanJump )
			{
				return;
			}
			
			if (amount > 0.5)
			{
				SoundToPlay = "cute.grunt.long";
				SoundVolume = 1;
			} else if ( amount > 0.1)
			{
				SoundToPlay = "cute.grunt.short";
				SoundVolume = amount;
			}
			CanJump = false;
			var jumpHeight = 2f;
			var force = JumpPower;

			direction = Vector3.Up * jumpHeight + direction;
			Velocity = direction * amount * force;
			ApplyLocalAngularImpulse( Velocity );
		}

		[Event.Hotload]
		public void Hotload()
		{
			if (IsClient)
			{
				CreateWorldUi(true);
			}
		}

		public void CreateWorldUi(bool deleteExisting = false)
		{
			if ( IsClient )
			{
				if ( deleteExisting || BurdleUi == null )
				{
					if ( Owner?.Client == Local.Client )
					{
						BurdleUi?.Delete( true );
						BurdleUi = new BurdleCharger( this );
					}
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			BurdleUi?.Delete();
		}
	}
}
