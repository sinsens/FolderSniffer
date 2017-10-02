using System;
using System.Windows.Forms;

namespace FolderSniffer
{
    internal static class Program
    {
        /// <summary>
        ///     应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var formMain = new FormMain())
            {
                Application.Run(formMain);
            }
        }
    }
}