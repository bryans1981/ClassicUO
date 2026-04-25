// SPDX-License-Identifier: BSD-2-Clause

// Browser builds use a separate SDL-free partial implementation.
#if !BROWSER_WASM
using System;
using ClassicUO.Utility.Logging;
using Microsoft.Xna.Framework;
using SDL3;
using ClassicUO.Utility.Platforms;

namespace ClassicUO.Input
{
    internal static partial class Mouse
    {
        public const int MOUSE_DELAY_DOUBLE_CLICK = 350;

        /* Log a button press event at the given time. */
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

#if !BROWSER_WASM
            if (!PlatformHelper.IsBrowser)
            {
                SDL.SDL_CaptureMouse(true);
            }
#endif
        }

        /* Log a button release event at the given time */
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

#if !BROWSER_WASM
            if (!(LButtonPressed || RButtonPressed || MButtonPressed) && !PlatformHelper.IsBrowser)
            {
                SDL.SDL_CaptureMouse(false);
            }
#endif
        }

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

        public static Point LDragOffset => LButtonPressed ? Position - LClickPosition : Point.Zero;

        public static Point RDragOffset => RButtonPressed ? Position - RClickPosition : Point.Zero;

        public static Point MDragOffset => MButtonPressed ? Position - MClickPosition : Point.Zero;

        public static bool MouseInWindow { get; set; }

        private static Point _rawPosition;

        public static void SetPosition(int x, int y)
        {
            _rawPosition.X = x;
            _rawPosition.Y = y;

            if (PlatformHelper.IsBrowser)
            {
                Position = _rawPosition;
            }
        }

        public static void Update()
        {
#if BROWSER_WASM
            if (!_browserUpdateLogged)
            {
                _browserUpdateLogged = true;
                Log.Trace($"Mouse.Update browser branch active: {PlatformHelper.IsBrowser}");
            }
            Position = _rawPosition;
            IsDragging = LButtonPressed || RButtonPressed || MButtonPressed;
            return;
#else
            if (PlatformHelper.IsBrowser)
            {
                if (!_browserUpdateLogged)
                {
                    _browserUpdateLogged = true;
                    Log.Trace($"Mouse.Update browser runtime branch active: {PlatformHelper.IsBrowser}");
                }
                Position = _rawPosition;
                IsDragging = LButtonPressed || RButtonPressed || MButtonPressed;
                return;
            }

            if (!MouseInWindow)
            {
                SDL.SDL_GetGlobalMouseState(out float x, out float y);
                SDL.SDL_GetWindowPosition(Client.Game.Window.Handle, out int winX, out int winY);
                _rawPosition.X = (int)x - winX;
                _rawPosition.Y = (int)y - winY;
            }
            else
            {
                SDL.SDL_GetMouseState(out float x, out float y);
                _rawPosition.X = (int)x;
                _rawPosition.Y = (int)y;
            }

            // Scale the mouse coordinates for the faux-backbuffer and DPI settings
            int clientWidth = Math.Max(1, Client.Game.Window.ClientBounds.Width);
            int clientHeight = Math.Max(1, Client.Game.Window.ClientBounds.Height);
            Position.X = (int) ((double) _rawPosition.X * (Client.Game.GraphicManager.PreferredBackBufferWidth / clientWidth) / Client.Game.DpiScale);

            Position.Y = (int) ((double) _rawPosition.Y * (Client.Game.GraphicManager.PreferredBackBufferHeight / clientHeight) / Client.Game.DpiScale);

            IsDragging = LButtonPressed || RButtonPressed || MButtonPressed;
#endif
        }

        private static bool _browserUpdateLogged;
    }
}
#endif
