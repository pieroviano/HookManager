using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HookManager.Components;
using HookManager.HookUtility.Win32;
using HookManager.Model;

namespace HookManager.Utility
{
    public class MouseHookUtility
    {
        private int _oldX;
        private int _oldY;
        private Timer _sDoubleClickTimer;
        private HookProc _sMouseDelegate;
        private int _sMouseHookHandle;
        private MouseButtons _sPrevClickedButton;

        private static MouseHookUtility _instance;

        private MouseHookUtility()
        {
            
        }

        public static MouseHookUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MouseHookUtility();
                return _instance;
            }
        }

        private void DoubleClickTimeElapsed(object sender, EventArgs e)
        {
            _sDoubleClickTimer.Enabled = false;
            _sPrevClickedButton = MouseButtons.None;
        }

        private void EnsureSubscribedToGlobalMouseEvents()
        {
            if (_sMouseHookHandle == 0)
            {
                _sMouseDelegate = MouseHookProc;
                var executingAssembly = Assembly.GetExecutingAssembly();
                // ReSharper disable once UnusedVariable
                var module = executingAssembly.GetModules()[0];
                var intPtr =
                    Win32Interop.GetModuleHandle(Process.GetCurrentProcess().MainModule
                        .ModuleName); //Marshal.GetHINSTANCE(module);
                _sMouseHookHandle = Win32Interop.SetWindowsHookEx(14, _sMouseDelegate, intPtr, 0);
                if (_sMouseHookHandle == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        private void ForceUnsunscribeFromGlobalMouseEvents()
        {
            if (_sMouseHookHandle != 0)
            {
                var num = Win32Interop.UnhookWindowsHookEx(_sMouseHookHandle);
                _sMouseHookHandle = 0;
                _sMouseDelegate = null;
                if (num == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        public event MouseEventHandler MouseClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseClick += value;
            }
            remove
            {
                SMouseClick -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        public event EventHandler<MouseEventExtArgs> MouseClickExt
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseClickExt += value;
            }
            remove
            {
                SMouseClickExt -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        public event MouseEventHandler MouseDoubleClick
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseDoubleClick += value;
            }
            remove
            {
                if (SMouseDoubleClick != null)
                {
                    SMouseDoubleClick -= value;
                    if (SMouseDoubleClick == null)
                    {
                        MouseUp -= OnMouseUp;
                        _sDoubleClickTimer.Tick -= DoubleClickTimeElapsed;
                        _sDoubleClickTimer = null;
                    }
                }
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        public event MouseEventHandler MouseDown
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseDown += value;
            }
            remove
            {
                SMouseDown -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var struct2 = (MouseLLHookStruct) Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));
                var none = MouseButtons.None;
                short delta = 0;
                var clicks = 0;
                var flag = false;
                var flag2 = false;
                switch (wParam)
                {
                    case 0x201:
                        flag = true;
                        none = MouseButtons.Left;
                        clicks = 1;
                        break;

                    case 0x202:
                        flag2 = true;
                        none = MouseButtons.Left;
                        clicks = 1;
                        break;

                    case 0x203:
                        none = MouseButtons.Left;
                        clicks = 2;
                        break;

                    case 0x204:
                        flag = true;
                        none = MouseButtons.Right;
                        clicks = 1;
                        break;

                    case 0x205:
                        flag2 = true;
                        none = MouseButtons.Right;
                        clicks = 1;
                        break;

                    case 0x206:
                        none = MouseButtons.Right;
                        clicks = 2;
                        break;

                    case 0x20a:
                        try
                        {
                            delta = (short) ((struct2.MouseData >> 0x10) & 0xffff);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                        break;
                }
                var e = new MouseEventExtArgs(none, clicks, struct2.Point.X, struct2.Point.Y, delta);
                if (Instance.SMouseUp != null && flag2)
                {
                    Instance.SMouseUp(null, e);
                }
                if (Instance.SMouseDown != null && flag)
                {
                    Instance.SMouseDown(null, e);
                }
                if (Instance.SMouseClick != null && clicks > 0)
                {
                    Instance.SMouseClick(null, e);
                }
                if (Instance.SMouseClickExt != null && clicks > 0)
                {
                    Instance.SMouseClickExt(null, e);
                }
                if (Instance.SMouseDoubleClick != null && clicks == 2)
                {
                    Instance.SMouseDoubleClick(null, e);
                }
                if (Instance.SMouseWheel != null && delta != 0)
                {
                    Instance.SMouseWheel(null, e);
                }
                if ((Instance.SMouseMove != null || Instance.SMouseMoveExt != null) &&
                    (Instance._oldX != struct2.Point.X || Instance._oldY != struct2.Point.Y))
                {
                    Instance._oldX = struct2.Point.X;
                    Instance._oldY = struct2.Point.Y;
                    if (Instance.SMouseMove != null)
                    {
                        Instance.SMouseMove(null, e);
                    }
                    if (Instance.SMouseMoveExt != null)
                    {
                        Instance.SMouseMoveExt(null, e);
                    }
                }
                if (e.Handled)
                {
                    return -1;
                }
            }
            return Win32Interop.CallNextHookEx(Instance._sMouseHookHandle, nCode, wParam, lParam);
        }

        public event MouseEventHandler MouseMove
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseMove += value;
            }
            remove
            {
                SMouseMove -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        public event EventHandler<MouseEventExtArgs> MouseMoveExt
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseMoveExt += value;
            }
            remove
            {
                SMouseMoveExt -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        public event MouseEventHandler MouseUp
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseUp += value;
            }
            remove
            {
                SMouseUp -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        public event MouseEventHandler MouseWheel
        {
            add
            {
                EnsureSubscribedToGlobalMouseEvents();
                SMouseWheel += value;
            }
            remove
            {
                SMouseWheel -= value;
                TryUnsubscribeFromGlobalMouseEvents();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Clicks >= 1)
            {
                if (e.Button.Equals(_sPrevClickedButton))
                {
                    SMouseDoubleClick?.Invoke(null, e);
                    _sDoubleClickTimer.Enabled = false;
                    _sPrevClickedButton = MouseButtons.None;
                }
                else
                {
                    _sDoubleClickTimer.Enabled = true;
                    _sPrevClickedButton = e.Button;
                }
            }
        }

        private event MouseEventHandler SMouseClick;

        private event EventHandler<MouseEventExtArgs> SMouseClickExt;

        private event MouseEventHandler SMouseDoubleClick;

        private event MouseEventHandler SMouseDown;

        private event MouseEventHandler SMouseMove;

        private event EventHandler<MouseEventExtArgs> SMouseMoveExt;

        private event MouseEventHandler SMouseUp;

        private event MouseEventHandler SMouseWheel;

        private void TryUnsubscribeFromGlobalMouseEvents()
        {
            if (SMouseClick == null && SMouseDown == null && SMouseMove == null && SMouseUp == null &&
                SMouseClickExt == null && SMouseMoveExt == null && SMouseWheel == null)
            {
                ForceUnsunscribeFromGlobalMouseEvents();
            }
        }
    }

    public delegate int HookProc(int nCode, int wParam, IntPtr lParam);
}