using System.Runtime.InteropServices;

namespace HookManager.Model
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Point
    {
        public readonly int X;
        public readonly int Y;
    }
}