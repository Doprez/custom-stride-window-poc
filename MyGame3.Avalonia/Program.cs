using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using MyGame3.Avalonia.Views;
using System;
using System.Threading;

namespace MyGame3.Avalonia;

internal class Program
{
	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static void Main(string[] args) => BuildAvaloniaApp()
		.Start(AppMain, args);

	// Avalonia configuration, don't remove; also used by visual designer.
	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace()
			.UseReactiveUI();

	// Application entry point. Avalonia is completely initialized.
	static void AppMain(Application app, string[] args)
	{
		// A cancellation token source that will be 
		// used to stop the main loop
		var cts = new CancellationTokenSource();

		// Do your startup code here
		new MainWindow().Show();

		// Start the main loop
		Dispatcher.UIThread.MainLoop(cts.Token);
	}
}
