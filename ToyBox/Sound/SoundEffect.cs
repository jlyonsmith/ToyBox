using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToyBox;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ToyBox
{
	public class SoundEffect : IDisposable
	{
		public bool IsLooped
		{
			get;
			set;
		}

		public void Stop()
		{
			throw new NotImplementedException();
		}

		public void Play()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
