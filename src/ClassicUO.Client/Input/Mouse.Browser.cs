// SPDX-License-Identifier: BSD-2-Clause

#if BROWSER_WASM
using Microsoft.Xna.Framework;

namespace ClassicUO.Input
{
    internal static partial class Mouse
    {
        public const int MOUSE_DELAY_DOUBLE_CLICK = 350;

        public static Point Position;
        public static Point LClickPosition;
        public static Point RClickPosition;
        public static Point MClickPosition;

        public static uint LastLeftButtonClickTime { get; set; }
        public static uint LastMidButtonClickTime { get; set; }
        public static uint LastRightButtonClickTime { get; set; }

        public static bool CancelDoubleClick { get; set; }
        public static bool LButtonPressed { get; set; }
        public static bool RButtonPressed { get; set; }
        public static bool MButtonPressed { get; set; }
        public static bool XButtonPressed { get; set; }
        public static bool IsDragging { get; set; }
        public static bool MouseInWindow { get; set; }

        public static Point LDragOffset => LButtonPressed ? Position - LClickPosition : Point.Zero;
        public static Point RDragOffset => RButtonPressed ? Position - RClickPosition : Point.Zero;
        public static Point MDragOffset => MButtonPressed ? Position - MClickPosition : Point.Zero;

        private static Point _rawPosition;

        public static void ButtonPress(MouseButtonType type)
        {
            CancelDoubleClick = false;

            switch (type)
            {
                case MouseButtonType.Left:
                    LButtonPressed = true;
                    LClickPosition = Position;
                    break;
                case MouseButtonType.Middle:
                    MButtonPressed = true;
                    MClickPosition = Position;
                    break;
                case MouseButtonType.Right:
                    RButtonPressed = true;
                    RClickPosition = Position;
                    break;
                case MouseButtonType.XButton1:
                case MouseButtonType.XButton2:
                    XButtonPressed = true;
                    break;
            }
        }

        public static void ButtonRelease(MouseButtonType type)
        {
            switch (type)
            {
                case MouseButtonType.Left:
                    LButtonPressed = false;
                    break;
                case MouseButtonType.Middle:
                    MButtonPressed = false;
                    break;
                case MouseButtonType.Right:
                    RButtonPressed = false;
                    break;
                case MouseButtonType.XButton1:
                case MouseButtonType.XButton2:
                    XButtonPressed = false;
                    break;
            }
        }

        public static void SetPosition(int x, int y)
        {
            _rawPosition.X = x;
            _rawPosition.Y = y;
            Position = _rawPosition;
        }

        public static void Update()
        {
            Position = _rawPosition;
            IsDragging = LButtonPressed || RButtonPressed || MButtonPressed;
        }
    }
}
#endif
