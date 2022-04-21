using Sandbox;

namespace Burdle
{
	public class MinigameRound: Entity
	{
		public MinigameBase Minigame { get; set; }

		public bool SpawnBurdle { get; set; }
		public bool IsLoading { get; set; }
		public float Duration { get; set; }
		public float StartTime { get; set; }
		public float EndTime { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			SpawnBurdle = true;
			IsLoading = false;
			Duration = -1;
			StartTime = -1;
			EndTime = -1;
		}

		public virtual void Tick()
		{
			if ( IsOver())
			{
				Minigame?.NextRound();
			}
		}

		public virtual void OnPlayerSpawn( BurdlePlayer player )
		{
		}

		public virtual void Join( BurdlePlayer player )
		{
			
		}

		public virtual void Leave( BurdlePlayer player )
		{
			
		}

		public virtual bool IsOver()
		{
			if (Duration == -1)
			{
				return false;
			}
			return (Time.Now > EndTime);
		}

		public virtual void Start()
		{
			if ( Duration > 0 )
			{
				StartTime = Time.Now;
				EndTime = Time.Now + Duration;
				return ;
			}
		}
		public virtual void End()
		{
			if ( Minigame?.IsValid() ?? false )
			{
				foreach ( var kv in Minigame.Players )
				{
					End( kv.Value );
				}
			}
		}
		public virtual void End( BurdlePlayer player )
		{

		}
	}
}
