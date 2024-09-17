using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using MyGame3.MouseHelpers;
using Silk.NET.SDL;
using Stride.Core.Mathematics;
using Stride.Input;
using MouseButton = Stride.Input.MouseButton;
using Point = Stride.Core.Mathematics.Point;


namespace MyGame3.Avalonia.Input
{
    internal class MouseAvalonia : MouseDeviceBase, IDisposable
    {
        private readonly Control uiControl;
        private bool isMousePositionLocked;
        private Point previousPosition;
        private Sdl sdl;

		private Point relativeCapturedPosition;

		public MouseAvalonia(InputSourceAvalonia source, Control uiControl, Sdl sdl)
        {
            this.sdl = sdl;

			Source = source;
            this.uiControl = uiControl;

            // Subscribe to mouse events
            uiControl.PointerMoved += OnPointerMoved;
            uiControl.PointerPressed += OnPointerPressed;
            uiControl.PointerReleased += OnPointerReleased;
            uiControl.PointerWheelChanged += OnPointerWheelChanged;
            uiControl.SizeChanged += OnSizeChanged;

            // Initialize surface size
            OnSizeChanged(null, null);

            Id = InputDeviceUtils.DeviceNameToGuid(uiControl.Name ?? uiControl.ToString() + Name);
        }

        public override string Name => "Avalonia Mouse";

        public override Guid Id { get; }

        public override bool IsPositionLocked => isMousePositionLocked;

        public override IInputSource Source { get; }

        public void Dispose()
        {
            uiControl.PointerMoved -= OnPointerMoved;
            uiControl.PointerPressed -= OnPointerPressed;
            uiControl.PointerReleased -= OnPointerReleased;
            uiControl.PointerWheelChanged -= OnPointerWheelChanged;
            uiControl.SizeChanged -= OnSizeChanged;
        }

        public override void LockPosition(bool forceCenter = false)
		{
			if (!IsPositionLocked)
			{
				if (forceCenter)
				{
					relativeCapturedPosition = new Point((int)uiControl.Bounds.Width / 2, (int)uiControl.Bounds.Height / 2);
				}
				else
				{
                    
					relativeCapturedPosition = MouseHelper.GetCursorPosition();
				}

				isMousePositionLocked = true;
			}
		}

        public override void UnlockPosition()
		{
			if (IsPositionLocked)
			{
                MouseHelper.SetCursorPosition(relativeCapturedPosition.X, relativeCapturedPosition.Y);
				isMousePositionLocked = false;
				relativeCapturedPosition = Point.Zero;
			}
		}

        public override void SetPosition(Vector2 normalizedPosition)
        {

        }

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetSurfaceSize(new Vector2((float)uiControl.Bounds.Width, (float)uiControl.Bounds.Height));
        }

        private void OnPointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            MouseState.HandleMouseWheel((float)e.Delta.Y);
        }

        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var updateKind = e.GetCurrentPoint(uiControl).Properties.PointerUpdateKind;
            var button = ConvertMouseButton(updateKind);

            MouseState.HandleButtonDown(button);
        }

        private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            var updateKind = e.GetCurrentPoint(uiControl).Properties.PointerUpdateKind;
            var button = ConvertMouseButton(updateKind);

            MouseState.HandleButtonUp(button);
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            var position = e.GetCurrentPoint(uiControl).Position;

            if (IsPositionLocked)
			{
				var currentRelativePosition = MouseHelper.GetCursorPosition();
				var delta = new Vector2(currentRelativePosition.X - relativeCapturedPosition.X, currentRelativePosition.Y - relativeCapturedPosition.Y);
				MouseState.HandleMouseDelta(delta);
				MouseHelper.SetCursorPosition(relativeCapturedPosition.X, relativeCapturedPosition.Y);
			}
            else
            {
                // Update mouse position
                MouseState.HandleMove(new Vector2((float)position.X, (float)position.Y));
            }

            previousPosition = new Point((int)position.X, (int)position.Y);
        }

        private MouseButton ConvertMouseButton(PointerUpdateKind updateKind)
        {
            switch (updateKind)
            {
                case PointerUpdateKind.LeftButtonPressed:
                case PointerUpdateKind.LeftButtonReleased:
                    return MouseButton.Left;
                case PointerUpdateKind.RightButtonPressed:
                case PointerUpdateKind.RightButtonReleased:
                    return MouseButton.Right;
                case PointerUpdateKind.MiddleButtonPressed:
                case PointerUpdateKind.MiddleButtonReleased:
                    return MouseButton.Middle;
                case PointerUpdateKind.XButton1Pressed:
                case PointerUpdateKind.XButton1Released:
                    return MouseButton.Extended1;
                case PointerUpdateKind.XButton2Pressed:
                case PointerUpdateKind.XButton2Released:
                    return MouseButton.Extended2;
                default:
                    return MouseButton.Left; // TODO: Check if this will cause issues
			}
        }
    }
}

