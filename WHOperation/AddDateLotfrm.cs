using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WHOperation.API;

namespace WHOperation
{
    public partial class AddDateLotfrm : Form
    {
        Form1 _frm1;
        KeyboardHook kh;

        public AddDateLotfrm()
        {
            InitializeComponent();
            getkeyG();
        }
        public AddDateLotfrm(Form1 frm1, DataGridView dgv)
        {
            InitializeComponent();


            _frm1 = frm1;

            initTxt(_frm1._dtPIRemoteIlistvpi_detWHO_VPrint, _frm1._dgvCurrRowIndex);

            getkeyG();

            num0fromDate.KeyDown += num0fromDate_KeyDown;
            num2ToDate.KeyDown += num0fromDate_KeyDown;
            num3FromLot.KeyDown += num3FromLot_KeyDown;
            num4ToLot.KeyDown += num3FromLot_KeyDown;

            if (_frm1._hasdateCodeToEnter > 0)
            {
                tf00DateCode.Focus();
            }
            else if (_frm1._hasLotNubmerToEnter > 0)
            {
                tf00LotNum.Focus();
            }
            else
            {
                tf00DateCode.Focus();
            }
        }

        void num3FromLot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                tf00LotNum.Focus();
            }
        }

        void num0fromDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode==Keys.Enter)
            {
                tf00DateCode.Focus();
            }
        }
        void getkeyG()
        {
            kh = new KeyboardHook();
            kh.SetHook();
            kh.OnKeyDownEvent += kh_OnKeyDownEvent;
        }
        void kh_OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == (Keys.S | Keys.Control)) { this.Show(); }//Ctrl+S显示窗口
            //if (e.KeyData == (Keys.H | Keys.Control)) { this.Hide(); }//Ctrl+H隐藏窗口
            //if (e.KeyData == (Keys.C | Keys.Control)) { this.Close(); }//Ctrl+C 关闭窗口 
            //if (e.KeyData == (Keys.A | Keys.Control | Keys.Alt)) { this.Text = "你发现了什么？"; }//Ctrl+Alt+A

            if (e.KeyCode == Keys.Down)
            {
                btn0Next_Click(sender, e);
            }
            if (e.KeyCode == Keys.Up)
            {
                btn0Pre_Click(sender, e);
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            kh.UnHook();
            base.OnClosing(e);
        }

        void initTxt(IList<EF.PI.vpi_detWHO_VPrint> vpi, int rowindex)
        {

            _frm1._dgvCurrRowIndex = rowindex;
            _frm1.dgv7PrintAll.Rows[rowindex].Cells[0].Selected = true;

            tf1rirno.Text = vpi[rowindex].PI_LOT;
            tf2mfgpart.Text = vpi[rowindex].pi_mfgr_part;
            tf3partno.Text = vpi[rowindex].PI_PART;
            tf4dnqty.Text = vpi[rowindex].PI_QTY.ToString("#,##");
            tf5CartonNo.Text = vpi[rowindex].PI_CARTON_NO;

            tf6Site.Text = vpi[rowindex].PI_SITE;
            tf7MPQ.Text = vpi[rowindex].PI_PO_price.ToString("#,##");
            tf8TTLQty.Text = vpi[rowindex].ttlQTY.ToString("#,##");
            tf9NumOfLabel.Text = vpi[rowindex].NumOfLabel.ToString("#,#0");
            tf10NumOfCarton.Text = vpi[rowindex].NumOfAllCarton.ToString("#,#0");

            tf01DateCode.Text = vpi[rowindex].pi_dateCode;

            tf01LotNum.Text = vpi[rowindex].pi_lotNumber;

            num0Go.Value = _frm1._dgvCurrRowIndex + 1;
            num0Go.Minimum = 1;
            num0Go.Maximum = vpi.Count;

            initNumber(vpi, rowindex);

            tf00LotNum.Text = "";
            tf00DateCode.Text = "";
        }
        void initNumber(IList<EF.PI.vpi_detWHO_VPrint> vpi, int rowindex)
        {
            num4ToLot.Minimum = 1;
            num3FromLot.Minimum = 1;
            num2ToDate.Minimum = 1;
            num0fromDate.Minimum = 1;

            num0fromDate.Maximum = vpi[rowindex].NumOfLabel;
            num2ToDate.Maximum = vpi[rowindex].NumOfLabel;
            num3FromLot.Maximum = vpi[rowindex].NumOfLabel;
            num4ToLot.Maximum = vpi[rowindex].NumOfLabel;

            num3FromLot.Value = 1;
            num4ToLot.Value = 1;

            num0fromDate.Value = 1;
            num2ToDate.Value = 1;

            _useLotMax = false;
            _useDateMax = false;
        }
        private void AddDateLotfrm_Load(object sender, EventArgs e)
        {

        }

        private void btn0Pre_Click(object sender, EventArgs e)
        {
            if (_frm1 == null)
            {
                return;
            }
            if (_frm1._dgvCurrRowIndex > -1 && _frm1._dgvCurrRowIndex <= _frm1._dtPIRemoteIlistvpi_detWHO_VPrint.Count)
            {
                if (_frm1._dgvCurrRowIndex > 0)
                {
                    _frm1._dgvCurrRowIndex--;
                }
                initTxt(_frm1._dtPIRemoteIlistvpi_detWHO_VPrint, _frm1._dgvCurrRowIndex);
            }
            else
            {
                initTxt(_frm1._dtPIRemoteIlistvpi_detWHO_VPrint, 0);
            }
        }

        private void btn0Next_Click(object sender, EventArgs e)
        {
            if (_frm1._dgvCurrRowIndex > -1 && _frm1._dgvCurrRowIndex < _frm1._dtPIRemoteIlistvpi_detWHO_VPrint.Count)
            {
                if (_frm1._dgvCurrRowIndex < (_frm1._dtPIRemoteIlistvpi_detWHO_VPrint.Count - 1))
                {
                    _frm1._dgvCurrRowIndex++;
                }
                initTxt(_frm1._dtPIRemoteIlistvpi_detWHO_VPrint, _frm1._dgvCurrRowIndex);
            }
            else
            {
                initTxt(_frm1._dtPIRemoteIlistvpi_detWHO_VPrint, 0);
            }
        }

        private void btn0Go_Click(object sender, EventArgs e)
        {
            initTxt(_frm1._dtPIRemoteIlistvpi_detWHO_VPrint, Convert.ToInt32(num0Go.Value - 1));
        }

        private void num0Go_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn0Go_Click(sender, e);
            }
        }
        bool checkFromTo(NumericUpDown nfrom, NumericUpDown nto, bool isfrom)
        {

            if (nfrom.Value > nto.Value)
            {
                if (isfrom)
                {
                    nfrom.Value = nto.Value;
                    nfrom.Focus();
                }
                else
                {
                    nto.Value = nfrom.Value;
                    nto.Focus();
                }
                lbl0Notice.Text = "Error: From " + nfrom.Value + " > To " + nto.Value;
                return false;
            }
            else
            {
                lbl0Notice.Text = "";
            }
            return true;
        }
        private void num0fromDate_ValueChanged(object sender, EventArgs e)
        {
            checkFromTo(num0fromDate, num2ToDate, true);
        }
        private void num2ToDate_ValueChanged(object sender, EventArgs e)
        {
            checkFromTo(num0fromDate, num2ToDate, false);
        }

        private void num3FromLot_ValueChanged(object sender, EventArgs e)
        {
            checkFromTo(num3FromLot, num4ToLot, true);
        }

        private void num4ToLot_ValueChanged(object sender, EventArgs e)
        {
            checkFromTo(num3FromLot, num4ToLot, false);
        }

        private void AddDateLotfrm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                btn0Pre_Click(sender, e);
            }
            if (e.KeyCode == Keys.Down)
            {
                btn0Next_Click(sender, e);
            }
        }

        private void btn1ResetDate_Click(object sender, EventArgs e)
        {
            tf00DateCode.Text = "";
            tf01DateCode.Text = "";
            initNumber(_frm1._dtPIRemoteIlistvpi_detWHO_VPrint, _frm1._dgvCurrRowIndex);
        }

        private void btn2ResetLot_Click(object sender, EventArgs e)
        {
            tf00LotNum.Text = "";
            tf01LotNum.Text = "";
            initNumber(_frm1._dtPIRemoteIlistvpi_detWHO_VPrint, _frm1._dgvCurrRowIndex);
        }

        private void btn0AddDateCode_Click(object sender, EventArgs e)
        {
            genDateLot(tf00DateCode, num0fromDate, num2ToDate, tf01DateCode, true);
        }

        private void genDateLot(TextBox tb00, NumericUpDown n0from, NumericUpDown n0To, TextBox tb01, bool isdatecode)
        {
            if (isdatecode)
            {
                if (_useDateMax)
                {
                    return;
                }
            }
            else
            {
                if (_useLotMax)
                {
                    return;
                }
            }
            if (!string.IsNullOrEmpty(tb00.Text.Trim()))
            {
                tb00.Text = tb00.Text.Replace("|", "").Replace(":", "").Trim();
                if (checkFromTo(n0from, n0To, false))
                {
                    for (decimal i = n0from.Value; i <= n0To.Value; i++)
                    {
                        if (string.IsNullOrEmpty(tb01.Text.Trim()))
                        {
                            tb01.Text = i + ":" + tb00.Text;
                        }
                        else
                        {
                            tb01.Text += "|" + i + ":" + tb00.Text;
                        }
                    }
                    if (n0To.Value < n0To.Maximum)
                    {
                        n0To.Value = n0To.Value + 1;
                    }
                    else
                    {
                        if (isdatecode)
                        {
                            _useDateMax = true;
                        }
                        else
                        {
                            _useLotMax = true;
                        }
                    }
                    n0from.Minimum = n0To.Value;
                    n0To.Minimum = n0To.Value;

                }
            }

            this.AcceptButton = null;
        }

        private void tf00DateCode_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = btn0AddDateCode;
        }
        private void tf00LotNum_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = btn00AddLotNumber;
        }
        private void btn00AddLotNumber_Click(object sender, EventArgs e)
        {
            genDateLot(tf00LotNum, num3FromLot, num4ToLot, tf01LotNum, false);
        }

        private void btn3Save_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tf01DateCode.Text.Trim()))
            {
                _frm1.dgv7PrintAll.Rows[_frm1._dgvCurrRowIndex].Cells["pi_dateCode"].Value = tf01DateCode.Text.Trim();
            }

            if (!string.IsNullOrEmpty(tf01LotNum.Text.Trim()))
            {
                _frm1.dgv7PrintAll.Rows[_frm1._dgvCurrRowIndex].Cells["pi_lotNumber"].Value = tf01LotNum.Text.Trim();
            }

        }






        public bool _useDateMax { get; set; }

        public bool _useLotMax { get; set; }
    }
}
