using System;
using System.Windows.Forms;

namespace ABPRenamer
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
            Application.Run(new FormMain());
        }
    }
}
