using System;

namespace ToyBox
{
	public interface IUpdateable
	{
		void Update(GameTime gameTime);
		event EventHandler<EventArgs> EnabledChanged;
		event EventHandler<EventArgs> UpdateOrderChanged;
		bool Enabled { get; }
		int UpdateOrder { get; }
	}
}
