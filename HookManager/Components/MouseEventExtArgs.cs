using System.Windows.Forms;

namespace HookManager.Components
{
    public class MouseEventExtArgs : MouseEventArgs
    {
        internal MouseEventExtArgs(MouseEventArgs e) : base(e.Button, e.Clicks, e.X, e.Y, e.Delta)
        {
        }

        public MouseEventExtArgs(MouseButtons buttons, int clicks, int x, int y, int delta) : base(buttons, clicks, x,
            y, delta)
        {
        }

        public bool Handled { get; set; }
    }
}