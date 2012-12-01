using System;
using System.Collections.Generic;

namespace ToyBox
{
	public class GameServiceContainer : IServiceProvider
	{
		Dictionary<Type, object> services;
		
		public GameServiceContainer()
		{
			services = new Dictionary<Type, object>();
		}
		
		public void AddService(Type type, object provider)
		{
			services.Add(type, provider);
		}
		
		public object GetService(Type type)
		{
			object service;

			if (services.TryGetValue(type, out service))
			{
				return service;
			}
			
			return null;
		}
		
		public void RemoveService(Type type)
		{
			services.Remove(type);
		}
	}
}