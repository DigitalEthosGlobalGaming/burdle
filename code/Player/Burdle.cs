using Degg.Util;
using Sandbox;
using System;
using System.Collections.Generic;

namespace Burdle
{
	public partial class BurdleEntity: ModelEntity
	{
		public float JumpPower { get; set; } = 250f;
		public Carriable CarriedItem { get; set; }

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

		public MinigameBase GetMinigame()
		{
			var player = GetPlayer();
			if ( (player?.IsValid() ?? false) && (player?.Gamemode?.IsValid() ?? false) )
			{
				return player?.Gamemode;
			}
			return null;
		}

		public bool CheckCanJump()
		{
			if ( GetMinigame()?.IsGameLoading() ?? true)
			{
				return false;
			}
			return CanJump;
		}

		public void Jump(Vector3 direction, float amount)
		{
			if ( !CheckCanJump() )
			{
				return;
			}

			if (CarriedItem?.IsValid() ?? false)
			{
				if (!CarriedItem.CanJumpWhileHolding)
				{
					return;
				}
			}

			var owner = GetPlayer();
			var saveData = owner.GetSaveData<BurdlePlayerSave>();
			if ( saveData != null )
			{
				saveData.TotalJumps = saveData.TotalJumps + 1;
				saveData.Burds = saveData.Burds + amount;
				saveData.Save();
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

		public void Throw( Vector3 direction, float amount )
		{
			if ( CarriedItem?.IsValid() ?? false )
			{
				if ( amount > 0.5 )
				{
					SoundToPlay = "cute.grunt.long";
					SoundVolume = 1;
				}
				else if ( amount > 0.1 )
				{
					SoundToPlay = "cute.grunt.short";
					SoundVolume = amount;
				}

				var jumpHeight = 2f;
				var force = JumpPower;

				direction = Vector3.Up * jumpHeight + direction;
				var throwForce = (direction * force * amount) + (Velocity / 2);
				CarriedItem.Throw( throwForce );
				CarriedItem = null;
			}
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
