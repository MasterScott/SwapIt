using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using ImGuiNET;
using PoeHUD.Hud.Menu;
using PoeHUD.Plugins;
using PoeHUD.Poe.RemoteMemoryObjects;
using SharpDX;

namespace SwapIt
{
    public class SwapIt : BaseSettingsPlugin<SwapItSetting>
    {
        private const int Speed = 100;

        private IngameState _ingameState;

        private bool _run;
        private Vector2 _windowOffset;

        public override void Initialise()
        {
            base.Initialise();
            _ingameState = GameController.Game.IngameState;
            _windowOffset = GameController.Window.GetWindowRectangle().TopLeft;
            MenuPlugin.KeyboardMouseEvents.MouseDownExt += KeyboardMouseEvents_MouseDownExt;
        }

        public override void DrawSettingsMenu()
        {
            base.DrawSettingsMenu();
            if (ImGui.Button("Clear Mouse Clicks"))
                Settings.CustomMouseClicks.Clear();
        }

        private void KeyboardMouseEvents_MouseDownExt(object sender, MouseEventExtArgs e)
        {
            if (!Settings.Enable)
                return;
            if (!Settings.Record)
                return;
            if (WinApi.IsKeyDown(Settings.AdditKey1.Value) 
                || WinApi.IsKeyDown(Settings.AdditKey2.Value)
                || WinApi.IsKeyDown(Settings.StartSwap.Value))
            {
                return;
            }
            List<Tuple<int, int, bool>> currentTupple = Settings.CustomMouseClicks;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    currentTupple.Add(new Tuple<int, int, bool>(e.X, e.Y, true));
                    break;
                case MouseButtons.Right:
                    currentTupple.Add(new Tuple<int, int, bool>(e.X, e.Y, false));
                    break;
            }

            foreach (var tuple in currentTupple)
                LogMessage($"X:{tuple.Item1} Y:{tuple.Item2} Left:{tuple.Item3}", 1);
        }

        public override void Render()
        {
            base.Render();
            if (!Settings.Enable)
                return;
            try
            {
                if (WinApi.IsKeyDown(Settings.StartSwap.Value)
                    && WinApi.IsKeyDown(Settings.AdditKey1.Value)
                    && WinApi.IsKeyDown(Settings.AdditKey2.Value))
                {
                    _run = true;
                    return;
                }

                if (_run && !WinApi.IsKeyDown(Settings.StartSwap.Value))
                {
                    _run = false;
                    DoCustomMouseClicks();
                }

                if (Settings.ShowPoint)
                    DrawCustomLines();
            }
            catch (Exception e)
            {
                File.AppendAllText("log.txt", $"{e.Source} || {e.Message} \n");
                throw;
            }
        }

        private void DrawCustomLines()
        {
            var colorList = GetGradients(Color.Red, Color.Lime, Settings.CustomMouseClicks.Count);
            for (var i = 0; i < Settings.CustomMouseClicks.Count; i++)
            {
                if (i >= Settings.CustomMouseClicks.Count - 1)
                    break;
                Color[] enumerable = colorList as Color[] ?? colorList.ToArray();
                switch (Settings.CustomMouseClicks[i].Item3)
                {
                    case true:
                        Graphics.DrawLine(new Vector2(Settings.CustomMouseClicks[i].Item1, Settings.CustomMouseClicks[i].Item2),
                            new Vector2(Settings.CustomMouseClicks[i + 1].Item1, Settings.CustomMouseClicks[i + 1].Item2), 2, enumerable[i]);
                        break;
                    case false:
                        Graphics.DrawLine(new Vector2(Settings.CustomMouseClicks[i].Item1, Settings.CustomMouseClicks[i].Item2),
                            new Vector2(Settings.CustomMouseClicks[i + 1].Item1, Settings.CustomMouseClicks[i + 1].Item2), 2, enumerable[i]);
                        break;
                }
            }
        }

        private static IEnumerable<Color> GetGradients(Color start, Color end, int steps)
        {
            var stepA = (end.A - start.A) / (steps - 1);
            var stepR = (end.R - start.R) / (steps - 1);
            var stepG = (end.G - start.G) / (steps - 1);
            var stepB = (end.B - start.B) / (steps - 1);
            for (var i = 0; i < steps; i++)
                yield return new Color(start.R + stepR * i, start.G + stepG * i, start.B + stepB * i, start.A + stepA * i);
        }

        private void DoCustomMouseClicks()
        {
            var latency = (int) _ingameState.CurLatency;
            if (!_ingameState.IngameUi.InventoryPanel.IsVisible)
                WinApi.KeyPress(Settings.InvHotkey.Value);
            var mousePosition = WinApi.GetMousePosition();
            foreach (var pointClick in Settings.CustomMouseClicks)
                if (pointClick.Item3)
                {
                    WinApi.SetCursorPosAndLeftClick(new Vector2(pointClick.Item1, pointClick.Item2),
                        Speed + latency);
                    Thread.Sleep(latency);
                }
                else if (!pointClick.Item3)
                {
                    WinApi.SetCursorPosAndRightClick(new Vector2(pointClick.Item1, pointClick.Item2),
                        Speed + latency);
                    Thread.Sleep(latency);
                }

            Thread.Sleep(latency);
            WinApi.SetCursorPos((int) mousePosition.X, (int) mousePosition.Y);
        }
    }

    #region WinApi

    public static class WinApi
    {
        public const int ClickDelay = 64;

        private const int KeyeventfExtendedkey = 0x0001;
        private const int KeyeventfKeyup = 0x0002;

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
            KeyEventKeyUp = 2
        }

        public static void SetCursorPosAndLeftClick(Vector2 pos, int extraDelay)
        {
            SetCursorPos((int) pos.X, (int) pos.Y);
            Thread.Sleep(extraDelay);
            mouse_event((int) MouseEventFlags.LeftDown, 0, 0, 0, 0);
            Thread.Sleep(ClickDelay);
            mouse_event((int) MouseEventFlags.LeftUp, 0, 0, 0, 0);
        }

        public static void SetCursorPosAndRightClick(Vector2 pos, int extraDelay)
        {
            SetCursorPos((int) pos.X, (int) pos.Y);
            Thread.Sleep(extraDelay);
            mouse_event((int) MouseEventFlags.RightDown, 0, 0, 0, 0);
            Thread.Sleep(ClickDelay);
            mouse_event((int) MouseEventFlags.RightUp, 0, 0, 0, 0);
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

        public static bool IsKeyDown(Keys key) => GetKeyState((int) key) < 0;

        #endregion
    }

    #endregion
}