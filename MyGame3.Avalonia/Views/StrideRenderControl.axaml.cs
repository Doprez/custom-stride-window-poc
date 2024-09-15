using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MyGame3.Avalonia.Stride;
using Stride.Games;
using System;

namespace MyGame3.Avalonia;
public partial class StrideRenderControl : UserControl
{

	private GamePlatformAvalonia gamePlatform;
	private GameWindow<Control> gameWindow;
	private AvaloniaGame game;

	public StrideRenderControl()
	{
		Initialized += OnInitialized;
		DetachedFromVisualTree += OnDetachedFromVisualTree;
		Loaded += OnLoaded;
	}

	private void OnLoaded(object? sender, RoutedEventArgs e)
	{
		gameWindow = new GameWindowAvalonia(this);

		game = new AvaloniaGame();

		game.Run(gameWindow.GameContext);
	}

	private void OnInitialized(object sender, EventArgs e)
	{

	}

	private void OnDetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
	{

	}
}