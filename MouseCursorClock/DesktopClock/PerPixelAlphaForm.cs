using System;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopClock {

    public class PerPixelAlphaForm : Form {

        public Graphics g = null;
        public IntPtr ScreenDC = Win32.GetDC(IntPtr.Zero);
        public IntPtr MemDC;
        public IntPtr hBitmap = IntPtr.Zero;
        public IntPtr oldBitmap = IntPtr.Zero;
        public Win32.Point bitmapOrigin = new Win32.Point(0, 0);
        public Win32.BLENDFUNCTION blend = new Win32.BLENDFUNCTION();
        public Win32.Size bitmapSize = new Win32.Size(0, 0);
        public Win32.Point windowTopLeftPos = new Win32.Point(0, 0);
        public Win32.Size windowSize = new Win32.Size(0, 0);


        public PerPixelAlphaForm() {
            MemDC = Win32.CreateCompatibleDC(ScreenDC);
            if (!DesignMode) {
                FormBorderStyle = FormBorderStyle.None;
                blend.BlendOp = Win32.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = 255;
                blend.AlphaFormat = Win32.AC_SRC_ALPHA;
            }

            Paint += designPaint;
        }

        private void designPaint(object sender, PaintEventArgs e) {
            if (DesignMode) {
                e.Graphics.DrawImage(My.Resources.Resources.birdClock, 0, 0, Width, Height);
            }
        }

        public void SetBitmap(Bitmap bmp) {
            if (!DesignMode)
                SetBitmap(bmp, 255);
        }

        private const int WS_EX_TOOLWINDOW = 0x80; // No apperance in alt+tab
        private const int WS_EX_NOACTIVATE = 0x8000000; // Don't bring to foreground
        private const int WS_TRANSPARENT = 0x20; // Click through
        private const int WS_LAYERED = 0x80000; // Needed for PNG transparency

        protected override CreateParams CreateParams {
            get {
                if (DesignMode) {
                    return base.CreateParams;
                }
                else {
                    var cp = base.CreateParams;
                    cp.ExStyle = cp.ExStyle | WS_EX_TOOLWINDOW | WS_LAYERED;
                    // cp.Parent = IntPtr.Zero ' Keep this line only if you used UserControl
                    return cp;
                }
            }
        }

        public void SetBitmap(Bitmap bmp, byte Opacity) {
            try {
                hBitmap = bmp.GetHbitmap(Color.FromArgb(0));
                oldBitmap = Win32.SelectObject(MemDC, hBitmap);

                bitmapSize.cx = bmp.Width;
                bitmapSize.cy = bmp.Height;

                windowTopLeftPos.x = Left;
                windowTopLeftPos.y = Top;

                windowSize.cx = Size.Width;
                windowSize.cy = Size.Height;

                blend.SourceConstantAlpha = Opacity;

                Win32.UpdateLayeredWindow(Handle, ScreenDC, ref windowTopLeftPos, ref windowSize, MemDC, ref bitmapOrigin, 0, ref blend, Win32.ULW_ALPHA);
            }
            catch (Exception) {
            }
            finally {
                if (!(hBitmap == IntPtr.Zero)) {
                    Win32.SelectObject(MemDC, oldBitmap);
                    Win32.DeleteObject(hBitmap);
                }
            }
        }

        public void closeDown() {
            Win32.ReleaseDC(IntPtr.Zero, ScreenDC);
            Win32.DeleteDC(MemDC);
        }

        private void InitializeComponent() {
            SuspendLayout();
            // 
            // PerPixelAlphaForm
            // 
            ClientSize = new Size(243, 224);
            Name = "Clock";
            ResumeLayout(false);

        }
    }
}