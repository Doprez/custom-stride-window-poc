﻿using System;
using Silk.NET.SDL;
using Stride.Games;
using Stride.Graphics.SDL;
using Window = MyGame3.SDL.SDLWindow;

namespace MyGame3.SDL.CustomWindowing;

/// <summary>
/// RenderLoop provides a rendering loop infrastructure. See remarks for usage. 
/// </summary>
/// <remarks>
/// Use static <see cref="Run(Window,RenderCallback)"/>  
/// method to directly use a renderloop with a render callback or use your own loop:
/// <code>
/// control.Show();
/// using (var loop = new RenderLoop(control))
/// {
///     while (loop.NextFrame())
///     {
///        // Perform draw operations here.
///     }
/// }
/// </code>
/// Note that the main control can be changed at anytime inside the loop.
/// </remarks>
internal unsafe class SDLMessageLoop : IMessageLoop
{
	private static Sdl SDL = Window.SDL;

	private Window control;
	private bool isControlAlive;
	private bool switchControl;

	/// <summary>
	/// Initializes a new instance of the <see cref="WindowsMessageLoop"/> class.
	/// </summary>
	public SDLMessageLoop(Window control)
	{
		Control = control;
	}

	/// <summary>
	/// Gets or sets the control to associate with the current render loop.
	/// </summary>
	/// <value>The control.</value>
	/// <exception cref="System.InvalidOperationException">Control is already disposed</exception>
	public Window Control
	{
		get
		{
			return control;
		}
		set
		{
			if (control == value) return;

			// Remove any previous control
			if (control != null && !switchControl)
			{
				isControlAlive = false;
				control.Disposed -= ControlDisposed;
			}

			if (value != null && value.IsDisposed)
			{
				throw new InvalidOperationException("Control is already disposed");
			}

			control = value;
			switchControl = true;
		}
	}

	/// <summary>
	/// Calls this method on each frame.
	/// </summary>
	/// <returns><c>true</c> if if the control is still active, <c>false</c> otherwise.</returns>
	/// <exception cref="System.InvalidOperationException">An error occurred </exception>
	public bool NextFrame()
	{
		// Setup new control
		// TODO this is not completely thread-safe. We should use a lock to handle this correctly
		if (switchControl && control != null)
		{
			control.Disposed += ControlDisposed;
			isControlAlive = true;
			switchControl = false;
		}

		if (isControlAlive)
		{
			Event e;
			while (SDL.PollEvent(&e) != 0)
			{
				Application.ProcessEvent(e);
				if ((EventType)e.Type == EventType.Quit)
				{
					isControlAlive = false;
				}
			}
		}

		return isControlAlive || switchControl;
	}

	private void ControlDisposed(object sender, EventArgs e)
	{
		isControlAlive = false;
	}

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose()
	{
		Control = null;
	}

	/// <summary>
	/// Delegate for the rendering loop.
	/// </summary>
	public delegate void RenderCallback();

	/// <summary>
	/// Runs the specified main loop for the specified windows form.
	/// </summary>
	/// <param name="form">The form.</param>
	/// <param name="renderCallback">The rendering callback.</param>
	/// <exception cref="System.ArgumentNullException">form
	/// or
	/// renderCallback</exception>
	public static void Run(Window form, RenderCallback renderCallback)
	{
		if (form == null) throw new ArgumentNullException(nameof(form));
		if (renderCallback == null) throw new ArgumentNullException(nameof(renderCallback));

		form.Show();
		using (var renderLoop = new SDLMessageLoop(form))
		{
			while (renderLoop.NextFrame())
			{
				renderCallback();
			}
		}
	}
}