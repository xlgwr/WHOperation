using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WHOperation
{
    static class Program
    {
        public static string _version = @"@4V20150116H10";

        public static string _userName;
        public static string _userIP;
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
    public class tfclass
    {
        public tfclass(string tfdnpartnumber, string tfrecmfgrpart, string tfdatecode, string tfrecqty, 
            string tflotno, string tfmfgdate, string tfexpiredate,
            string tfrirno, string tfmfgpart, string tfdndate, string tfdnqty)
        {

            _tfdnpartnumber = tfdnpartnumber.Trim();
            _tfrecmfgrpart = tfrecmfgrpart.Trim();
            _tfdatecode = tfdatecode.Trim();
            _tfrecqty = tfrecqty.Trim();
            _tflotno = tflotno.Trim();
            _tfmfgdate = tfmfgdate.Trim();
            _tfexpiredate = tfexpiredate.Trim();

            _tfrirno = tfrirno.Trim();
            _tfmfgpart =tfmfgpart.Trim();
            _tfdndate = tfdndate.Trim();
            _tfdnqty = tfdnqty.Trim();
        }


        public string _tfdnpartnumber { get; set; }

        public string _tfrecmfgrpart { get; set; }

        public string _tfdatecode { get; set; }

        public string _tfrecqty { get; set; }

        public string _tflotno { get; set; }

        public string _tfmfgdate { get; set; }

        public string _tfexpiredate { get; set; }

        public string _tfrirno { get; set; }

        public string _tfmfgpart { get; set; }

        public string _tfdndate { get; set; }

        public string _tfdnqty { get; set; }
    }
}