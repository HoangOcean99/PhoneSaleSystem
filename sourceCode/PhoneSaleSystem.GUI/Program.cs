using System;
using System.Windows.Forms;
using PhoneSaleSystem.GUI.Forms;

namespace PhoneSaleSystem.GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            using var loginForm = new FormLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FormMain());
            }
        }
    }
}
