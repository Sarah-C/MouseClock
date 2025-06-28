using System;
using System.ComponentModel;
using System.Diagnostics;

namespace DesktopClock.My {
    internal static partial class MyProject {
        internal partial class MyForms {

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Clock m_Clock;

            public Clock Clock {
                [DebuggerHidden]
                get {
                    m_Clock = Create__Instance__(m_Clock);
                    return m_Clock;
                }
                [DebuggerHidden]
                set {
                    if (ReferenceEquals(value, m_Clock))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Clock);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public PerPixelAlphaForm m_PerPixelAlphaForm;

            public PerPixelAlphaForm PerPixelAlphaForm {
                [DebuggerHidden]
                get {
                    m_PerPixelAlphaForm = Create__Instance__(m_PerPixelAlphaForm);
                    return m_PerPixelAlphaForm;
                }
                [DebuggerHidden]
                set {
                    if (ReferenceEquals(value, m_PerPixelAlphaForm))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_PerPixelAlphaForm);
                }
            }

        }


    }
}