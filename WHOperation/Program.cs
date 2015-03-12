using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WHOperation
{
    static class Program
    {
        public static string _version = @"@4V20150312H08";

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
        public tfclass()
        {
            initNothing();
        }
        public void initNothing()
        {
            _piid = "";
            _tfvendorid = "";
            _tfdnpartnumber = "";
            _tfrecmfgrpart = "";
            _tfdatecode = "";
            _tfrecqty = "";
            _tflotno = "";
            _tfmfgdate = "";
            _tfexpiredate = "";

            _tfrirno = "";
            _tfpartrec = "";
            _tfmfgpart = "";
            _tfdndate = "";
            _tfdnqty = "";
            _ttlQty = "";
        }
        public tfclass(string piid, string tfvendor, string tfdnpartnumber, string tfrecmfgrpart, string tfdatecode, string tfrecqty,
            string tflotno, string tfmfgdate, string tfexpiredate,
            string tfrirno, string tfpart, string tfmfgpart, string tfdndate, string tfdnqty)
        {
            initNothing();
            _piid = piid;
            _tfvendorid = tfvendor;
            _tfdnpartnumber = tfdnpartnumber.Trim();
            _tfrecmfgrpart = tfrecmfgrpart.Trim();
            _tfdatecode = tfdatecode == null ? "" : tfdatecode.Trim();
            _tfrecqty = tfrecqty.Trim();
            _tflotno = tflotno == null ? "" : tflotno.Trim();
            _tfmfgdate = tfmfgdate.Trim();
            _tfexpiredate = tfexpiredate.Trim();

            _tfrirno = tfrirno.Trim();
            _tfpartrec = tfpart.Trim();
            _tfmfgpart = tfmfgpart.Trim();
            _tfdndate = tfdndate.Trim();
            _tfdnqty = tfdnqty.Trim();
            _ttlQty = "";
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

        public string _tfpartrec { get; set; }

        public string _ttlQty { get; set; }

        public string _piid { get; set; }

        public string _tfvendorid { get; set; }
    }
}