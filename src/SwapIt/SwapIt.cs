using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using PoeHUD.Plugins;
using PoeHUD.Poe.RemoteMemoryObjects;
using SharpDX;

namespace SwapIt
{
    public class SwapIt : BaseSettingsPlugin<SwapItSetting>
    {
        private IngameState _ingameState;

        private bool _key1;
        private bool _key2;

        private static Vector2 _mousePosition;
        private Vector2 _windowOffset;
        private const int Speed = 10;

        public override void Initialise()
        {
            base.Initialise();
            _ingameState = GameController.Game.IngameState;
            _windowOffset = GameController.Window.GetWindowRectangle().TopLeft;
        }

        public override void Render()
        {
            base.Render();
            if (!Settings.Enable)
            {
                return;
            }

            try
            {
                if (WinApi.IsKeyDown(Settings.macro1.Value))
                {
                    _key1 = true;
                    return;
                }

                if (_key1 && !WinApi.IsKeyDown(Settings.macro1.Value))
                {
                    _key1 = false;

                    if (Settings.Record.Value)
                    {
                        _mousePosition = WinApi.GetMousePosition();
                        if (Settings.m1eX.Value > 0
                            && Settings.m1eY.Value > 0
                            && Settings.m1sX.Value > 0
                            && Settings.m1sY.Value > 0)
                        {
                            Settings.m1sX.Value = -1;
                            Settings.m1sY.Value = -1;
                            Settings.m1eX.Value = -1;
                            Settings.m1eY.Value = -1;
                            LogMessage($"RESET m1 point", 5);
                        }

                        if (Settings.m1sX.Value > 0 && Settings.m1sY.Value > 0)
                        {
                            Settings.m1eX.Value = _mousePosition.X;
                            Settings.m1eY.Value = _mousePosition.Y;
                            LogMessage($"End m1 point save:{Settings.m1eX.Value},{Settings.m1eY.Value}", 5);
                            return;
                        }
                        else
                        {
                            Settings.m1sX.Value = _mousePosition.X;
                            Settings.m1sY.Value = _mousePosition.Y;
                            LogMessage($"Start m1 point save: {Settings.m1sX.Value},{Settings.m1sY.Value}", 5);
                            return;
                        }
                    }
                    else
                    {
                        if (Settings.m1eX.Value <= 0
                            || Settings.m1eY.Value <= 0
                            || Settings.m1sX.Value <= 0
                            || Settings.m1sY.Value <= 0)
                        {
                            LogMessage($"Set points using the record!", 8);
                            return;
                        }

                        var latency = (int) _ingameState.CurLatency;
                        if (!_ingameState.IngameUi.InventoryPanel.IsVisible)
                        {
                            WinApi.KeyPress(Settings.InvHotkey.Value);
                        }
                        var mousePosition = WinApi.GetMousePosition();
                        Thread.Sleep(latency);
                        WinApi.SetCursorPosAndLeftClick(new Vector2(Settings.m1sX, Settings.m1sY), Speed + latency);
                        Thread.Sleep(latency);
                        WinApi.SetCursorPosAndLeftClick(new Vector2(Settings.m1eX, Settings.m1eY), Speed + latency);
                        Thread.Sleep(latency);
                        WinApi.SetCursorPosAndLeftClick(new Vector2(Settings.m1sX, Settings.m1sY), Speed + latency);
                        Thread.Sleep(latency);
                        if (_ingameState.IngameUi.InventoryPanel.IsVisible)
                        {
                            WinApi.KeyPress(Settings.InvHotkey.Value);
                        }
                        Thread.Sleep(latency);
                        WinApi.SetCursorPos((int)mousePosition.X, (int)mousePosition.Y);
                    }
                }

                if (WinApi.IsKeyDown(Settings.macro2.Value))
                {
                    _key2 = true;
                    return;
                }

                if (_key2 && !WinApi.IsKeyDown(Settings.macro2.Value))
                {
                    _key2 = false;

                    if (Settings.Record.Value)
                    {
                        _mousePosition = WinApi.GetMousePosition();
                        if (Settings.m2eX.Value > 0
                            && Settings.m2eY.Value > 0
                            && Settings.m2sX.Value > 0
                            && Settings.m2sY.Value > 0)
                        {
                            Settings.m2sX.Value = -1;
                            Settings.m2sY.Value = -1;
                            Settings.m2eX.Value = -1;
                            Settings.m2eY.Value = -1;
                            LogMessage($"RESET m2 point", 5);
                        }

                        if (Settings.m2sX.Value > 0 && Settings.m2sY.Value > 0)
                        {
                            Settings.m2eX.Value = _mousePosition.X;
                            Settings.m2eY.Value = _mousePosition.Y;
                            LogMessage($"End m2 point save:{Settings.m2eX.Value},{Settings.m2eY.Value}", 5);
                            return;
                        }
                        else
                        {
                            Settings.m2sX.Value = _mousePosition.X;
                            Settings.m2sY.Value = _mousePosition.Y;
                            LogMessage($"Start m2 point save: {Settings.m2sX.Value},{Settings.m2sY.Value}", 5);
                            return;
                        }
                    }
                    else
                    {
                        if (Settings.m2eX.Value <= 0
                            || Settings.m2eY.Value <= 0
                            || Settings.m2sX.Value <= 0
                            || Settings.m2sY.Value <= 0)
                        {
                            LogMessage($"Set points using the record!", 8);
                            return;
                        }

                        var latency = (int) _ingameState.CurLatency;
                        if (!_ingameState.IngameUi.InventoryPanel.IsVisible)
                        {
                            WinApi.KeyPress(Settings.InvHotkey.Value);
                        }
                        var mousePosition = WinApi.GetMousePosition();
                        Thread.Sleep(latency);
                        WinApi.SetCursorPosAndLeftClick(new Vector2(Settings.m2sX, Settings.m2sY), Speed + latency);
                        Thread.Sleep(latency);
                        WinApi.SetCursorPosAndLeftClick(new Vector2(Settings.m2eX, Settings.m2eY), Speed + latency);
                        Thread.Sleep(latency);
                        WinApi.SetCursorPosAndLeftClick(new Vector2(Settings.m2sX, Settings.m2sY), Speed + latency);
                        Thread.Sleep(latency);
                        if (_ingameState.IngameUi.InventoryPanel.IsVisible)
                        {
                            WinApi.KeyPress(Settings.InvHotkey.Value);
                        }
                        Thread.Sleep(latency);
                        WinApi.SetCursorPos((int)mousePosition.X, (int)mousePosition.Y);
                    }
                }

                if (Settings.ShowPoint)
                {
                    var start = new Vector2(Settings.m1sX, Settings.m1sY - _windowOffset.Y);
                    var end = new Vector2(Settings.m1eX, Settings.m1eY - _windowOffset.Y);
                    Graphics.DrawLine(start, end, 2, Color.AliceBlue);
                    Graphics.DrawText($"Start 1 ({start.ToString()})", 14, start, Color.AliceBlue);
                    Graphics.DrawText($"End 1 ({end.ToString()})", 14, end, Color.AliceBlue);

                    start = new Vector2(Settings.m2sX, Settings.m2sY - _windowOffset.Y);
                    end = new Vector2(Settings.m2eX, Settings.m2eY - _windowOffset.Y);
                    Graphics.DrawLine(start, end, 2, Color.Cyan);
                    Graphics.DrawText($"Start 2 ({start.ToString()})", 14, start, Color.Cyan);
                    Graphics.DrawText($"End 2 ({end.ToString()})", 14, end, Color.Cyan);
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("log.txt", $"{e.Source} || {e.Message} \n");
                throw;
            }
        }
    }

