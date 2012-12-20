using System;

namespace ToyBox
{
	public enum PlatformType
	{
		Unknown,
		Windows,
		WinPhone7,
		iPhone3,
		iPhone4,
		iPad2,
		iPad3
	}
	
    public delegate void UpdateDelegate(GameTime gameTime);
    public delegate void RenderDelegate();

    public interface IPlatformService
	{
		PlatformType Platform { get; }
        Size AdBannerSize { get; }
		void SendMail(string to, string title, string description, Action whenDismissed);
		void Exit();
        event UpdateDelegate Update;
        event RenderDelegate Draw;
    }
}
