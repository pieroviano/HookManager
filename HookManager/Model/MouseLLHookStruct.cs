using System.Runtime.InteropServices;
using HookManager.Model;

namespace HookManager.Model
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseLLHookStruct
    {
        public Point Point;
        public readonly int MouseData;
        public readonly int Flags;
        public readonly int Time;
        public readonly int ExtraInfo;
    }
}