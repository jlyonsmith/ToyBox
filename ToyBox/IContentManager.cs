using System;

namespace ToyBox
{
	public interface IContentManager
	{
		T Load<T>(string contentName);
		void Unload();
	}
}

