using System;

﻿namespace ToyBox
{
	public class GameComponentCollectionEventArgs : EventArgs
	{
		private IGameComponent gameComponent;
		
		public GameComponentCollectionEventArgs(IGameComponent gameComponent)
		{
			this.gameComponent = gameComponent;
		}
		
		public IGameComponent GameComponent
		{
			get
			{
				return gameComponent;
			}
		}
	}
}

