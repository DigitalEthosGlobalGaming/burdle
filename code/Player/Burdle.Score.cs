using Sandbox;
using System;
using System.Collections.Generic;

namespace Burdle
{
	public partial class BurdleEntity: ModelEntity
	{
		public void SetScore(float amount)
		{
			GetMinigame<MinigameBase>().SetScore(this, amount );
		}

		public float GetScore()
		{
			return GetMinigame<MinigameBase>().GetScore( this );
		}
		public float AddScore(float amount)
		{
			return GetMinigame<MinigameBase>().AddScore( this, amount );
		}
	}
}