    #region WinApi

    public static class WinApi
    {
        public const int ClickDelay = 64;

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpMousePoint);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);

        private const int KeyeventfExtendedkey = 0x0001;
        private const int KeyeventfKeyup = 0x0002;

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        #region Structs/Enums

        [Flags]
        private enum MouseEventFlags
        {
            LeftDown = 0x00000002,
            LeftUp = 0x00000004,
            MiddleDown = 0x00000020,
            MiddleUp = 0x00000040,
            Move = 0x00000001,
            Absolute = 0x00008000,
            RightDown = 0x00000008,
            RightUp = 0x00000010
        }

        [Flags]
        public enum KeyEventFlags
        {
            KeyEventShiftVirtual = 0x10,
            KeyLControlVirtual = 0x11,
            KeyLeftVirtual = 0x25,
            KeyRightVirtual = 0x27,
            KeyEventKeyDown = 0,
            KeyEventKeyUp = 2,
        }

        public static void SetCursorPosAndLeftClick(Vector2 pos, int extraDelay)
        {
            SetCursorPos((int) pos.X, (int) pos.Y);
            Thread.Sleep(extraDelay);
            mouse_event((int) MouseEventFlags.LeftDown, 0, 0, 0, 0);
            Thread.Sleep(ClickDelay);
            mouse_event((int) MouseEventFlags.LeftUp, 0, 0, 0, 0);
        }

        public static Vector2 GetMousePosition()
        {
            GetCursorPos(out var mouse);
            return new Vector2(mouse.X, mouse.Y);
        }

        public static void KeyPress(Keys key)
        {
            keybd_event((byte) key, 0, KeyeventfExtendedkey | 0, 0);
            Thread.Sleep(ClickDelay);
            keybd_event((byte) key, 0, KeyeventfExtendedkey | KeyeventfKeyup, 0);
        }

        public static bool IsKeyDown(Keys key)
        {
            return GetKeyState((int) key) < 0;
        }

        #endregion
    }

    #endregion
}