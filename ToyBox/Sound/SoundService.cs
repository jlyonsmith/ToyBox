using System;

namespace ToyBox
{
	public class SoundService : ISoundService, IDisposable
	{
		public SoundService(IServiceProvider services)
		{
		}

		public void Attach(SoundEffect soundEffect)
		{
		}

        #region IDisposable implementation

        public void Dispose()
        {
        }

        #endregion
	}
}

