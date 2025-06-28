using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DesktopClock {
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Clock : PerPixelAlphaForm {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing) {
            try {
                if (disposing && components is not null) {
                    components.Dispose();
                }
            }
            finally {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components = null;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent() {
            SuspendLayout();
            // 
            // Clock
            // 
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(80, 80);
            Name = "Clock";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Clock";
            Load += new EventHandler(Clock_Load);
            ResumeLayout(false);

        }

    }
}