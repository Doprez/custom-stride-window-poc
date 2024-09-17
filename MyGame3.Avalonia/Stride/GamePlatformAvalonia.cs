using System;
using MyGame3.Avalonia.Stride;
using Stride.Graphics;

namespace Stride.Games
{
	public class GamePlatformAvalonia : GamePlatform
	{

		private bool hasExitRan = false;

		public GamePlatformAvalonia()
			: base()
		{

		}

		public override string DefaultAppDirectory => AppDomain.CurrentDomain.BaseDirectory;

		public override GameWindow CreateWindow(GameContext gameContext)
		{
			if(gameContext is GameContextAvalonia avaloniaContext)
			{
				return new GameWindowAvalonia(avaloniaContext.Control);
			}

			throw new ArgumentException("GameContext must be of type GameContextAvalonia");
		}

		public override GameWindow GetSupportedGameWindow(AppContextType contextType)
		{
			// I dont currently care about the context type but I might in the future
			return null;
		}

		public new void Run(GameContext gameContext)
		{
			IsBlockingRun = !gameContext.IsUserManagingRun;

			gameWindow = CreateWindow(gameContext);

			// Register event handlers
			gameWindow.Activated += OnActivated;
			gameWindow.Deactivated += OnDeactivated;
			gameWindow.InitCallback = OnInitCallback;
			gameWindow.RunCallback = OnRunCallback;
			//gameWindow.ExitCallback = OnExiting;

			//WindowCreated?.Invoke(this, EventArgs.Empty);

			// Start the game window
			gameWindow.Run();
		}

		public override GraphicsDevice CreateDevice(GraphicsDeviceInformation deviceInformation)
		{
			var graphicsDevice = GraphicsDevice.New(deviceInformation.Adapter, deviceInformation.DeviceCreationFlags, gameWindow.NativeWindow, deviceInformation.GraphicsProfile);
			graphicsDevice.ColorSpace = deviceInformation.PresentationParameters.ColorSpace;
			graphicsDevice.Presenter = new SwapChainGraphicsPresenter(graphicsDevice, deviceInformation.PresentationParameters);

			return graphicsDevice;
		}

		public override void RecreateDevice(GraphicsDevice currentDevice, GraphicsDeviceInformation deviceInformation)
		{
			currentDevice.ColorSpace = deviceInformation.PresentationParameters.ColorSpace;
			currentDevice.Recreate(deviceInformation.Adapter ?? GraphicsAdapterFactory.Default, new[] { deviceInformation.GraphicsProfile }, deviceInformation.DeviceCreationFlags, gameWindow.NativeWindow);
		}

		public override void DeviceChanged(GraphicsDevice currentDevice, GraphicsDeviceInformation deviceInformation)
		{
			// Resize the game window if necessary
			gameWindow.Resize(deviceInformation.PresentationParameters.BackBufferWidth, deviceInformation.PresentationParameters.BackBufferHeight);
		}

		public override GraphicsDevice ChangeOrCreateDevice(GraphicsDevice currentDevice, GraphicsDeviceInformation deviceInformation)
		{
			if (currentDevice == null)
			{
				currentDevice = CreateDevice(deviceInformation);
			}
			else
			{
				RecreateDevice(currentDevice, deviceInformation);
			}

			DeviceChanged(currentDevice, deviceInformation);

			return currentDevice;
		}

		private void OnRunCallback()
		{
			try
			{
				Tick();
			}
			catch (Exception e)
			{
				game.Exit();
			}
		}

		private void OnInitCallback()
		{
			try
			{
				game.InitializeBeforeRun();
			}
			catch (Exception e)
			{
				game.Exit();
			}
		}

		private void Tick()
		{
			game.Tick();

			if (!IsBlockingRun && game.IsExiting && !hasExitRan)
			{
				hasExitRan = true;
				OnExiting(this, EventArgs.Empty);
			}
		}

		protected override void Destroy()
		{
			base.Destroy();
		}
	}
}
