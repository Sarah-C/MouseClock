using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Security;

namespace DesktopClock {

    public class AeroShape {

        public static void createAeroEllipse(IntPtr handle, int width, int height) {
            using (var path = new GraphicsPath()) {
                path.AddEllipse(0, 0, width, height);
                using (var region = new Region(path)) {
                    using (var g = Graphics.FromHwnd(handle)) {
                        var hRgn = region.GetHrgn(g);
                        var blur = new NativeMethods.DWM_BLURBEHIND();
                        blur.dwFlags = NativeMethods.DWM_BB.DWM_BB_ENABLE | NativeMethods.DWM_BB.DWM_BB_BLURREGION | NativeMethods.DWM_BB.DWM_BB_TRANSITIONONMAXIMIZED;
                        blur.fEnable = true;
                        blur.hRgnBlur = hRgn;
                        blur.fTransitionOnMaximized = true;

                        NativeMethods.DwmEnableBlurBehindWindow(handle, ref blur);
                        region.ReleaseHrgn(hRgn);
                    }
                }
            }
        }

        [SuppressUnmanagedCodeSecurity()]
        private sealed class NativeMethods {

            [StructLayout(LayoutKind.Sequential)]
            public struct DWM_BLURBEHIND {
                public DWM_BB dwFlags;
                public bool fEnable;
                public IntPtr hRgnBlur;
                public bool fTransitionOnMaximized;
            }

            private NativeMethods() {
            }


            [Flags()]
            public enum DWM_BB {
                DWM_BB_ENABLE = 1,
                DWM_BB_BLURREGION = 2,
                DWM_BB_TRANSITIONONMAXIMIZED = 4
            }

            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern bool DwmIsCompositionEnabled();

            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);

        }

    }
}