using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PoeHUD.Hud.Settings;
using PoeHUD.Plugins;

namespace SwapIt
{
    public class SwapItSetting : SettingsBase
    {
        public SwapItSetting()
        {
            Enable = true;
            //Speed = new RangeNode<int>(50, 10, 200);

            StartSwap = Keys.A;
            InvHotkey = Keys.V;
            AdditKey1 = Keys.ControlKey;
            AdditKey2 = Keys.LShiftKey;
            Record = false;
            ShowPoint = false;

            StartPointX = new RangeNode<float>(-1, -10, 2000);
            StartPointY = new RangeNode<float>(-1, -10, 2000);
            EndPointX = new RangeNode<float>(-1, -10, 2000);
            EndPointY = new RangeNode<float>(-1, -10, 2000);
            CustomMouseClicks = new List<Tuple<int, int, bool>>();
        }

        #region Setting


        [Menu("Record", Tooltip = "Enable macro recording. To finish recording, press additional key 1 or 2 and turn it off.")]
        public ToggleNode Record { get; set; }

        [Menu("Swap key", Tooltip = "To start, press this key + additional 1 and 2 keys")]
        public HotkeyNode StartSwap { get; set; }

        [Menu("Set your inv key", Tooltip = "Button to open your inventory in the game")]
        public HotkeyNode InvHotkey { get; set; }

        [Menu("Additional key 1")]
        public HotkeyNode AdditKey1 { get; set; }

        [Menu("Additional key 2")]
        public HotkeyNode AdditKey2 { get; set; }

        [Menu("Show points", 10)]
        public ToggleNode ShowPoint { get; set; }
        #endregion

        public RangeNode<float> StartPointX { get; set; }
        public RangeNode<float> StartPointY { get; set; }
        public RangeNode<float> EndPointX { get; set; }
        public RangeNode<float> EndPointY { get; set; }
        
        public List<Tuple<int, int, bool>> CustomMouseClicks { get; set; }
    }
}
