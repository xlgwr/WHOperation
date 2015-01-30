using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using WHOperation.EF.WHO;

namespace WHOperation
{
    public partial class frmChangeErr : Form
    {
        Form1 _frm1;
        public frmChangeErr()
        {
            InitializeComponent();
        }
        public frmChangeErr(Form1 frm1, DataGridView dgv)
        {
            InitializeComponent();

            _frm1 = frm1;
            try
            {
                DataGridViewRow _sdgvr = dgv.SelectedRows[0];
                if (_frm1._usdgv1Pend)
                {

                    tfpartno.Text = _sdgvr.Cells["PartNumber"].Value.ToString();
                    tfmfgpart.Text = _sdgvr.Cells["MFGPartNo"].Value.ToString();
                    tfrirno.Text = _sdgvr.Cells["RIRNo"].Value.ToString();
                    _strPONum = _sdgvr.Cells["PONumber"].Value.ToString();
                    _strPiMfgr = _sdgvr.Cells["ASNMFGPN"].Value.ToString();
                    tfdnqty.Text = _sdgvr.Cells["DNQty"].Value.ToString();
                    _intOldPrintQty = _sdgvr.Cells["PrintedQty"].Value.ToString();
                    txt0PrintedQty.Text = _intOldPrintQty;
                }
                else
                {

                    tfpartno.Text = _sdgvr.Cells["PI_PART"].Value.ToString();
                    tfmfgpart.Text = _sdgvr.Cells["pi_mfgr_part"].Value.ToString();
                    tfrirno.Text = _sdgvr.Cells["PI_LOT"].Value.ToString();
                    _strPONum = _sdgvr.Cells["PI_PO"].Value.ToString();
                    _strPiMfgr = _sdgvr.Cells["pi_mfgr"].Value.ToString();
                    tfdnqty.Text = _sdgvr.Cells["PI_QTY"].Value.ToString();
                    _intOldPrintQty = _sdgvr.Cells["PI_Print_QTY"].Value.ToString();
                    txt0PrintedQty.Text = _intOldPrintQty;
                }

                if (_frm1._usePrintPI)
                {
                    _IdKey = _frm1._piid;
                }
                else
                {
                    _IdKey = _frm1._dnNo;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }


            this.MaximizeBox = false;
            this.AcceptButton = btnUpdate;
            this.StartPosition = FormStartPosition.CenterScreen;
            txt0PrintedQty.Focus();
        }

        private void frmChangeErr_Load(object sender, EventArgs e)
        {
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt0PrintedQty.Text))
            {
                if (!_frm1.IsNumber(txt0PrintedQty.Text.Trim()))
                {
                    txt0PrintedQty.Focus();
                    lbl_msg.Text = "Please enter a right Number.Thank you!";
                }
                if (Convert.ToDouble(tfdnqty.Text) < Convert.ToDouble(txt0PrintedQty.Text))
                {
                    txt0PrintedQty.Focus();
                    lbl_msg.Text = "Enter too big number >" + tfdnqty.Text + ", Please enter a right Number.Thank you!";
                }

            }
            else
            {
                txt0PrintedQty.Focus();
                lbl_msg.Text = "Please enter a right Number.Thank you!";
            }
            // update
            if (_frm1._usePrintPI)
            {
                using (var db = new WHOperation.EF.WHO.dbWHOperation())
                {
                    PI_Print tmppiprint = new PI_Print();
                    tmppiprint.PI_Line = db.PI_Print.Max(p => p.PI_Line) + 1;
                    tmppiprint.PI_NO = _IdKey;
                    tmppiprint.PI_PART = tfpartno.Text;
                    tmppiprint.pi_mfgr_part = tfmfgpart.Text;
                    tmppiprint.PI_LOT = tfrirno.Text;
                    tmppiprint.PI_PO = _strPONum;
                    tmppiprint.pi_mfgr = _strPiMfgr;

                    tmppiprint.PI_QTY = Convert.ToDecimal(tfdnqty.Text);
                    tmppiprint.PI_Print_QTY = Convert.ToDecimal(txt0PrintedQty.Text.Trim()) - Convert.ToDecimal(_intOldPrintQty);

                    tmppiprint.pi_char1 = "Edit Printed Qty";

                    db.PI_Print.Add(tmppiprint);
                    db.SaveChanges();
                    _frm1.btn2PIID_Click(sender, e);
                    this.Close();
                }
            }
            else
            {
                string tmpmsg = "Edit Printed Qty " + (Convert.ToDecimal(txt0PrintedQty.Text.Trim()) - Convert.ToDecimal(_intOldPrintQty)).ToString();
                string cQuery = "update PIMLDetail set LineQty= '" + txt0PrintedQty.Text + "',NoOfLabels=NoofLabels+1,DeliveryNoteNo='" + tmpmsg + "' where DNNo='" + _IdKey + "' and PONo='" + _strPONum + "' and PartNumber='" + tfpartno.Text + "' and RIRNo='" + tfrirno.Text + "' and MFGPartNumber='" + tfmfgpart.Text + "' and DNQty='" + tfdnqty.Text + "'";

                _frm1.SQLUpdate(cQuery);
                _frm1.bGo_Click(sender, e);
                this.Close();
            }

        }

        private void txt0PrintedQty_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt0PrintedQty.Text))
            {
                if (_frm1.IsNumber(txt0PrintedQty.Text.Trim()))
                {
                    txt0PrintedQty.Focus();
                    lbl_msg.Text = "Please enter a right Number.Thank you!";
                }

            }
        }

        public string _IdKey { get; set; }

        public string _strPONum { get; set; }

        public string _intOldPrintQty { get; set; }

        public string _strPiMfgr { get; set; }

        private void btn2Print_Click(object sender, EventArgs e)
        {
            StreamReader sr = null;
            if (!string.IsNullOrEmpty(tfrirno.Text))
            {
                try
                {
                    string tmpfilepath = @"c:\tmp\pims" + tfrirno.Text.Trim()+".txt";
                    sr = new StreamReader(tmpfilepath, Encoding.UTF8);
                    string tmpreadstr = sr.ReadToEnd();
                    _frm1.toPrinterEnd(tmpreadstr);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Close();

                    }
                }

            }
        }
    }
}
