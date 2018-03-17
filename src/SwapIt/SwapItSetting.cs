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

            macro1 = Keys.A;
            macro2 = Keys.Oem6;
            InvHotkey = Keys.V;
            Record = false;
            ShowPoint = false;

            m1sX = new RangeNode<float>(-1, -10, 2000);
            m1sY = new RangeNode<float>(-1, -10, 2000);
            m1eX = new RangeNode<float>(-1, -10, 2000);
            m1eY = new RangeNode<float>(-1, -10, 2000);
            m2sX = new RangeNode<float>(-1, -10, 2000);
            m2sY = new RangeNode<float>(-1, -10, 2000);
            m2eX = new RangeNode<float>(-1, -10, 2000);
            m2eY = new RangeNode<float>(-1, -10, 2000);
        }

        #region Setting


        [Menu("Record")]
        public ToggleNode Record { get; set; }

        [Menu("swap 1 key")]
        public HotkeyNode macro1 { get; set; }

        [Menu("swap 2 key")]
        public HotkeyNode macro2 { get; set; }

        [Menu("Set your inv key")]
        public HotkeyNode InvHotkey { get; set; }

        [Menu("Show points", 10)]
        public ToggleNode ShowPoint { get; set; }
        #endregion

        public RangeNode<float> m1sX { get; set; }
        public RangeNode<float> m1sY { get; set; }
        public RangeNode<float> m1eX { get; set; }
        public RangeNode<float> m1eY { get; set; }
        public RangeNode<float> m2sX { get; set; }
        public RangeNode<float> m2sY { get; set; }
        public RangeNode<float> m2eX { get; set; }
        public RangeNode<float> m2eY { get; set; }

    }
}
