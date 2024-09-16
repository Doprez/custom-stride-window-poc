using Stride.Core.Mathematics;
using System;
using System.Runtime.InteropServices;

namespace MyGame3.MouseHelpers;
public static class MouseHelperWindows
{
    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    private static extern bool ClipCursor(ref RECT lpRect);

    [DllImport("user32.dll")]
    private static extern bool ClipCursor(nint lpRect);

    [DllImport("user32.dll")]
    private static extern bool GetClientRect(nint hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    private static extern bool ClientToScreen(nint hWnd, ref POINT lpPoint);

    [DllImport("user32.dll")]
    private static extern int ShowCursor(bool bShow);

	[DllImport("user32.dll")]
	private static extern bool GetCursorPos(out POINT lpPoint);

	[StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
		public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int left, top, right, bottom;
    }

    public static void SetCursorPosition(int x, int y)
    {
        SetCursorPos(x, y);
    }

	public static Point GetCursorPosition()
	{
		if (GetCursorPos(out POINT point))
		{
			return new Point(point.X, point.Y);
		}
		else
		{
			throw new InvalidOperationException("Could not get cursor position.");
		}
	}

	public static void LockCursor(nint windowHandle)
    {
        RECT rect;
        GetClientRect(windowHandle, out rect);

        POINT ul = new POINT { X = rect.left, Y = rect.top };
        POINT lr = new POINT { X = rect.right, Y = rect.bottom };

        ClientToScreen(windowHandle, ref ul);
        ClientToScreen(windowHandle, ref lr);

        rect.left = ul.X;
        rect.top = ul.Y;
        rect.right = lr.X;
        rect.bottom = lr.Y;

        ClipCursor(ref rect);
    }

    public static void UnlockCursor()
    {
        ClipCursor(nint.Zero);
    }

    public static void HideCursor()
    {
        ShowCursor(false);
    }

    public static void ShowCursor()
    {
        ShowCursor(true);
    }
}
