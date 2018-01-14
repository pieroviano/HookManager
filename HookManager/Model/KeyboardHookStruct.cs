using System.Runtime.InteropServices;

namespace HookManager.Model
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyboardHookStruct
    {
        public readonly int VirtualKeyCode;
        public readonly int ScanCode;
        public readonly int Flags;
        public readonly int Time;
        public readonly int ExtraInfo;
    }
}