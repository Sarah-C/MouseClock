using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DesktopClock {

    public static class MouseHook {

        public delegate void mouseCallback();
        public static mouseCallback persistentMouseCallback;
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public static HookProc persistentHookProc = mouseHookProc;
        public static int hHook = 0;
        public const int WH_MOUSE_LL = 14;

        [StructLayout(LayoutKind.Sequential)]
        public class MouseState {
            public Point point;
            public int mouseData;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();

        public static void hook(ref mouseCallback mouseCallback) {
            if (hHook == 0) {
                SetProcessDPIAware();
                persistentMouseCallback = mouseCallback;
                hHook = SetWindowsHookEx(WH_MOUSE_LL, persistentHookProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                if (hHook == 0)
                    throw new Exception("Hook failed");
            }
        }

        public static void unhook() {
            bool ret = UnhookWindowsHookEx(hHook);
            if (ret == false)
                throw new Exception("UnHook failed");
            hHook = 0;
        }

        public static bool _wheelUp = false;
        public static bool _wheelDown = false;
        public static bool _wheelPressed = false;
        public static bool _wheelReleased = false;
        public static bool _leftPressed = false;
        public static bool _leftReleased = false;
        public static bool _rightPressed = false;
        public static bool _rightReleased = false;
        public static bool _mouseMoved = false;
        public static int _mouseX = 0;
        public static int _mouseY = 0;

        public static int mouseHookProc(int nCode, IntPtr wParam, IntPtr lParam) {
            MouseState mouseState = (MouseState)Marshal.PtrToStructure(lParam, typeof(MouseState));
            if (nCode < 0) {
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else {
                _wheelUp = mouseState.mouseData > 0;
                _wheelDown = mouseState.mouseData < 0;
                _wheelPressed = wParam == (IntPtr)519;
                _wheelReleased = wParam == (IntPtr)520;
                _leftPressed = wParam == (IntPtr)513;
                _leftReleased = wParam == (IntPtr)514;
                _rightPressed = wParam == (IntPtr)516;
                _rightReleased = wParam == (IntPtr)517;
                _mouseX = mouseState.point.X;
                _mouseY = mouseState.point.Y;
                _mouseMoved = wParam == (IntPtr)512;
                persistentMouseCallback();
                return CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

    }
}