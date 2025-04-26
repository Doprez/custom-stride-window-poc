using Avalonia.Controls;
using Avalonia.Interactivity;
using MyGame3.Avalonia.Stride;
using Stride.Core;
using Stride.Games;
using System;
using System.Threading;

namespace MyGame3.Avalonia.Views;
public partial class MainWindow : Window
{
	private GamePlatformAvalonia gamePlatform;
	private GameBase game;

	private CancellationTokenSource _cancellationTokenSource;

    public MainWindow(CancellationTokenSource cancellationTokenSource)
	{
		InitializeComponent();

		Loaded += OnLoaded;
		Closed += OnClosed;

        _cancellationTokenSource = cancellationTokenSource;
    }

	private void OnLoaded(object? sender, RoutedEventArgs e)
	{
		gamePlatform = new GamePlatformAvalonia(new ServiceRegistry(), this);
		game = new AvaloniaCustomGame(gamePlatform, this);

		game.Run();
	}

	private void OnClosed(object? sender, EventArgs e)
	{
		game.Exit();
		_cancellationTokenSource.Cancel();
    }
}