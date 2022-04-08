using System;
using System.Collections.Generic;
using Degg.Util;
using Sandbox;

namespace Degg.Utils
{

	public interface ITickable
	{
		public TickableCollection ParentCollection { get; set; }
		public void Tick( float delta, float currentTick );
	}

	public partial class Timer: ITickable
	{
		public float Interval { get; private set; }
		public float NextTick { get; private set; }
		public float LastTick { get; private set; }
		public float CurrentTick { get; private set; }
		public float Delta { get; private set; }
		public bool IsStarted { get; private set; }
		public Action<Timer> Callback { get; private set; }
		public TickableCollection ParentCollection { get; set; }

		public Timer( Action<Timer> callback, float interval )
		{
			TickableCollection.Global.Add( this );
			Interval = interval;
			Callback = callback;
		}

		public void Delete()
		{
			TickableCollection.Global.Remove( this );
		}

		public void Stop()
		{
			IsStarted = false;
		}

		public void Start()
		{
			IsStarted = true;
		}


		public float GetPercentage(float currentTick = -1)
		{
			if (currentTick < 0)
			{
				currentTick = Time.Tick;
			}

			currentTick = currentTick - LastTick;
			var max = NextTick - LastTick;

			if (max <= 0)
			{
				return 0;
			}

			if (currentTick <= 0)
			{
				return 0;
			}

			return currentTick / max;
		}

		public void Tick(float delta, float currentTick )
		{
			if ( Callback != null && IsStarted )
			{
				if ( currentTick >= NextTick && currentTick != LastTick )
				{
					CurrentTick = Time.Tick;
					LastTick = NextTick;
					NextTick = currentTick + (Interval / 1000);
					Delta = currentTick - LastTick;
					Callback(this);
				}
			}
		}
	}
	public partial class TickableCollection
	{
		public static TickableCollection Global = new TickableCollection();
		public List<ITickable> Items { get; set; } = new List<ITickable>();

		[Event.Tick]
		public static void GlobalTick()
		{
			Global.Tick();
		}
		public void Add(ITickable item)
		{						
			item.ParentCollection = this;
			Items.Add( item );
		}

		public void Remove(ITickable item )
		{
			if ( item != null )
			{
				Items.Remove( item );
				item.ParentCollection = null;
			}
		}

		public void Tick()
		{
			if ( Items != null )
			{
				var itemsToTick = new List<ITickable>( Items );
				foreach (var item in itemsToTick )
				{
					if ( item != null )
					{
						item.Tick( Time.Delta, Time.Now );
					}
				}
			}
		}

	}
}
