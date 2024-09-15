using Avalonia.Controls;
using Stride.Games;

namespace MyGame3.Avalonia.Stride;
public class GameContextAvalonia : GameContext<Control>
{
	public GameContextAvalonia(Control control, int requestedWidth = 720, int requestedHeight = 480, bool isUserManagingRun = false)
		: base(control, requestedWidth, requestedHeight, isUserManagingRun)
	{
		ContextType = AppContextType.Custom;
	}
}
