using Avalonia;
using Avalonia.Markup.Xaml;

namespace MyGame3.Avalonia;
public partial class App : Application
{
	public override void Initialize()
	{
		AvaloniaXamlLoader.Load(this);
	}
}