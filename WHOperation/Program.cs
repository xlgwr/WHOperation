using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WHOperation
{
    static class Program
    {
        public static string _version = @"@4V20141230H14";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var mdip = new MDIParent1();
            mdip.Text += _version;
            Application.Run(mdip);
            //Application.Run(new Form1());
            //Application.Run(new fLogin());
            //Application.Run(new vendorLabelMaster());
        }
    }
}