using Olympia.Forms;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Olympia {
    internal static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RootForm());
            Application.ApplicationExit += Application_Exit;
            Application.Exit();     
        }

        private static void Application_Exit(object sender, EventArgs e) {
            foreach (Form form in Application.OpenForms) {
                form.Close();
                //Application.Exit();
            }
        }
    }
}
