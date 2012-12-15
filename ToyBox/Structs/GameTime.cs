using System;

namespace ToyBox
{
	public class GameTime
	{
		private TimeSpan elapsedTime;
		private TimeSpan totalTime;
        private bool isRunningSlowly;
		
		public TimeSpan TimeSinceLastUpdate
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
		
		public GameTime (TimeSpan totalGameTime, TimeSpan timeSinceLastUpdate, bool isRunningSlowly)
		{
			totalTime = totalGameTime;
			elapsedTime = timeSinceLastUpdate;
			isRunningSlowly = isRunningSlowly;
		}
	}
}

