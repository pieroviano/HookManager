using System;
using System.ComponentModel;
using System.Windows.Forms;
using HookManager.HookUtility;
using HookManager.Utility;

namespace HookManager.Components
{
    internal class GlobalEventProvider : Component
    {
        protected override bool CanRaiseEvents => true;

        private void HookUtility_KeyDown(object sender, KeyEventArgs e)
        {
            PrivateKeyDown?.Invoke(this, e);
        }

        private void HookUtility_KeyPress(object sender, KeyPressEventArgs e)
        {
            PrivateKeyPress?.Invoke(this, e);
        }

        private void HookUtility_KeyUp(object sender, KeyEventArgs e)
        {
            PrivateKeyUp?.Invoke(this, e);
        }

        private void HookUtility_MouseClick(object sender, MouseEventArgs e)
        {
            PrivateMouseClick?.Invoke(this, e);
        }

        private void HookUtility_MouseClickExt(object sender, MouseEventExtArgs e)
        {
            PrivateMouseClickExt?.Invoke(this, e);
        }

        private void HookUtility_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PrivateMouseDoubleClick?.Invoke(this, e);
        }

        private void HookUtility_MouseDown(object sender, MouseEventArgs e)
        {
            PrivateMouseDown?.Invoke(this, e);
        }

        private void HookUtility_MouseMove(object sender, MouseEventArgs e)
        {
            PrivateMouseMove?.Invoke(this, e);
        }

        private void HookUtility_MouseMoveExt(object sender, MouseEventExtArgs e)
        {
            PrivateMouseMoveExt?.Invoke(this, e);
        }

        private void HookUtility_MouseUp(object sender, MouseEventArgs e)
        {
            PrivateMouseUp?.Invoke(this, e);
        }

        public event KeyEventHandler KeyDown
        {
            add
            {
                if (PrivateKeyDown == null)
                {
                    KeyboardHookUtility.Instance.KeyDown += HookUtility_KeyDown;
                }
                PrivateKeyDown += value;
            }
            remove
            {
                PrivateKeyDown -= value;
                if (PrivateKeyDown == null)
                {
                    KeyboardHookUtility.Instance.KeyDown -= HookUtility_KeyDown;
                }
            }
        }

        public event KeyPressEventHandler KeyPress
        {
            add
            {
                if (PrivateKeyPress == null)
                {
                    KeyboardHookUtility.Instance.KeyPress += HookUtility_KeyPress;
                }
                PrivateKeyPress += value;
            }
            remove
            {
                PrivateKeyPress -= value;
                if (PrivateKeyPress == null)
                {
                    KeyboardHookUtility.Instance.KeyPress -= HookUtility_KeyPress;
                }
            }
        }

        public event KeyEventHandler KeyUp
        {
            add
            {
                if (PrivateKeyUp == null)
                {
                    KeyboardHookUtility.Instance.KeyUp += HookUtility_KeyUp;
                }
                PrivateKeyUp += value;
            }
            remove
            {
                PrivateKeyUp -= value;
                if (PrivateKeyUp == null)
                {
                    KeyboardHookUtility.Instance.KeyUp -= HookUtility_KeyUp;
                }
            }
        }

        public event MouseEventHandler MouseClick
        {
            add
            {
                if (PrivateMouseClick == null)
                {
                    MouseHookUtility.Instance.MouseClick += HookUtility_MouseClick;
                }
                PrivateMouseClick += value;
            }
            remove
            {
                PrivateMouseClick -= value;
                if (PrivateMouseClick == null)
                {
                    MouseHookUtility.Instance.MouseClick -= HookUtility_MouseClick;
                }
            }
        }

        public event EventHandler<MouseEventExtArgs> MouseClickExt
        {
            add
            {
                if (PrivateMouseClickExt == null)
                {
                    MouseHookUtility.Instance.MouseClickExt += HookUtility_MouseClickExt;
                }
                PrivateMouseClickExt += value;
            }
            remove
            {
                PrivateMouseClickExt -= value;
                if (PrivateMouseClickExt == null)
                {
                    MouseHookUtility.Instance.MouseClickExt -= HookUtility_MouseClickExt;
                }
            }
        }

        public event MouseEventHandler MouseDoubleClick
        {
            add
            {
                if (PrivateMouseDoubleClick == null)
                {
                    MouseHookUtility.Instance.MouseDoubleClick += HookUtility_MouseDoubleClick;
                }
                PrivateMouseDoubleClick += value;
            }
            remove
            {
                PrivateMouseDoubleClick -= value;
                if (PrivateMouseDoubleClick == null)
                {
                    MouseHookUtility.Instance.MouseDoubleClick -= HookUtility_MouseDoubleClick;
                }
            }
        }

        public event MouseEventHandler MouseDown
        {
            add
            {
                if (PrivateMouseDown == null)
                {
                    MouseHookUtility.Instance.MouseDown += HookUtility_MouseDown;
                }
                PrivateMouseDown += value;
            }
            remove
            {
                PrivateMouseDown -= value;
                if (PrivateMouseDown == null)
                {
                    MouseHookUtility.Instance.MouseDown -= HookUtility_MouseDown;
                }
            }
        }

        public event MouseEventHandler MouseMove
        {
            add
            {
                if (PrivateMouseMove == null)
                {
                    MouseHookUtility.Instance.MouseMove += HookUtility_MouseMove;
                }
                PrivateMouseMove += value;
            }
            remove
            {
                PrivateMouseMove -= value;
                if (PrivateMouseMove == null)
                {
                    MouseHookUtility.Instance.MouseMove -= HookUtility_MouseMove;
                }
            }
        }

        public event EventHandler<MouseEventExtArgs> MouseMoveExt
        {
            add
            {
                if (PrivateMouseMoveExt == null)
                {
                    MouseHookUtility.Instance.MouseMoveExt += HookUtility_MouseMoveExt;
                }
                PrivateMouseMoveExt += value;
            }
            remove
            {
                PrivateMouseMoveExt -= value;
                if (PrivateMouseMoveExt == null)
                {
                    MouseHookUtility.Instance.MouseMoveExt -= HookUtility_MouseMoveExt;
                }
            }
        }

        public event MouseEventHandler MouseUp
        {
            add
            {
                if (PrivateMouseUp == null)
                {
                    MouseHookUtility.Instance.MouseUp += HookUtility_MouseUp;
                }
                PrivateMouseUp += value;
            }
            remove
            {
                PrivateMouseUp -= value;
                if (PrivateMouseUp == null)
                {
                    MouseHookUtility.Instance.MouseUp -= HookUtility_MouseUp;
                }
            }
        }

        private event KeyEventHandler PrivateKeyDown;

        private event KeyPressEventHandler PrivateKeyPress;

        private event KeyEventHandler PrivateKeyUp;

        private event MouseEventHandler PrivateMouseClick;

        private event EventHandler<MouseEventExtArgs> PrivateMouseClickExt;

        private event MouseEventHandler PrivateMouseDoubleClick;

        private event MouseEventHandler PrivateMouseDown;

        private event MouseEventHandler PrivateMouseMove;

        private event EventHandler<MouseEventExtArgs> PrivateMouseMoveExt;

        private event MouseEventHandler PrivateMouseUp;
    }
}