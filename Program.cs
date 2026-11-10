using System;
using System.Windows.Forms;

namespace MultiManageApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new GUI.frmMainApp());
        }
    }
}