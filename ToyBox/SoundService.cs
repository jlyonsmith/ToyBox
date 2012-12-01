using System;

namespace ToyBox
{
	public class SoundService : GameComponent, ISoundService
	{
		public SoundService(Game game) : base(game)
		{
		}

		public void Attach(SoundEffect winApplauseSound)
		{
		}
	}
}

