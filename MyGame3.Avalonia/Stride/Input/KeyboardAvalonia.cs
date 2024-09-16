using Avalonia.Input;
using Avalonia.Interactivity;
using Stride.Input;
using System.Collections.Generic;
using System;
using Avalonia.Controls;
using MyGame3.Avalonia.Input;

internal class KeyboardAvalonia : KeyboardDeviceBase, ITextInputDevice, IDisposable
{
	private readonly Control control;
	private readonly List<TextInputEvent> textEvents = new List<TextInputEvent>();

	public KeyboardAvalonia(InputSourceAvalonia source, Control control)
	{
		Source = source;
		this.control = control;

		// Subscribe to keyboard events
		this.control.AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
		this.control.AddHandler(InputElement.KeyUpEvent, OnKeyUp, RoutingStrategies.Tunnel);
		this.control.AddHandler(InputElement.TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);

		Id = InputDeviceUtils.DeviceNameToGuid(control.Name ?? control.ToString());
	}

	public void Dispose()
	{
		// Unsubscribe from keyboard events
		this.control.RemoveHandler(InputElement.KeyDownEvent, OnKeyDown);
		this.control.RemoveHandler(InputElement.KeyUpEvent, OnKeyUp);
		this.control.RemoveHandler(InputElement.TextInputEvent, OnTextInput);
	}

	public override string Name => "Avalonia Keyboard";

	public override Guid Id { get; }

	public override IInputSource Source { get; }

	public override void Update(List<InputEvent> inputEvents)
	{
		base.Update(inputEvents);

		inputEvents.AddRange(textEvents);
		textEvents.Clear();
	}

	public void EnabledTextInput()
	{
		// Avalonia handles text input automatically; no action needed
	}

	public void DisableTextInput()
	{
		// Avalonia handles text input automatically; no action needed
	}

	private void OnKeyDown(object sender, KeyEventArgs e)
	{
		var key = MapKey(e.Key);
		if (key != Keys.None)
		{
			HandleKeyDown(key);
		}

		// Optional: Mark event as handled to prevent further processing
		// e.Handled = true;
	}

	private void OnKeyUp(object sender, KeyEventArgs e)
	{
		var key = MapKey(e.Key);
		if (key != Keys.None)
		{
			HandleKeyUp(key);
		}

		// Optional: Mark event as handled to prevent further processing
		// e.Handled = true;
	}

	private void OnTextInput(object sender, TextInputEventArgs e)
	{
		var textInputEvent = InputEventPool<TextInputEvent>.GetOrCreate(this);
		textInputEvent.Text = e.Text;
		textInputEvent.Type = TextInputEventType.Input;
		textEvents.Add(textInputEvent);

		// Optional: Mark event as handled to prevent further processing
		// e.Handled = true;
	}

