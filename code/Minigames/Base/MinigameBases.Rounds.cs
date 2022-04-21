using Degg;
using Degg.Util;
using Sandbox;
using System.Collections.Generic;

namespace Burdle
{
	public partial class MinigameBase
	{
		[Net]
		public int CurrentRoundIndex { get; set; }

		public MinigameRound CurrentRound { get; set; }

		public GameStartRound WarmupRound { get; set; }

		public GameEndRound WarmdownRound { get; set; }

		public List<MinigameRound> Rounds { get; set; }

		public float LoadingFocusDistance { get; set; }

		public T GetRound<T>() where T: MinigameRound
		{
			var rounds = GetRound();
			if (rounds is T round)
			{
				return round;
			}
			return null;
		}
		public MinigameRound GetRound()
		{
			if ( Rounds == null)
			{
				Rounds = new List<MinigameRound>();
			}
			var maxRoundIndex = Rounds.Count;
			if ( maxRoundIndex == 0)
			{
				return null;
			}
			if ( CurrentRoundIndex > maxRoundIndex || CurrentRoundIndex < 1)
			{
				return null;
			}
			return Rounds[CurrentRoundIndex - 1];
		}

		public void DeleteAllRounds()
		{
			if (Rounds != null)
			{
				foreach(var round in Rounds)
				{
					round.Delete();
				}
			}
			Rounds = new List<MinigameRound>();
		}

		public void NextRound()
		{
			if (CurrentRound != null)
			{
				CurrentRound.End();
				OnRoundEnd( CurrentRound );
			}
			CurrentRoundIndex = CurrentRoundIndex + 1;
			var round = GetRound();
			CurrentRound = round;
			if (round == null)
			{
				End();
			} else
			{
				OnRoundStart( CurrentRound );
				round.Start();
			}

			GameStartTime = round?.StartTime ?? -1;
			GameDuration = round?.Duration ?? -1;
			GameEndTime = round?.EndTime ?? -1;
		}

		public T AddRound<T>()where T: MinigameRound, new()
		{
			var newRound = new T();
			AddRound( newRound );
			return newRound;
		}

		public T AddRoundToStart<T>() where T : MinigameRound, new()
		{
			var newRound = new T();
			AddRoundToStart( newRound );
			return newRound;
		}

		public MinigameRound AddRoundToStart( MinigameRound r )
		{
			if ( Rounds == null )
			{
				Rounds = new List<MinigameRound>();
			}
			r.Minigame = this;
			Rounds.Insert( 0, r );
			return r;
		}

		public MinigameRound AddRound( MinigameRound r)
		{
			if ( Rounds == null)
			{
				Rounds = new List<MinigameRound>();
			}
			r.Minigame = this;
			Rounds.Add(r);
			return r;
		}

		public virtual void SetupRounds()
		{
			CurrentRoundIndex = 0;
			if (Rounds == null)
			{
				Rounds = new List<MinigameRound>();
			}
			WarmupRound = AddRoundToStart<GameStartRound>();

			WarmdownRound = AddRound<GameEndRound>();
			WarmdownRound.LoadingFocusDistance = LoadingFocusDistance;
		}

		public bool IsGameLoading()
		{
			return CurrentRound?.IsLoading ?? false;
		}


		public virtual void OnRoundStart(MinigameRound r)
		{

		}

		public virtual void OnRoundEnd( MinigameRound r )
		{

		}




	}
}
