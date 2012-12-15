using System;

namespace ToyBox
{
	public class GameTime
	{
		TimeSpan elapsedTime;
		TimeSpan totalTime;
		
		public bool IsRunningSlowly { get; set; }
		
		public TimeSpan ElapsedGameTime
		{
			get
			{
				return elapsedTime;
			}
			internal set
			{
				elapsedTime = value;
			}
		}
		
		public TimeSpan ElapsedRealTime
		{
			get
			{
				return elapsedTime;
			}
		}
		
		public TimeSpan TotalGameTime
		{
			get
			{
				return totalTime;
			}
		}
		
		public TimeSpan TotalRealTime
		{
			get
			{
				return totalTime;
			}
		}
		
		public GameTime()
		{
			elapsedTime = totalTime = TimeSpan.Zero;
		}
		
		public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
		{
			totalTime = totalGameTime;
			elapsedTime = elapsedGameTime;
		}
		
		public GameTime (TimeSpan totalRealTime, TimeSpan elapsedRealTime, bool isRunningSlowly)
		{
			totalTime = totalRealTime;
			elapsedTime = elapsedRealTime;
			IsRunningSlowly = isRunningSlowly;
		}
		
		internal void Update(TimeSpan elapsed)
		{
			elapsedTime = elapsed;
			totalTime += elapsed;
		}				
		
		internal void ResetElapsedTime()
		{
			elapsedTime = TimeSpan.Zero;
		}
	}
}

