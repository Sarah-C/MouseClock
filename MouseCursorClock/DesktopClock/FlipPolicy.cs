using System;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace DesktopClock {

    public enum Flip3DPolicy {
        Default, // Window flips normally
        ExcludeBelow, // Window will not flip and will be drawn below the flipping windows
        ExcludeAbove // Window will not flip and will be drawn above the flipping windows
    }

    public class FlipChanger {

        private const int DWMWA_FLIP3D_POLICY = 8;

        public static void SetFlip3DPolicy(IntPtr hwndForm, Flip3DPolicy policy) {
            if (Environment.OSVersion.Version.Major < 6 | Environment.OSVersion.Version.Major > 6 | Environment.OSVersion.Version.Major == 6 & Environment.OSVersion.Version.Minor == 2)
                return;
            if (!DwmIsCompositionEnabled())
                return;
            int argattrValue = (int)policy;
            DwmSetWindowAttribute(hwndForm, DWMWA_FLIP3D_POLICY, ref argattrValue, Strings.Len(new int()));
        }

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern bool DwmIsCompositionEnabled();

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    }
}