using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HookManager.HookUtility.Win32;
using HookManager.Model;
using HookManager.Utility;

namespace HookManager.HookUtility
{
    internal class KeyboardHookUtility
    {
        private HookProc _sKeyboardDelegate;
        private int _sKeyboardHookHandle;

        private event KeyEventHandler SKeyDown;
        private event KeyPressEventHandler SKeyPress;
        private event KeyEventHandler SKeyUp;

        private static KeyboardHookUtility _instance;

        private KeyboardHookUtility()
        {

        }

        public static KeyboardHookUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new KeyboardHookUtility();
                return _instance;
            }
        }

        private void EnsureSubscribedToGlobalKeyboardEvents()
        {
            if (_sKeyboardHookHandle == 0)
            {
                _sKeyboardDelegate = KeyboardHookProc;
                _sKeyboardHookHandle = Win32Interop.SetWindowsHookEx(13, _sKeyboardDelegate,
                    Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                if (_sKeyboardHookHandle == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        private void ForceUnsunscribeFromGlobalKeyboardEvents()
        {
            if (_sKeyboardHookHandle != 0)
            {
                var num = Win32Interop.UnhookWindowsHookEx(_sKeyboardHookHandle);
                _sKeyboardHookHandle = 0;
                _sKeyboardDelegate = null;
                if (num == 0)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
        }

        private static int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            var handled = false;
            if (nCode >= 0)
            {
                var struct2 = (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                if (Instance.SKeyDown != null && (wParam == 0x100 || wParam == 260))
                {
                    var e = new KeyEventArgs((Keys) struct2.VirtualKeyCode);
                    Instance.SKeyDown(null, e);
                    handled = e.Handled;
                }
                if (Instance.SKeyPress != null && wParam == 0x100)
                {
                    var flag2 = (Win32Interop.GetKeyState(0x10) & 0x80) == 0x80;
                    var flag3 = Win32Interop.GetKeyState(20) != 0;
                    var pbKeyState = new byte[0x100];
                    Win32Interop.GetKeyboardState(pbKeyState);
                    var lpwTransKey = new byte[2];
                    if (Win32Interop.ToAscii(struct2.VirtualKeyCode, struct2.ScanCode, pbKeyState, lpwTransKey,
                            struct2.Flags) == 1)
                    {
                        var c = (char) lpwTransKey[0];
                        if (flag3 ^ flag2 && char.IsLetter(c))
                        {
                            c = char.ToUpper(c);
                        }
                        var args2 = new KeyPressEventArgs(c);
                        Instance.SKeyPress(null, args2);
                        handled = handled || args2.Handled;
                    }
                }
                if (Instance.SKeyUp != null && (wParam == 0x101 || wParam == 0x105))
                {
                    var args3 = new KeyEventArgs((Keys) struct2.VirtualKeyCode);
                    Instance.SKeyUp(null, args3);
                    handled = handled || args3.Handled;
                }
            }
            if (handled)
            {
                return -1;
            }
            return Win32Interop.CallNextHookEx(Instance._sKeyboardHookHandle, nCode, wParam, lParam);
        }

        public event KeyEventHandler KeyDown
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                SKeyDown += value;
            }
            remove
            {
                SKeyDown -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        public event KeyPressEventHandler KeyPress
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                SKeyPress += value;
            }
            remove
            {
                SKeyPress -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        public event KeyEventHandler KeyUp
        {
            add
            {
                EnsureSubscribedToGlobalKeyboardEvents();
                SKeyUp += value;
            }
            remove
            {
                SKeyUp -= value;
                TryUnsubscribeFromGlobalKeyboardEvents();
            }
        }

        private void TryUnsubscribeFromGlobalKeyboardEvents()
        {
            if (SKeyDown == null && SKeyUp == null && SKeyPress == null)
            {
                ForceUnsunscribeFromGlobalKeyboardEvents();
            }
        }
    }
}