using System;
using System.Runtime.InteropServices;

namespace DictationProcessorApp
{
    public class GtkHelper
    {
        [DllImport("libgtk-x11-2.0.so.0")]
        static extern void gtk_init(int argc, IntPtr argv);

        [DllImport("libgtk-x11-2.0.so.0")]
        static extern IntPtr gtk_message_dialog_new(IntPtr window, int flags, int type, int buttons, string message,
            IntPtr args);

        [DllImport("libgtk-x11-2.0.so.0")]
        static extern int gtk_dialog_run(IntPtr dialog);

        public static void DisplayAlert(string message)
        {
            gtk_init(0, IntPtr.Zero);
            var dialog = gtk_message_dialog_new(IntPtr.Zero, 0, 0, 1, message, IntPtr.Zero);
            gtk_dialog_run(dialog);
        }
    }
}