	/// <summary>
	/// Maps Avalonia's <see cref="Key"/> to Stride's <see cref="Keys"/>.
	/// </summary>
	private static Keys MapKey(Key input)
	{
		// Map Avalonia Key to Stride Keys
		switch (input)
		{
			case Key.None: return Keys.None;
			case Key.Cancel: return Keys.Cancel;
			case Key.Back: return Keys.Back;
			case Key.Tab: return Keys.Tab;
			case Key.Clear: return Keys.Clear;
			case Key.Return: return Keys.Enter;
			case Key.Pause: return Keys.Pause;
			case Key.CapsLock: return Keys.CapsLock;
			case Key.Escape: return Keys.Escape;
			case Key.Space: return Keys.Space;
			case Key.PageUp: return Keys.PageUp;
			case Key.PageDown: return Keys.PageDown;
			case Key.End: return Keys.End;
			case Key.Home: return Keys.Home;
			case Key.Left: return Keys.Left;
			case Key.Up: return Keys.Up;
			case Key.Right: return Keys.Right;
			case Key.Down: return Keys.Down;
			case Key.Select: return Keys.Select;
			case Key.Print: return Keys.Print;
			case Key.Execute: return Keys.Execute;
			case Key.Snapshot: return Keys.PrintScreen;
			case Key.Insert: return Keys.Insert;
			case Key.Delete: return Keys.Delete;
			case Key.Help: return Keys.Help;
			case Key.D0: return Keys.D0;
			case Key.D1: return Keys.D1;
			case Key.D2: return Keys.D2;
			case Key.D3: return Keys.D3;
			case Key.D4: return Keys.D4;
			case Key.D5: return Keys.D5;
			case Key.D6: return Keys.D6;
			case Key.D7: return Keys.D7;
			case Key.D8: return Keys.D8;
			case Key.D9: return Keys.D9;
			case Key.A: return Keys.A;
			case Key.B: return Keys.B;
			case Key.C: return Keys.C;
			case Key.D: return Keys.D;
			case Key.E: return Keys.E;
			case Key.F: return Keys.F;
			case Key.G: return Keys.G;
			case Key.H: return Keys.H;
			case Key.I: return Keys.I;
			case Key.J: return Keys.J;
			case Key.K: return Keys.K;
			case Key.L: return Keys.L;
			case Key.M: return Keys.M;
			case Key.N: return Keys.N;
			case Key.O: return Keys.O;
			case Key.P: return Keys.P;
			case Key.Q: return Keys.Q;
			case Key.R: return Keys.R;
			case Key.S: return Keys.S;
			case Key.T: return Keys.T;
			case Key.U: return Keys.U;
			case Key.V: return Keys.V;
			case Key.W: return Keys.W;
			case Key.X: return Keys.X;
			case Key.Y: return Keys.Y;
			case Key.Z: return Keys.Z;
			case Key.LWin: return Keys.LeftWin;
			case Key.RWin: return Keys.RightWin;
			case Key.Apps: return Keys.Apps;
			case Key.Sleep: return Keys.Sleep;
			case Key.NumPad0: return Keys.NumPad0;
			case Key.NumPad1: return Keys.NumPad1;
			case Key.NumPad2: return Keys.NumPad2;
			case Key.NumPad3: return Keys.NumPad3;
			case Key.NumPad4: return Keys.NumPad4;
			case Key.NumPad5: return Keys.NumPad5;
			case Key.NumPad6: return Keys.NumPad6;
			case Key.NumPad7: return Keys.NumPad7;
			case Key.NumPad8: return Keys.NumPad8;
			case Key.NumPad9: return Keys.NumPad9;
			case Key.Multiply: return Keys.Multiply;
			case Key.Add: return Keys.Add;
			case Key.Separator: return Keys.Separator;
			case Key.Subtract: return Keys.Subtract;
			case Key.Decimal: return Keys.Decimal;
			case Key.Divide: return Keys.Divide;
			case Key.F1: return Keys.F1;
			case Key.F2: return Keys.F2;
			case Key.F3: return Keys.F3;
			case Key.F4: return Keys.F4;
			case Key.F5: return Keys.F5;
			case Key.F6: return Keys.F6;
			case Key.F7: return Keys.F7;
			case Key.F8: return Keys.F8;
			case Key.F9: return Keys.F9;
			case Key.F10: return Keys.F10;
			case Key.F11: return Keys.F11;
			case Key.F12: return Keys.F12;
			case Key.NumLock: return Keys.NumLock;
			case Key.Scroll: return Keys.Scroll;
			case Key.LeftShift: return Keys.LeftShift;
			case Key.RightShift: return Keys.RightShift;
			case Key.LeftCtrl: return Keys.LeftCtrl;
			case Key.RightCtrl: return Keys.RightCtrl;
			case Key.LeftAlt: return Keys.LeftAlt;
			case Key.RightAlt: return Keys.RightAlt;
			case Key.OemPlus: return Keys.OemPlus;
			case Key.OemComma: return Keys.OemComma;
			case Key.OemMinus: return Keys.OemMinus;
			case Key.OemPeriod: return Keys.OemPeriod;
			case Key.OemQuestion: return Keys.OemQuestion;
			case Key.OemTilde: return Keys.OemTilde;
			case Key.OemOpenBrackets: return Keys.OemOpenBrackets;
			case Key.OemPipe: return Keys.OemPipe;
			case Key.OemCloseBrackets: return Keys.OemCloseBrackets;
			case Key.OemQuotes: return Keys.OemQuotes;
			case Key.OemBackslash: return Keys.OemBackslash;
			// Add more key mappings as needed
			default: return Keys.None;
		}
	}
}
