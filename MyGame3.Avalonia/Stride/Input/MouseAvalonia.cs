using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

        public MouseAvalonia(InputSourceAvalonia source, Control uiControl)
        {
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

		}

        public override void UnlockPosition()
        {

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
                // Calculate delta movement
                var deltaX = (float)(position.X - previousPosition.X);
                var deltaY = (float)(position.Y - previousPosition.Y);
                MouseState.HandleMouseDelta(new Vector2(deltaX, deltaY));

                // Update previous position
                previousPosition = new Point((int)position.X, (int)position.Y);

                // Note: Resetting the cursor position is not possible in Avalonia without platform-specific code
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

