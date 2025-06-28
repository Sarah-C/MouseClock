using System;
using System.Diagnostics;
using System.Drawing;

using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace DesktopClock {

    public partial class Clock {

        private Timer _t;
        public virtual Timer t {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get {
                return _t;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set {
                if (_t != null) {
                    _t.Tick -= t_Tick;
                }

                _t = value;
                if (_t != null) {
                    _t.Tick += t_Tick;
                }
            }
        }
        public int clockLeftTopborder = 10;

        public Bitmap background = null;

        private Bitmap surface = null;
        private Graphics surfaceG = null;

        public int centerX = 0;
        public int centerY = 0;

        public DateTime dt = default;
        public decimal h = 0m;
        public decimal m = 0m;
        public decimal s = 0m;
        public decimal ms = 0m;

        private const float startPoint = 0f;
        private const float hourEndPoint = 0.45f;
        private const float minuteEndPoint = 0.65f;
        private const float secondEndPoint = 0.85f;

        private const float shadowOffset = 3.0f;
        public Color shadowColor = Color.FromArgb(180, 0, 0, 0);

        public decimal hoursAngle = 0m;
        public decimal minsAngle = 0m;
        public decimal secAngle = 0m;

        public PointF hours = default;
        public PointF mins = default;
        public PointF secs = default;

        public Pen pen1;
        public Pen pen2;
        public Pen pen3;

        public Pen pen4 = new Pen(Color.Black, 4f);
        public Pen pen5 = new Pen(Color.Blue, 4f);
        public Pen pen6 = new Pen(Color.Maroon, 2f);

        public SolidBrush brush1 = new SolidBrush(Color.Maroon);

        public Clock() {
            t = new Timer();
            pen1 = new Pen(shadowColor, 4f);
            pen2 = new Pen(shadowColor, 4f);
            pen3 = new Pen(shadowColor, 2f);
            InitializeComponent();
        }

        private void Clock_Load(object sender, EventArgs e) {
            InitializeComponent();
            ClientSize = new Size(80, 80);
            TopMost = true;
            // FlipChanger.SetFlip3DPolicy(Handle, Flip3DPolicy.ExcludeAbove);
            // AeroShape.createAeroEllipse(Handle, Width, Height);
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            centerX = Width / 2;
            centerY = Height / 2;
            background = makeBackground();
            surface = (Bitmap)background.Clone();
            surfaceG = Graphics.FromImage(surface);
            surfaceG.CompositingMode = CompositingMode.SourceOver;
            surfaceG.CompositingQuality = CompositingQuality.HighQuality;
            surfaceG.InterpolationMode = InterpolationMode.HighQualityBicubic;
            surfaceG.SmoothingMode = SmoothingMode.AntiAlias;
            surfaceG.TranslateTransform(centerX, centerY);
            MouseHook.mouseCallback argmouseCallback = mouseCallback;
            MouseHook.hook(ref argmouseCallback);
            SetBitmap(surface);
            Left = Cursor.Position.X + clockLeftTopborder;
            Top = Cursor.Position.Y + clockLeftTopborder;
            t.Interval = 50;
            t.Enabled = true;
        }

        public void mouseCallback() {
            // Move the form the moment the mouse registers a movement.
            Left = MouseHook._mouseX + clockLeftTopborder;
            Top = MouseHook._mouseY + clockLeftTopborder;

            if (MouseHook._mouseY == 0 && MouseHook._rightPressed) {
                closeDown();
                MouseHook.unhook();
                Application.Exit();
            }

            //Debug.Print(MouseHook._leftPressed);
            // string strCaption = $"x = {MouseHook._mouseX}  y = {MouseHook._mouseY} {MouseHook._mouseMoved} {MouseHook._leftPressed} {MouseHook._leftReleased} {MouseHook._rightPressed} {MouseHook._rightReleased} {MouseHook._wheelDown} {MouseHook._wheelUp} {MouseHook._wheelPressed} {MouseHook._wheelReleased}";
            // Debug.Print(strCaption);
        }

        // Public surface As Bitmap = Nothing
        private void t_Tick(object sender, EventArgs e) {
            surfaceG.Clear(Color.FromArgb(0, 0, 0, 0));
            surfaceG.DrawImageUnscaled(background, -centerX, -centerY);

            dt = DateTime.Now;
            h = dt.Hour;
            m = dt.Minute;
            s = dt.Second;
            ms = dt.Millisecond;

            hoursAngle = 360m * (h / 12m) + 30m * (m / 60m);
            hours = coords(hoursAngle);
            minsAngle = 360m * (m / 60m) + 6m * (s / 60m);
            mins = coords(minsAngle);
            secAngle = 360m * (s / 60m) + 6m * (ms / 1000m);
            secs = coords(secAngle);

            surfaceG.DrawLine(pen1, shadowOffset + mins.X * (centerX * startPoint), shadowOffset + mins.Y * (centerY * startPoint), shadowOffset + mins.X * (centerX * minuteEndPoint), shadowOffset + mins.Y * (centerY * minuteEndPoint));
            surfaceG.DrawLine(pen2, shadowOffset + hours.X * (centerX * startPoint), shadowOffset + hours.Y * (centerY * startPoint), shadowOffset + hours.X * (centerX * hourEndPoint), shadowOffset + hours.Y * (centerY * hourEndPoint));
            surfaceG.DrawLine(pen3, shadowOffset + secs.X * (centerX * startPoint), shadowOffset + secs.Y * (centerY * startPoint), shadowOffset + secs.X * (centerX * secondEndPoint), shadowOffset + secs.Y * (centerY * secondEndPoint));
            surfaceG.DrawLine(pen4, mins.X * (centerX * startPoint), mins.Y * (centerY * startPoint), mins.X * (centerX * minuteEndPoint), mins.Y * (centerY * minuteEndPoint));
            surfaceG.DrawLine(pen5, hours.X * (centerX * startPoint), hours.Y * (centerY * startPoint), hours.X * (centerX * hourEndPoint), hours.Y * (centerY * hourEndPoint));
            surfaceG.DrawLine(pen6, secs.X * (centerX * startPoint), secs.Y * (centerY * startPoint), secs.X * (centerX * secondEndPoint), secs.Y * (centerY * secondEndPoint));
            surfaceG.FillEllipse(brush1, -3, -3, 6, 6);

            SetBitmap(surface, 100);

            Left = Cursor.Position.X + clockLeftTopborder;
            Top = Cursor.Position.Y + clockLeftTopborder;
            //SetBitmap(rotateImage(surface, (float)-((360 * (s / 60)) + (6 * (ms / 1000)))), 100);
        }

        public Bitmap makeBackground() {
            var bg = new Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var bgg = Graphics.FromImage(bg);

            bgg.CompositingQuality = CompositingQuality.HighQuality;
            bgg.InterpolationMode = InterpolationMode.HighQualityBicubic;
            bgg.SmoothingMode = SmoothingMode.AntiAlias;
            bgg.TextContrast = 2;
            bgg.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            bgg.Clear(Color.FromArgb(0, 0, 0, 0));

            bgg.FillEllipse(new SolidBrush(Color.FromArgb(255, 60, 60, 60)), 1, 1, Width - 2, Height - 2);
            bgg.FillEllipse(new SolidBrush(Color.FromArgb(255, 210, 210, 210)), 3, 3, Width - 6, Height - 6);

            bgg.TranslateTransform(centerX, centerY);
            int hour = 0;
            for (int angle = 30; angle <= 360; angle += (int)Math.Round(360d / 12d)) {
                hour += 1;
                var r = coords(angle);
                var p = r;
                p.X *= (float)(centerX / 1.3m);
                p.Y *= (float)(centerY / 1.3m);
                p.X -= 3f;
                p.Y -= 4f;
                bgg.DrawString(hour.ToString(), new Font("Arial", 6f), Brushes.Black, p);

                bgg.DrawLine(Pens.Black, r.X * (float)(centerX / 1.05m), r.Y * (float)(centerY / 1.05m), r.X * (float)(centerX / 1.2m), r.Y * (float)(centerY / 1.2m));
            }

            bgg.Dispose();

            return bg;
        }

        public PointF coords(decimal angle) {
            angle -= 90m;
            decimal x = (decimal)Math.Cos((double)(angle / 180m) * Math.PI);
            decimal y = (decimal)Math.Sin((double)(angle / 180m) * Math.PI);
            return new PointF((float)x, (float)y);
        }

        public Bitmap rotateImage(Image preRotated, float angle) {
            return rotateImage(preRotated, new PointF((float)(preRotated.Width / 2m), (float)(preRotated.Height / 2m)), angle);
        }

        public Bitmap rotateImage(Image preRotated, PointF offset, float angle) {
            if (preRotated is null) {
                throw new ArgumentNullException("image");
            }
            var rotatedBmp = new Bitmap(preRotated.Width, preRotated.Height);
            rotatedBmp.SetResolution(preRotated.HorizontalResolution, preRotated.VerticalResolution);
            var g = Graphics.FromImage(rotatedBmp);

            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.TranslateTransform(offset.X, offset.Y);
            g.RotateTransform(angle);
            g.TranslateTransform(-offset.X, -offset.Y);
            g.DrawImage(preRotated, new PointF(0f, 0f));
            g.Dispose();
            return rotatedBmp;
        }

    }
}