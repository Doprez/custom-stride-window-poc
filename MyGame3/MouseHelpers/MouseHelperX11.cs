using Stride.Core.Mathematics;
using System;
using System.Runtime.InteropServices;

namespace MyGame3.MouseHelpers;
public static class MouseHelperX11
{
	[DllImport("libX11")]
	private static extern IntPtr XOpenDisplay(IntPtr display);

	[DllImport("libX11")]
	private static extern int XWarpPointer(IntPtr display, IntPtr src_w, IntPtr dest_w, int src_x, int src_y, uint src_width, uint src_height, int dest_x, int dest_y);

	[DllImport("libX11")]
	private static extern bool XQueryPointer(IntPtr display, IntPtr window,
	out IntPtr root_return, out IntPtr child_return,
	out int root_x_return, out int root_y_return,
	out int win_x_return, out int win_y_return,
	out uint mask_return);

	[DllImport("libX11")]
	private static extern IntPtr XDefaultRootWindow(IntPtr display);

	[DllImport("libX11")]
	private static extern int XFlush(IntPtr display);

	public static void SetCursorPosition(int x, int y)
	{
		IntPtr display = XOpenDisplay(IntPtr.Zero);
		XWarpPointer(display, IntPtr.Zero, IntPtr.Zero, 0, 0, 0, 0, x, y);
		XFlush(display);
	}

	public static Point GetCursorPosition()
	{
		IntPtr display = XOpenDisplay(IntPtr.Zero);

		IntPtr rootWindow = XDefaultRootWindow(display);
		IntPtr root_return, child_return;
		int root_x, root_y, win_x, win_y;
		uint mask_return;

		bool result = XQueryPointer(display, rootWindow,
			out root_return, out child_return,
			out root_x, out root_y,
			out win_x, out win_y,
			out mask_return);

		if (result)
		{
			return new Point(root_x, root_y);
		}
		else
		{
			throw new InvalidOperationException("Could not get cursor position.");
		}
	}
}
