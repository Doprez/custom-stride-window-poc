using SharpHook;
using Stride.Input;

namespace MyGame3.CustomInput;
public class SharpHookInputSource : InputSourceBase
{
	TaskPoolGlobalHook _hook;

	KeyboardSharpHook _keyboard;

	public SharpHookInputSource(TaskPoolGlobalHook hook)
	{
		_hook = hook;
	}

	public override void Initialize(InputManager inputManager)
	{
		// Create the keyboard device
		_keyboard = new KeyboardSharpHook(this, _hook);
		RegisterDevice(_keyboard);
	}
}
