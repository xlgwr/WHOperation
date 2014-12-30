using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Runtime.InteropServices; // Needed for Marshal functions
using Code;
using System.Threading;
using System.Xml;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using System.Data.Entity;

using WHOperation.EF.WHO;
using WHOperation.EF.DW;

//using System.Runtime.InteropServices;
//using Microsoft.Win32.SafeHandles;
//batch
//batchj0b
namespace WHOperation
{
    public partial class Form1 : Form
    {
        WebReference.Service MFGProService = new WebReference.Service();

        DataSet dsDNDetail = new DataSet("dsDNDetail");
        DataSet _dsComplete = new DataSet();

        String _cConnStr = "Persist Security Info=False;User ID=appuser;pwd=application;Initial Catalog=dbWHOperation;Data Source=142.2.70.81;pooling=true";
        String _cConnStrPI = "server=142.2.70.53;database=pi;uid=pi;";
        String cUserID, cLastLabel;
        List<String> lXML = new List<String>();
        List<byte[]> lVendorLabelImage = new List<byte[]>();
        List<vendorLabelDefinition> lVendorLabel = new List<vendorLabelDefinition>();
        String cTemplateType, c2DSeperator;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        Thread readerThread;
        CaptureBarCode bcReader = new CaptureBarCode();
        IntPtr CodeReaderhandle;
        int cDisable;
        DateTime cLastPrint;
        int cSearchEnable;
        //add new qr listen
        KeyBordHook kbh;
        public static string getStrQRcode = "";
        DateTime _dt;  //定义一个成员函数用于保存每次的时间点
        int _spanint = 30;
        string _strold = "";
        string _strnew = "";
        // add new by xlgwr

        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        public static string _splitPrefix = @"D;9D;1P;Q;1T;P;T;PKOA-;PKOA/;PKOA+;3N1;3N2";
        public List<prefixCheckbox> _splitStrTample;
        public static string getQRcode = "";
        public string _strtmp;
        public int _firstOpenSelectList;
        public static List<char> _splitChar_list = new List<char>();
        public string _splitStringTmp { get; set; }

        public List<string> _strScanlit = new List<string>();
        public List<string> _strlit = new List<string>();
        public List<string> _strNoPrefixlit { get; set; }
        public List<string> _strNoPrefixlitTmp { get; set; }

        public static List<prefixContent> _prefixcontList;
        public List<prefixContent> _scanList { get; set; }

        public class prefixCheckbox
        {
            public string _split { get; set; }
            public CheckBox _cb { get; set; }
            public prefixCheckbox() { }
            public prefixCheckbox(string split, CheckBox cb)
            {
                _split = split;
                _cb = cb;
            }
        }
        public class prefixContent
        {
            public string _prefix { get; set; }
            public Control _cl { get; set; }
            public bool _currcl { get; set; }
            public bool _currclSplit { get; set; }

            public prefixContent() { }
            public prefixContent(string p, Control c)
            {
                _prefix = p;
                _cl = c;
            }
            public prefixContent(string p, Control c, bool currcl, bool currclSplit)
            {
                _prefix = p;
                _cl = c;
                _currcl = currcl;
                _currclSplit = currclSplit;
            }
        }
        //end by xlgwr

        public struct cCaptureData
        {
            public String cDNPartumber;
            public String cMFGPart;
            public String cDateCode;
            public String cMfgDate;
            public String cExpiredate;
            public String cRecQty;
            public String cLotNumber;
            public Image cPMFGPart;
            public Image cPDateCode;
            public Image cPMfgDate;
            public Image cPExpiredate;
            public Image cPRecQty;
            public Image cPLotNumber;
            public Image cPDNPartNumber;
        };

        cCaptureData cBufferData;

        dbWHOperation _dbWHOperation;
        public Form1()
        {
            InitializeComponent();

            _strNoPrefixlit = new List<string>();
            _strNoPrefixlitTmp = new List<string>();
            _dbWHOperation = new dbWHOperation();

            this.FormClosing += new FormClosingEventHandler(this.Form1_FormClosing);


        }

        protected override void OnLoad(EventArgs e)
        {
            dgv1Pending.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
            dgv5PIPending.SelectionChanged += new EventHandler(dgv5PIPending_SelectionChanged);

            dgv3VendorTemplate.SelectionChanged += new EventHandler(dataGridView3_SelectionChanged);
            dgv0DNNumber.SelectionChanged += new EventHandler(dgDNNumber_SelectionChanged);
            //this.tflotno.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandler);
            this.tfscanarea.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandlerscanArea);

            //add by xlgwr
            _prefixcontList = new List<prefixContent>();
            //scan model
            initScanList();
            //end by xlgwr

            //this.txt0ListKeyMsg.KeyDown+=new System.Windows.Forms.KeyEventHandler(this.OnKeyDownHandlerscanArea);
            //this.pb1.MouseHover += new EventHandler (this.pb1_MouseOverHandle);
            //this.pb1.MouseLeave  += new EventHandler(this.pb1_MouseLeaveHandle);
            cbport.SelectedIndex = 0;
            cbprintertype.SelectedIndex = 0;
            cbsystem.Text = GlobalClass1.systemID;
            cUserID = GlobalClass1.userID;
            try
            {
                MFGProService.GetTable(cbsystem.Text, "wsas001", tfdnno.Text + "," + tfdndate.Text + "," + tftodndate.Text);
                //MFGProService.GetTable(cbsystem.Text, "wsas001", tfdnno.Text + "," + tfdndate.Text); 
            }
            catch (Exception ex) { }
            cTemplateType = ""; c2DSeperator = ""; cLastPrint = DateTime.Now;
            cBufferData = new cCaptureData();
            cSearchEnable = 0;
            tfdndate.Text = DateTime.Now.AddDays(-3).Date.ToString();
            tftodndate.Text = DateTime.Now.Date.ToString();
            base.OnLoad(e);
        }

        void dgv5PIPending_SelectionChanged(object sender, EventArgs e)
        {
            resetForm(1);
            setPIMLData();
            //getTemplate();
            setMandField();
            tabControl1.SelectedIndex = 1;
            //   var qty = dgv5PIPending.CurrentRow.Cells["PI_QTY"].Value;
            //  var pqty = dgv5PIPending.CurrentRow.Cells["PI_Print_QTY"].Value;
            //  tool_lbl_Msg.Text = qty + "," + pqty + ":" + qty.Equals(pqty) + ",dec" + Convert.ToDecimal(qty).ToString("#,###") + ":" + Convert.ToDecimal(pqty).ToString("#,###") + ":" + Convert.ToDecimal(qty).ToString("#,###").Equals(Convert.ToDecimal(pqty).ToString("#,###"));
        }
        public string getPrefixOfContent(string item)
        {
            foreach (var fc in _prefixcontList)
            {
                if (item.StartsWith(fc._prefix, true, null))
                {
                    fc._cl.Text = item.Substring(fc._prefix.Length);
                    //_strlit.Add(item);
                    return fc._cl.Text;
                }
            }
            _strNoPrefixlitTmp.Add(item);
            return item;
        }
        void dgDNNumber_SelectionChanged(object sender, EventArgs e)
        {
            handleDNChange();
            //getTemplate();

            enableScan();
        }
        void handleDNChange()
        {

            DataGridViewRow cDGR = new DataGridViewRow();
            DataRow cR;
            DataTable dt = new DataTable();
            String cSelDNNo;
            int i = 0;
            Double cDNQty, cPrintQty;
            if (dsDNDetail.Tables.Count < 7)
                return;

            dt = (DataTable)dsDNDetail.Tables[6];
            cDGR = dgv0DNNumber.CurrentRow;
            cSelDNNo = cDGR.Cells["DNNumber"].Value.ToString();
            dgv1Pending.Rows.Clear();

            while (i <= dsDNDetail.Tables[6].Rows.Count - 1)
            {
                cR = dsDNDetail.Tables[6].Rows[i];
                dsDNDetail.Tables[6].Rows[i]["RowID"] = i.ToString();
                if (cR.ItemArray[0].ToString().ToUpper() == cSelDNNo.ToUpper())
                {

                    cDNQty = Convert.ToDouble(cR.ItemArray[6].ToString());
                    cPrintQty = getCompleteQty(cR["t_dn"].ToString(), cR["t_po"].ToString(), cR["t_id"].ToString(), cR["t_rir"].ToString(), cR["t_deli_date"].ToString(), cR["t_supp"].ToString());
                    /*if (cR.ItemArray[20].ToString().Length == 0)
                        cPrintQty = 0;
                    else
                        cPrintQty = Convert.ToDouble(cR.ItemArray[20].ToString()); */

                    cR["PrintedQty"] = cPrintQty;
                    if (cDNQty > cPrintQty)
                        dgv1Pending.Rows.Add(cR.ItemArray[0], cR.ItemArray[10], cR.ItemArray[7], cR["t_part"], cR["t_mfgr_part"], cR["t_rir"], cR.ItemArray[4], "", cR.ItemArray[6], cR.ItemArray[1], cR.ItemArray[5], cR.ItemArray[11], cR.ItemArray[12], cR.ItemArray[13], cR.ItemArray[14], cR.ItemArray[15], cR.ItemArray[16], cR.ItemArray[17], cR.ItemArray[18], cR.ItemArray[20], i.ToString());

                }
                i += 1;
            }
            setCompleteDN();


        }
        Double getCompleteQty(String cDNNo, String cPoNo, String cPoLine, String cRIRNo, String cDNDate, String cVendorID)
        {
            double cRet, cPQty;
            String cQuery, cTotQty;
            SqlDataReader myReader;
            cTotQty = "0";
            //cQuery = "select case when sum(LineQty) is null then 0 else sum(LineQty) end from PIMLDetail where DNNo='" + cDNNo + "' and PONo='" + cPoNo + "' and PoLine='" + cPoLine + "' and RIRNo='" + cRIRNo + "' and DNDate='" + cDNDate + "' and VendorID='" + cVendorID + "'";
            cQuery = "select case when sum(LineQty) is null then 0 else sum(LineQty) end from PIMLDetail where DNNo='" + cDNNo + "' and PONo='" + cPoNo + "' and RIRNo='" + cRIRNo + "' and DNDate='" + cDNDate + "' and VendorID='" + cVendorID + "'";
            try
            {
                using (SqlConnection conn = new SqlConnection(_cConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(cQuery, conn);
                    myReader = cmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        cTotQty = myReader.GetValue(0).ToString();
                        cPQty = Convert.ToDouble(cTotQty);
                        //cTotQty = (Convert.ToDouble(cTotQty) + cPQty).ToString();
                        cTotQty = (Convert.ToDouble(cPQty)).ToString();
                    }
                    myReader.Close();
                }
            }
            catch (Exception ex) { }
            cRet = Convert.ToDouble(cTotQty);
            return cRet;
        }
        void setCompleteDN()
        {
            String cQuery, cDNNo;
            SqlDataReader myReader;
            cDNNo = dgv0DNNumber.CurrentRow.Cells["DNNumber"].Value.ToString();
            cQuery = "select PartNumber,PONo,MFGPartNumber,'',RIRNo,DNQty,LineQty from PIMLDetail where DNNo='" + cDNNo + "' ";
            dgv1Complete.Rows.Clear();
            try
            {
                using (SqlConnection conn = new SqlConnection(_cConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(cQuery, conn);
                    myReader = cmd.ExecuteReader();
                    String[] cRec = new String[myReader.FieldCount];
                    int i;
                    while (myReader.Read())
                    {
                        i = 0;
                        for (i = 0; i <= myReader.FieldCount - 1; i += 1)
                        {
                            cRec[i] = myReader.GetValue(i).ToString();
                        }
                        dgv1Complete.Invoke(new Action(delegate() { dgv1Complete.Rows.Add(cRec); }));
                    }
                    myReader.Close();
                }
            }
            catch (Exception ex) { }

        }
        void setDSPrintedQty()
        {
            DataGridViewRow cR;
            String cPrintedQty, cCurrRow;
            Double dPrintedQty, dDNQty;
            int i;
            try
            {
                //cR = dataGridView1.CurrentRow;
                cR = dgv1Pending.SelectedRows[0];
                cCurrRow = cR.Cells["RowID"].Value.ToString();
                i = Convert.ToInt32(cCurrRow);
                cPrintedQty = dsDNDetail.Tables[6].Rows[i]["PrintedQty"].ToString();

                if (cPrintedQty.Length == 0)
                    cPrintedQty = "0";
                dPrintedQty = 0;
                dPrintedQty = Convert.ToDouble(cPrintedQty) + Convert.ToDouble(tfrecqty.Text);

                dsDNDetail.Tables[6].Rows[i]["PrintedQty"] = dPrintedQty.ToString();

                dDNQty = Convert.ToDouble(cR.Cells["DNQty"].Value);
                cR.Cells["PrintedQty"].Value = dPrintedQty.ToString();
                if (dDNQty <= dPrintedQty)
                {
                    dgv1Pending.Invoke(new Action(delegate() { dgv1Pending.Rows.Remove(cR); }));
                }
            }
            catch (Exception ex) { }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_dbWHOperation != null)
                {
                    _dbWHOperation.Dispose();
                }
                if (readerThread.IsAlive)
                {
                    StopCodeReader(CodeReaderhandle);
                    readerThread.Abort();
                }
            }
            catch (Exception) { }
        }
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                completeTrans();
            }
        }
        private void OnKeyDownHandlerscanArea(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return || e.KeyValue == 13)
            {
                if (tfscanarea.Text.ToUpper() == "SAVE" || tfscanarea.Text.ToUpper() == "PRINT")
                    completeTrans();
                else
                {

                    ParseLabelData();
                    //GrabLabelData();
                }
            }
        }
        private void pb1_MouseOverHandle(object sender, EventArgs e)
        {
            groupBox4.Height = 510;
            groupBox4.Width = 510;
            pb1.Height = 505;
            pb1.Width = 505;
            dgv3VendorTemplate.Visible = false;
            Point x = new Point();
            x.X = groupBox4.Location.X + 5;
            x.Y = groupBox4.Location.Y + 5;
            pb1.Location = x;
        }
        private void pb1_MouseLeaveHandle(object sender, EventArgs e)
        {
            groupBox4.Height = 270;
            groupBox4.Width = 345;
            pb1.Height = 105;
            pb1.Width = 165;
            dgv3VendorTemplate.Visible = true;
            Point x = new Point();
            x.X = 165;
            x.Y = 20;
            pb1.Location = x;

        }
        int completeTrans()
        {
            String cLot;
            int cVal;
            //cLot = tflotno.Text;
            //cLot = cLot.Replace(Convert.ToChar(13).ToString(),"");
            //tflotno.Text = cLot;
            cVal = valData();
            if (cVal == 0)
            {
                updData();
                //tflotno.Text = "";
            }
            else
            {
                //MessageBox.Show("Data Validation failed");
            }
            getQRcode = "";
            _strNoPrefixlit.Clear();
            _strNoPrefixlitTmp.Clear();
            return cVal;
        }
        private void bGetDNDetail_Click(object sender, EventArgs e)
        {
            /*
            For testing...
            String xmlData;
            lVendorLabel = new List<vendorLabelDefinition>();
            xmlData = "<Header><Field><Name>LOTNUMBER</Name><Prefix>&lt;LL&gt;</Prefix></Field> " +
                              "<Field><Name>RECQTY</Name><Prefix>LQ</Prefix></Field> " +
                              "<Field><Name>DATECODE</Name><Prefix>DC</Prefix></Field> " +
                              "<Field><Name>expiredate</Name><Prefix>ex</Prefix></Field> " +
                              "<type>Single</type>" +
                              "</Header>";
            setFields(lVendorLabel = parseTempXMLTest(xmlData));
            GrabLabelData(); */
            getMFGDNData();
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            resetForm(1);
            setPIMLData();
            //getTemplate();
            setMandField();
        }
        void setFields()
        {
            if (_usePrintPI)
            {
                if (dgv5PIPending.Rows.Count <= 0)
                {

                    tflotno.Visible = true;
                    tfmfgdate.Visible = true;
                    tfexpiredate.Visible = true;
                    tfdatecode.Visible = true;
                    tfrecmfgrpart.Visible = true;
                    tfdnpartnumber.Visible = true;

                    llotnumber.Visible = true;
                    lmfgdate.Visible = true;
                    lexpiredate.Visible = true;
                    ldatecode.Visible = true;
                    lrecmfgpart.Visible = true;
                    ldnpartnumber.Visible = true;
                    lMRecPartNumber.Visible = true;
                    pblotnumber.Visible = true;
                    pbmfgdate.Visible = true;
                    pbexpiredate.Visible = true;
                    pbdatecode.Visible = true;
                    pbrecmfgpart.Visible = true;
                    pbdnpartnumber.Visible = true;
                    return;
                }
            }
            else
            {

                if (dgv3VendorTemplate.Rows.Count <= 0)
                {

                    tflotno.Visible = true;
                    tfmfgdate.Visible = true;
                    tfexpiredate.Visible = true;
                    tfdatecode.Visible = true;
                    tfrecmfgrpart.Visible = true;
                    tfdnpartnumber.Visible = true;

                    llotnumber.Visible = true;
                    lmfgdate.Visible = true;
                    lexpiredate.Visible = true;
                    ldatecode.Visible = true;
                    lrecmfgpart.Visible = true;
                    ldnpartnumber.Visible = true;
                    lMRecPartNumber.Visible = true;
                    pblotnumber.Visible = true;
                    pbmfgdate.Visible = true;
                    pbexpiredate.Visible = true;
                    pbdatecode.Visible = true;
                    pbrecmfgpart.Visible = true;
                    pbdnpartnumber.Visible = true;
                    return;
                }
            }
        }
        void setFields(List<vendorLabelDefinition> vendorLabel)
        {
            String cFieldName, cPrefix, cIndex;
            tflotno.Visible = false;
            tfmfgdate.Visible = false;
            tfexpiredate.Visible = false;
            tfdatecode.Visible = false;
            tfrecmfgrpart.Visible = false;
            tfdnpartnumber.Visible = false;

            llotnumber.Visible = false;
            lmfgdate.Visible = false;
            lexpiredate.Visible = false;
            ldatecode.Visible = false;
            lrecmfgpart.Visible = false;
            ldnpartnumber.Visible = false;
            lMRecPartNumber.Visible = false;
            pblotnumber.Visible = false;
            pbmfgdate.Visible = false;
            pbexpiredate.Visible = false;
            pbdatecode.Visible = false;
            pbrecmfgpart.Visible = false;
            pbdnpartnumber.Visible = false;


            int i = 0;
            while (i <= vendorLabel.Count - 1)
            {
                cFieldName = vendorLabel[i].cFieldName;
                cPrefix = vendorLabel[i].cPrefix;
                cIndex = vendorLabel[i].cIndex;
                if (cFieldName.ToUpper() == "LOTNUMBER")
                {
                    tflotno.Visible = true;
                    llotnumber.Visible = true;
                    if (cPrefix.Length > 0 || cIndex.Length > 0)
                        pblotnumber.Visible = true;
                    else
                        pblotnumber.Visible = false;
                }
                if (cFieldName.ToUpper() == "MFGDATE")
                {
                    tfmfgdate.Visible = true;
                    lmfgdate.Visible = true;
                    if (cPrefix.Length > 0 || cIndex.Length > 0)
                        pbmfgdate.Visible = true;
                    else
                        pbmfgdate.Visible = false;

                }
                if (cFieldName.ToUpper() == "EXPIREDATE")
                {
                    tfexpiredate.Visible = true;
                    lexpiredate.Visible = true;
                    if (cPrefix.Length > 0 || cIndex.Length > 0)
                        pbexpiredate.Visible = true;
                    else
                        pbexpiredate.Visible = false;
                }
                if (cFieldName.ToUpper() == "DATECODE")
                {
                    tfdatecode.Visible = true;
                    ldatecode.Visible = true;
                    if (cPrefix.Length > 0 || cIndex.Length > 0)
                        pbdatecode.Visible = true;
                    else
                        pbdatecode.Visible = false;
                }
                if (cFieldName.ToUpper() == "MFGRPART")
                {
                    tfrecmfgrpart.Visible = true;
                    lrecmfgpart.Visible = true;
                    if (cPrefix.Length > 0 || cIndex.Length > 0)
                        pbrecmfgpart.Visible = true;
                    else
                        pbrecmfgpart.Visible = false;
                }
                if (cFieldName.ToUpper() == "DNPARTNUMBER")
                {
                    tfdnpartnumber.Visible = true;
                    ldnpartnumber.Visible = true;
                    if (cPrefix.Length > 0 || cIndex.Length > 0)
                        pbdnpartnumber.Visible = true;
                    else
                        pbdnpartnumber.Visible = false;
                }
                i += 1;
            }
        }

        void ParseLabelData(string strscan)
        {
            String cCompoundData, cSingleLabel;
            String[] cArrayData;
            int i;
            cCompoundData = strscan;
            cCompoundData = cCompoundData.Replace("\n", "");
            cCompoundData = cCompoundData.Replace("\r", "");
            if (cCompoundData.Length >= 3)
            {
                if (cCompoundData.Substring(0, 3) != "<|>")
                {
                    cCompoundData = "<|>" + cCompoundData;
                }
            }
            cArrayData = cCompoundData.Split(new string[] { "<|>" }, StringSplitOptions.None);
            if (cTemplateType.ToUpper() == "SINGLE")
            {
                i = 0;
                while (i <= cArrayData.Length - 1)
                {
                    cSingleLabel = cArrayData[i];
                    GrabLabelData(cSingleLabel);
                    i += 1;
                }
            }
            else if (cTemplateType.ToUpper() == "COMPOUND")
            {
                if (c2DSeperator.Length > 0)
                {
                    cArrayData = cArrayData[1].Split(new string[] { c2DSeperator }, StringSplitOptions.None);
                }
                Grab2DData(cArrayData);
            }
            else
            {
                i = 0;
                while (i <= cArrayData.Length - 1)
                {
                    cSingleLabel = cArrayData[i];
                    GrabGeneralData(cSingleLabel);
                    i += 1;
                }
            }
        }
        void ParseLabelData()
        {
            String cCompoundData, cSingleLabel;
            String[] cArrayData;
            int i;
            cCompoundData = tfscanarea.Text;
            cCompoundData = cCompoundData.Replace("\n", "");
            cCompoundData = cCompoundData.Replace("\r", "");
            if (cCompoundData.Length >= 3)
            {
                if (cCompoundData.Substring(0, 3) != "<|>")
                {
                    cCompoundData = "<|>" + cCompoundData;
                }
            }
            cArrayData = cCompoundData.Split(new string[] { "<|>" }, StringSplitOptions.None);

            if (cTemplateType.ToUpper() == "SINGLE")
            {
                i = 0;
                while (i <= cArrayData.Length - 1)
                {
                    cSingleLabel = cArrayData[i];
                    GrabLabelData(cSingleLabel);
                    i += 1;
                }
            }
            else if (cTemplateType.ToUpper() == "COMPOUND")
            {

                if (c2DSeperator.Length > 0)
                {
                    if (cArrayData.Length < 2)
                    {
                        return;
                    }
                    cArrayData = cArrayData[1].Split(new string[] { c2DSeperator }, StringSplitOptions.None);
                }
                Grab2DData(cArrayData);
            }
            else
            {
                i = 0;
                while (i <= cArrayData.Length - 1)
                {
                    cSingleLabel = cArrayData[i];
                    GrabGeneralData(cSingleLabel);
                    i += 1;
                }
            }
            tfscanarea.Invoke(new Action(delegate() { tfscanarea.Text = ""; }));
            tfscanarea.Invoke(new Action(delegate() { tfscanarea.Text = tfscanarea.Text.Replace("\n", ""); }));
            tfscanarea.Invoke(new Action(delegate() { tfscanarea.Text = tfscanarea.Text.Replace("\r", ""); }));

            //add by xlgwr

            foreach (var item in cArrayData)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (!_strScanlit.Contains(item))
                    {
                        //
                        //getPrefixOfContent(item);


                        lib0ScanDataListBox.Items.Add(item);
                        _strScanlit.Add(item);

                        //find in gridview
                        if (chk5NoSplit.Checked)
                        {
                            searchByItem(item);
                            searchByItemByPrefix(item, _splitPrefix, lib0ScanDataListBox);
                        }
                        ///end

                    }
                }
            }
        }
        public void searchByItemByPrefix(string item, string strprefix, ListBox libAdd)
        {
            var tmpspalit = strprefix.Split(';');
            foreach (var ckey in tmpspalit)
            {
                if (item.StartsWith(ckey, StringComparison.OrdinalIgnoreCase))
                {
                    var tmpitem = item.Substring(ckey.Length);

                    if (!_strScanlit.Contains(tmpitem))
                    {
                        libAdd.Items.Add(tmpitem);
                        _strScanlit.Add(tmpitem);
                    }
                    searchByItem(tmpitem);
                    break;
                }
            }
        }
        public bool searchByItem(string item)
        {
            if (!IsNumber(item.ToString().ToUpper()))
            {

                if (_usePrintPI)
                {
                    SearchDNPart2(item.ToUpper().Trim(), dgv5PIPending, "PI_PART", "pi_mfgr_part");
                }
                else
                {
                    SearchDNPart2(item.ToUpper().Trim(), dgv1Pending, "PartNumber", "MFGPartNo");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(tfdnqty.Text))
                {
                    return false;
                }
                int intitem = Convert.ToInt32(item.ToString().Trim());
                var tmpint = Convert.ToInt32(tfnoofcartons.Text) * Convert.ToInt32(tfnooflabels.Text) * intitem;


                if (tmpint > Convert.ToInt32(tfdnqty.Text) && string.IsNullOrEmpty(tfrecqty.Text))
                {
                    tool_lbl_Msg.Text = "超出 dn qty 数量:" + tfnoofcartons.Text + " * " + tfnooflabels.Text + " * " + intitem.ToString("###") + " = " + tmpint + " > " + tfdnqty.Text;
                    tfrecqty.Text = "";
                    pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                    return false;
                }
                else
                {
                    if (_usePrintPI)
                    {
                        var tmpmpq = dgv5PIPending.SelectedRows[0].Cells["PI_PO_price"].Value.ToString();
                        if (!string.IsNullOrEmpty(tmpmpq))
                        {
                            var tmp2mpq = Convert.ToDecimal(tmpmpq).ToString("###").ToString();
                            if (!tmp2mpq.Equals(intitem.ToString("###")))
                            {
                                tool_lbl_Msg.Text = "Enter Nubmer:" + item + " is not Equals MPQ:" + tmp2mpq;
                                return false;
                            }

                        }
                    }
                    tfrecqty.Invoke(new Action(delegate()
                    {
                        tfrecqty.Text = intitem.ToString("###");
                        pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }));
                }
            }
            return true;
        }
        void GrabGeneralData(String cLabelData)
        {
            String cFieldName, cPrefix, cSeperator;
            int cIndex;
            Char cSplitter;
            String[] aPrefix;
            int i = 0;

            if (cLabelData.Length == 0)
                return;
            while (i <= lVendorLabel.Count - 1)
            {
                cFieldName = lVendorLabel[i].cFieldName;
                cPrefix = lVendorLabel[i].cPrefix;
                cSeperator = lVendorLabel[i].cSeperator;
                //if (cPrefix.Length == 0 || cPrefix.Length > cLabelData.Length) { i += 1; continue; }
                if (cPrefix.Length == 0) { i += 1; continue; }

                if (lVendorLabel[i].cIndex.Length > 0)
                    cIndex = Convert.ToInt32(lVendorLabel[i].cIndex);
                else
                    cIndex = 1;
                aPrefix = cPrefix.Split(';');
                int cLoopPrefix;
                cLoopPrefix = 0;
                while (cLoopPrefix <= aPrefix.Length - 1)
                {
                    String[] cTemp;
                    cPrefix = aPrefix[cLoopPrefix];
                    if (cPrefix.Length == 0 || cPrefix.Length > cLabelData.Length) { cLoopPrefix += 1; continue; }
                    if (cPrefix.ToUpper() == cLabelData.Substring(0, cPrefix.Length).ToUpper())
                    {
                        if (cPrefix.Length == 0) { cLoopPrefix += 1; continue; }
                        if (cFieldName.ToUpper() == "LOTNUMBER")
                        {
                            //tflotno.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                            tflotno.Invoke(new Action(delegate() { tflotno.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                            if (cSeperator.Length > 0)
                            {
                                if (cSeperator == "SPACE")
                                    cSplitter = ' ';
                                else
                                    cSplitter = cSeperator[0];
                                cTemp = tflotno.Text.Split(cSplitter);
                                if (cTemp.Length >= cIndex)
                                    //tflotno.Text = cTemp[cIndex-1];
                                    tflotno.Invoke(new Action(delegate() { tflotno.Text = cTemp[cIndex - 1]; }));
                            }
                            //tflotno.Text = tflotno.Text.Trim();
                            tflotno.Invoke(new Action(delegate() { tflotno.Text = tflotno.Text.Trim(); }));
                            pblotnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                        }
                        else if (cFieldName.ToUpper() == "MFGDATE")
                        {
                            tfmfgdate.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                            if (cSeperator.Length > 0)
                            {
                                if (cSeperator == "SPACE")
                                    cSplitter = ' ';
                                else
                                    cSplitter = cSeperator[0];
                                cTemp = tfmfgdate.Text.Split(cSplitter);
                                if (cTemp.Length >= cIndex)
                                    tfmfgdate.Text = cTemp[cIndex - 1];
                            }
                            tfmfgdate.Text = tfmfgdate.Text.Trim();
                            pbmfgdate.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                        }
                        else if (cFieldName.ToUpper() == "EXPIREDATE")
                        {
                            tfexpiredate.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                            if (cSeperator.Length > 0)
                            {
                                if (cSeperator == "SPACE")
                                    cSplitter = ' ';
                                else
                                    cSplitter = cSeperator[0];
                                cTemp = tfexpiredate.Text.Split(cSplitter);
                                if (cTemp.Length >= cIndex)
                                    tfexpiredate.Text = cTemp[cIndex - 1];
                            }
                            tfexpiredate.Text = tfexpiredate.Text.Trim();
                            pbexpiredate.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                        }
                        else if (cFieldName.ToUpper() == "RECQTY")
                        {
                            //tfrecqty.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                            tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                            if (cSeperator.Length > 0)
                            {
                                if (cSeperator == "SPACE")
                                    cSplitter = ' ';
                                else
                                    cSplitter = cSeperator[0];
                                cTemp = tfrecqty.Text.Split(cSplitter);
                                if (cTemp.Length >= cIndex)
                                    //tfrecqty.Text = cTemp[cIndex-1];
                                    tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = cTemp[cIndex - 1]; }));
                            }
                            //tfrecqty.Text = tfrecqty.Text.Trim();
                            //tfrecqty.Text = tfrecqty.Text.Replace(",", "");
                            tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = tfrecqty.Text.Trim(); }));
                            tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = tfrecqty.Text.Replace(",", ""); }));
                            pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                        }
                        else if (cFieldName.ToUpper() == "DATECODE")
                        {
                            //tfdatecode.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                            tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                            if (cSeperator.Length > 0)
                            {
                                if (cSeperator == "SPACE")
                                    cSplitter = ' ';
                                else
                                    cSplitter = cSeperator[0];
                                cTemp = tfdatecode.Text.Split(cSplitter);
                                if (cTemp.Length >= cIndex)
                                    //tfdatecode.Text = cTemp[cIndex-1];
                                    tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = cTemp[cIndex - 1]; }));
                            }
                            //tfdatecode.Text = tfdatecode.Text.Trim();
                            tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = tfdatecode.Text.Trim(); }));
                            pbdatecode.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                        }
                        else if (cFieldName.ToUpper() == "DNPARTNUMBER")
                        {
                            tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                            if (cSeperator.Length > 0)
                            {
                                if (cSeperator == "SPACE")
                                    cSplitter = ' ';
                                else
                                    cSplitter = cSeperator[0];
                                cTemp = tfdnpartnumber.Text.Split(cSplitter);
                                if (cTemp.Length >= cIndex)
                                    tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = cTemp[cIndex - 1]; }));
                            }
                            tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = tfdnpartnumber.Text.Trim(); }));
                            pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                            if (cbSmartScan.Checked == true)
                            {
                                if (tfdnpartnumber.Text.Length > 0 && tfrecmfgrpart.Text.Length > 0 && cSearchEnable == 0)
                                {
                                    SearchDNPart();
                                }
                            }
                        }

                        else if (cFieldName.ToUpper() == "MFGRPART")
                        {
                            //tfrecmfgrpart.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                            tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                            if (cSeperator.Length > 0)
                            {
                                if (cSeperator == "SPACE")
                                    cSplitter = ' ';
                                else
                                    cSplitter = cSeperator[0];
                                cTemp = tfrecmfgrpart.Text.Split(cSplitter);
                                if (cTemp.Length >= cIndex)
                                    //tfrecmfgrpart.Text = cTemp[cIndex - 1];
                                    tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = cTemp[cIndex - 1]; }));
                            }
                            //tfrecmfgrpart.Text = tfrecmfgrpart.Text.Trim();
                            if (cbtrimmfgpart.Checked)
                                tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = tfrecmfgrpart.Text.Replace(" ", ""); tfrecmfgrpart.Text = tfrecmfgrpart.Text.Trim(); }));
                            else
                                tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = tfrecmfgrpart.Text.Trim(); }));
                            pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                            if (cbSmartScan.Checked == true)
                            {
                                if (cSearchEnable == 0)
                                {
                                    if (tfdnpartnumber.Visible)
                                    {
                                        if (tfdnpartnumber.Text.Length > 0 && tfrecmfgrpart.Text.Length > 0)
                                        {
                                            SearchDNPart();
                                        }
                                    }
                                    else
                                    {
                                        if (tfrecmfgrpart.Text.Length > 0)
                                        {
                                            tfdnpartnumber.Text = tfpartno.Text;
                                            SearchDNPart();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    cLoopPrefix += 1;
                }
                i += 1;
            }
            cLabelData = "";
            cLastLabel = "";
        }
        void Grab2DData(String[] c2DDataArray)
        {
            int i, cIndex, cCo;
            String cFieldName, cPrefix, cSeperator, cLabelData;
            cCo = 0;
            i = 0;
            while (i <= lVendorLabel.Count - 1)
            {
                if (lVendorLabel[i].cIndex.Length > 0)
                    cCo += 1;
                i += 1;
            }
            i = 0;
            if (c2DDataArray.Length < cCo)
                return;
            while (i <= lVendorLabel.Count - 1)
            {
                cFieldName = lVendorLabel[i].cFieldName;
                cPrefix = lVendorLabel[i].cPrefix;
                cSeperator = lVendorLabel[i].cSeperator;
                if (lVendorLabel[i].cIndex.Length > 0)
                    cIndex = Convert.ToInt32(lVendorLabel[i].cIndex);
                else
                    cIndex = 0;
                if (cIndex == 0)
                {
                    i += 1;
                    continue;
                }
                if (c2DDataArray.Length < cIndex)
                {
                    i += 1;
                    continue;
                }
                cLabelData = c2DDataArray[cIndex - 1];
                cLabelData = cLabelData.Trim();
                if (cPrefix.Length > 0 && cLabelData.Length > 0)
                    cLabelData = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length).ToUpper();

                if (cFieldName.ToUpper() == "LOTNUMBER")
                {

                    tflotno.Invoke(new Action(delegate() { tflotno.Text = cLabelData; }));
                    pblotnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                }
                if (cFieldName.ToUpper() == "MFGDATE")
                {
                    tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.Text = cLabelData; }));
                    pbmfgdate.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                }
                if (cFieldName.ToUpper() == "EXPIREDATE")
                {
                    tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.Text = cLabelData; }));
                    pbexpiredate.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                }
                if (cFieldName.ToUpper() == "RECQTY")
                {
                    //tfrecqty.Text = cLabelData;
                    tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = cLabelData; }));
                    pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    /*if (cSeperator.Length > 0)
                    {
                        if (cSeperator == "SPACE")
                            cSplitter = ' ';
                        else
                            cSplitter = cSeperator[0];
                        cTemp = tfrecqty.Text.Split(cSplitter);
                        if (cTemp.Length >= cIndex)
                            //tfrecqty.Text = cTemp[cIndex-1];
                            tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = cTemp[cIndex - 1]; }));
                    }
                    //tfrecqty.Text = tfrecqty.Text.Trim();
                    //tfrecqty.Text = tfrecqty.Text.Replace(",", "");
                    tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = tfrecqty.Text.Trim(); }));
                    tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = tfrecqty.Text.Replace(",", ""); }));
                    pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png"); */

                }
                if (cFieldName.ToUpper() == "DATECODE")
                {
                    tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = cLabelData; }));
                    pbdatecode.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                }
                if (cFieldName.ToUpper() == "DNPARTNUMBER")
                {
                    tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = cLabelData; }));
                    pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                }
                if (cFieldName.ToUpper() == "MFGRPART")
                {
                    tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = cLabelData; }));
                    pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                }
                if (cbSmartScan.Checked == true)
                {
                    if (cSearchEnable == 0)
                    {
                        if (tfdnpartnumber.Visible)
                        {
                            if (tfdnpartnumber.Text.Length > 0 && tfrecmfgrpart.Text.Length > 0)
                            {
                                SearchDNPart();
                            }
                        }
                        else
                        {
                            if (tfrecmfgrpart.Text.Length > 0)
                            {
                                tfdnpartnumber.Text = tfpartno.Text;
                                SearchDNPart();
                            }
                        }
                    }
                }
                i += 1;
            }
            handleBeep();
        }
        void GrabLabelData(String cLabelData)
        {
            String cFieldName, cPrefix, cSeperator;
            int cIndex;
            Char cSplitter;
            int i = 0;
            if (cTemplateType.ToUpper() != "SINGLE")
            {
                MessageBox.Show("Only support 1D Barcode labels in this version");
                return;
            }
            if (cLabelData.Length == 0)
                return;
            while (i <= lVendorLabel.Count - 1)
            {
                cFieldName = lVendorLabel[i].cFieldName;
                cPrefix = lVendorLabel[i].cPrefix;
                cSeperator = lVendorLabel[i].cSeperator;
                //cPrefix = "<|>" + cPrefix;
                if (cPrefix.Length == 0 || cPrefix.Length > cLabelData.Length)
                {
                    i += 1; continue;
                }

                if (lVendorLabel[i].cIndex.Length > 0)
                    cIndex = Convert.ToInt32(lVendorLabel[i].cIndex);
                else
                    cIndex = 1;
                String[] cTemp;
                if (cPrefix.ToUpper() == cLabelData.Substring(0, cPrefix.Length).ToUpper())
                {
                    if (cFieldName.ToUpper() == "LOTNUMBER")
                    {
                        //tflotno.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                        tflotno.Invoke(new Action(delegate() { tflotno.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                        if (cSeperator.Length > 0)
                        {
                            if (cSeperator == "SPACE")
                                cSplitter = ' ';
                            else
                                cSplitter = cSeperator[0];
                            cTemp = tflotno.Text.Split(cSplitter);
                            if (cTemp.Length >= cIndex)
                                //tflotno.Text = cTemp[cIndex-1];
                                tflotno.Invoke(new Action(delegate() { tflotno.Text = cTemp[cIndex - 1]; }));
                        }
                        //tflotno.Text = tflotno.Text.Trim();
                        tflotno.Invoke(new Action(delegate() { tflotno.Text = tflotno.Text.Trim(); }));
                        pblotnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }
                    else if (cFieldName.ToUpper() == "MFGDATE")
                    {
                        tfmfgdate.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                        if (cSeperator.Length > 0)
                        {
                            if (cSeperator == "SPACE")
                                cSplitter = ' ';
                            else
                                cSplitter = cSeperator[0];
                            cTemp = tfmfgdate.Text.Split(cSplitter);
                            if (cTemp.Length >= cIndex)
                                tfmfgdate.Text = cTemp[cIndex - 1];
                        }
                        tfmfgdate.Text = tfmfgdate.Text.Trim();
                        pbmfgdate.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }
                    else if (cFieldName.ToUpper() == "EXPIREDATE")
                    {
                        tfexpiredate.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                        if (cSeperator.Length > 0)
                        {
                            if (cSeperator == "SPACE")
                                cSplitter = ' ';
                            else
                                cSplitter = cSeperator[0];
                            cTemp = tfexpiredate.Text.Split(cSplitter);
                            if (cTemp.Length >= cIndex)
                                tfexpiredate.Text = cTemp[cIndex - 1];
                        }
                        tfexpiredate.Text = tfexpiredate.Text.Trim();
                        pbexpiredate.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }
                    else if (cFieldName.ToUpper() == "RECQTY")
                    {
                        //tfrecqty.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                        tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                        if (cSeperator.Length > 0)
                        {
                            if (cSeperator == "SPACE")
                                cSplitter = ' ';
                            else
                                cSplitter = cSeperator[0];
                            cTemp = tfrecqty.Text.Split(cSplitter);
                            if (cTemp.Length >= cIndex)
                                //tfrecqty.Text = cTemp[cIndex-1];
                                tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = cTemp[cIndex - 1]; }));
                        }
                        //tfrecqty.Text = tfrecqty.Text.Trim();
                        //tfrecqty.Text = tfrecqty.Text.Replace(",", "");
                        tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = tfrecqty.Text.Trim(); }));
                        tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = tfrecqty.Text.Replace(",", ""); }));
                        pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }
                    else if (cFieldName.ToUpper() == "DATECODE")
                    {
                        //tfdatecode.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                        tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                        if (cSeperator.Length > 0)
                        {
                            if (cSeperator == "SPACE")
                                cSplitter = ' ';
                            else
                                cSplitter = cSeperator[0];
                            cTemp = tfdatecode.Text.Split(cSplitter);
                            if (cTemp.Length >= cIndex)
                                //tfdatecode.Text = cTemp[cIndex-1];
                                tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = cTemp[cIndex - 1]; }));
                        }
                        //tfdatecode.Text = tfdatecode.Text.Trim();
                        tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = tfdatecode.Text.Trim(); }));
                        pbdatecode.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }
                    else if (cFieldName.ToUpper() == "DNPARTNUMBER")
                    {
                        tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                        if (cSeperator.Length > 0)
                        {
                            if (cSeperator == "SPACE")
                                cSplitter = ' ';
                            else
                                cSplitter = cSeperator[0];
                            cTemp = tfdnpartnumber.Text.Split(cSplitter);
                            if (cTemp.Length >= cIndex)
                                tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = cTemp[cIndex - 1]; }));
                        }
                        tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = tfdnpartnumber.Text.Trim(); }));
                        pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                        if (cbSmartScan.Checked == true)
                        {
                            if (tfdnpartnumber.Text.Length > 0 && tfrecmfgrpart.Text.Length > 0 && cSearchEnable == 0)
                            {
                                SearchDNPart();
                            }
                        }
                    }

                    else if (cFieldName.ToUpper() == "MFGRPART")
                    {
                        //tfrecmfgrpart.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length);
                        tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = cLabelData.Substring(cPrefix.Length, cLabelData.Length - cPrefix.Length); }));
                        if (cSeperator.Length > 0)
                        {
                            if (cSeperator == "SPACE")
                                cSplitter = ' ';
                            else
                                cSplitter = cSeperator[0];
                            cTemp = tfrecmfgrpart.Text.Split(cSplitter);
                            if (cTemp.Length >= cIndex)
                                //tfrecmfgrpart.Text = cTemp[cIndex - 1];
                                tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = cTemp[cIndex - 1]; }));
                        }
                        //tfrecmfgrpart.Text = tfrecmfgrpart.Text.Trim();
                        if (cbtrimmfgpart.Checked)
                            tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = tfrecmfgrpart.Text.Replace(" ", ""); tfrecmfgrpart.Text = tfrecmfgrpart.Text.Trim(); }));
                        else
                            tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = tfrecmfgrpart.Text.Trim(); }));
                        pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                        if (cbSmartScan.Checked == true)
                        {
                            if (cSearchEnable == 0)
                            {
                                if (tfdnpartnumber.Visible)
                                {
                                    if (tfdnpartnumber.Text.Length > 0 && tfrecmfgrpart.Text.Length > 0)
                                    {
                                        SearchDNPart();
                                    }
                                }
                                else
                                {
                                    if (tfrecmfgrpart.Text.Length > 0)
                                    {
                                        tfdnpartnumber.Text = tfpartno.Text;
                                        SearchDNPart();
                                    }
                                }
                            }
                        }
                    }
                }
                i += 1;
            }
            cLabelData = "";
            cLastLabel = "";
            handleBeep();
        }
        string removeCharTostr(string tmpstring, char ca)
        {
            string[] spstr = tmpstring.Split(ca);
            string restr = "";
            foreach (var item in spstr)
            {
                restr += item.Trim();
            }
            return restr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scanString"></param>
        /// <param name="dgv">dgv1Pending</param>
        /// <param name="strcellnamePart">"PartNumber"</param>
        /// <param name="strcellnameMFGP">"MFGPartNo"</param> 
        void SearchDNPart2(string scanString, DataGridView dgv, string strcellnamePart, string strcellnameMFGP)
        {
            var tmpmsg = "";
            char chara = ' ';
            string strSplit = " ";
            int cSearchFound = 0;

            ///PartNumber
            if (cSearchFound == 0)
            {
                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnamePart].Value.ToString().Replace(strSplit, "").ToUpper().Equals(scanString.Replace(strSplit, ""))
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;


                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;


                    tfdnpartnumber.Invoke(new Action(delegate()
                    {
                        tfdnpartnumber.Text = scanString;
                        pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }));


                    tmpmsg = "find in Pending list with PartNumber:[" + scanString + "]";

                    cSearchFound = 1;
                    break;
                }
            }
            if (cSearchFound == 0)
            {
                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnamePart].Value.ToString().Equals(scanString)
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;

                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;


                    tfdnpartnumber.Invoke(new Action(delegate()
                    {
                        tfdnpartnumber.Text = scanString;

                        pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }));
                    tmpmsg = "find in Pending list with PartNumber:[" + scanString + "]";
                    cSearchFound = 1;
                    break;
                }
            }
            /////////////mfgpartno
            if (cSearchFound == 0)
            {
                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnameMFGP].Value.ToString().Replace(strSplit, "").ToUpper().Equals(scanString.Replace(strSplit, ""))
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    tfrecmfgrpart.Invoke(new Action(delegate()
                    {
                        tfrecmfgrpart.Text = scanString;
                        pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }));
                    tmpmsg = "find in Pending list with MFGPartNo:[" + scanString + "]";
                    cSearchFound = 1;
                    break;
                }
            }
            if (cSearchFound == 0)
            {
                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnameMFGP].Value.ToString().Equals(scanString)
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    tfrecmfgrpart.Invoke(new Action(delegate()
                    {
                        tfrecmfgrpart.Text = scanString;
                        pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                    }));
                    tmpmsg = "find in Pending list with MFGPartNo:[" + scanString + "]";
                    cSearchFound = 1;
                    break;
                }
            }
            ///from dw_develop QPL_mstr
            ///

            //80 PartNumber
            if (cSearchFound == 0)
            {
                var txtMfgpart = scanString;
                var txtmfgpart80 = txtMfgpart.Substring(0, Convert.ToInt16(txtMfgpart.Length * 0.8));

                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnamePart].Value.ToString().ToUpper().StartsWith(txtmfgpart80)
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    tfdnpartnumber.Invoke(new Action(delegate()
                    {
                        tfdnpartnumber.Text = scanString;
                        pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick80.png");
                    }));
                    tmpmsg = "find in Pending list with 80% PartNumber:[" + scanString + "]";
                    cSearchFound = 1;
                    break;
                }
            }
            if (cSearchFound == 0)
            {
                var txtMfgpart = scanString;
                var txtmfgpart60 = txtMfgpart.Substring(0, Convert.ToInt16(txtMfgpart.Length * 0.6));

                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnamePart].Value.ToString().ToUpper().StartsWith(txtmfgpart60)
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    tfdnpartnumber.Invoke(new Action(delegate()
                    {
                        tfdnpartnumber.Text = scanString;
                        pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\tick60.png");
                    }));
                    tmpmsg = "find in Pending list with 60% PartNumber:[" + scanString + "]";
                    cSearchFound = 1;
                    break;
                }
            }
            ////60


            ///80 
            if (cSearchFound == 0)
            {
                var txtMfgpart = scanString;
                var txtmfgpart80 = txtMfgpart.Substring(0, Convert.ToInt16(txtMfgpart.Length * 0.8));

                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnameMFGP].Value.ToString().ToUpper().StartsWith(txtmfgpart80)
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    tfrecmfgrpart.Invoke(new Action(delegate()
                    {
                        tfrecmfgrpart.Text = scanString;
                        pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\tick80.png");
                    }));
                    tmpmsg = "find in Pending list with 80% MFGPartNo:[" + scanString + "]";
                    cSearchFound = 1;
                    break;
                }
            }
            if (cSearchFound == 0)
            {
                var txtMfgpart = scanString;
                var txtmfgpart60 = txtMfgpart.Substring(0, Convert.ToInt16(txtMfgpart.Length * 0.6));

                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnameMFGP].Value.ToString().ToUpper().StartsWith(txtmfgpart60)
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    tfrecmfgrpart.Invoke(new Action(delegate()
                    {
                        tfrecmfgrpart.Text = scanString;
                        pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\tick60.png");
                    }));
                    tmpmsg = "find in Pending list with 60% MFGPartNo:[" + scanString + "]";
                    cSearchFound = 1;
                    break;
                }
            }
            ////
            ///find ok
            ///
            if (!string.IsNullOrEmpty(tfdnpartnumber.Text) && !string.IsNullOrEmpty(tfrecmfgrpart.Text))
            {
                var query1 = from DataGridViewRow row in dgv.Rows
                             where row.Cells[strcellnamePart].Value.ToString() == tfdnpartnumber.Text &&
                                   row.Cells[strcellnameMFGP].Value.ToString() == tfrecmfgrpart.Text
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    tmpmsg = "find in Pending list with PartNumber:[" + tfdnpartnumber.Text + "] and MFGPartNo:[" + tfrecmfgrpart.Text + "]";
                    cSearchFound = 1;
                    break;
                }
            }
            //find by dw_develop qpl_mstr
            if (cSearchFound == 0)
            {
                if (!string.IsNullOrEmpty(tfdnpartnumber.Text))
                {
                    using (var db = new WHOperation.EF.DW.DW_Develop())
                    {
                        var tmp_qpl_mstr = db.qpl_mstr.Where(p => (p.qpl_part.Equals(tfdnpartnumber.Text.Trim()) && p.qpl_mfgr_part.Equals(scanString))).ToList();
                        if (tmp_qpl_mstr.Count > 0)
                        {
                            tfrecmfgrpart.Invoke(new Action(delegate()
                            {
                                tfrecmfgrpart.Text = scanString;
                                pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                            }));
                            tmpmsg = "find in DW_develop database with PartNumber:[" + tfdnpartnumber.Text + "] and MFGPartNo:[" + scanString + "]";
                            cSearchFound = 1;
                            dgv.ClearSelection();

                        }
                    }
                }
            }


            if (cSearchFound == 0)
            {
                //tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = ""; }));
                //tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = ""; }));
                //tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = ""; }));
                //tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = ""; }));
                //tflotno.Invoke(new Action(delegate() { tflotno.Text = ""; }));
                //tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.Text = ""; }));
                //tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.Text = ""; }));

                //pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                //pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                //pbdatecode.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                //pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                //pblotnumber.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                //pbmfgdate.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                //pbexpiredate.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                tmpmsg = "Can not find in Pending list with Part:[" + scanString + "] or Mfgr:[" + scanString + "]";
                // MessageBox.Show();

            }
            this.Invoke(new Action(delegate()
            {
                tool_lbl_Msg.Text = tmpmsg;
            }));

        }
        void SearchDNPart()
        {
            var query = from DataGridViewRow row in dgv1Pending.Rows
                        where row.Cells["PartNumber"].Value.ToString() == tfdnpartnumber.Text &&
                        row.Cells["MFGPartNo"].Value.ToString() == tfrecmfgrpart.Text
                        select row;
            int cSearchFound = 0;
            cBufferData.cDNPartumber = tfdnpartnumber.Text;
            cBufferData.cMFGPart = tfrecmfgrpart.Text;
            cBufferData.cDateCode = tfdatecode.Text;
            cBufferData.cRecQty = tfrecqty.Text;
            cBufferData.cLotNumber = tflotno.Text;
            cBufferData.cMfgDate = tfmfgdate.Text;
            cBufferData.cExpiredate = tfexpiredate.Text;

            cBufferData.cPMFGPart = pbrecmfgpart.Image;
            cBufferData.cPDateCode = pbdatecode.Image;
            cBufferData.cPRecQty = pbrecqty.Image;
            cBufferData.cPLotNumber = pblotnumber.Image;
            cBufferData.cPMfgDate = pbmfgdate.Image;
            cBufferData.cPExpiredate = pbexpiredate.Image;
            cBufferData.cPDNPartNumber = pbdnpartnumber.Image;
            foreach (DataGridViewRow onlineOrder in query)
            {
                onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                dgv1Pending.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                cSearchFound = 1;
                break;
            }
            if (cSearchFound == 0 && tfdnpartnumber.Visible == true)
            {
                var query1 = from DataGridViewRow row in dgv1Pending.Rows
                             where row.Cells["PartNumber"].Value.ToString().ToUpper() == tfdnpartnumber.Text.ToUpper()
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv1Pending.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    cSearchFound = 1;
                    break;
                }
            }
            if (cSearchFound == 0 && tfdnpartnumber.Visible == false)
            {
                var query1 = from DataGridViewRow row in dgv1Pending.Rows
                             where row.Cells["MFGPartNo"].Value.ToString().ToUpper() == tfrecmfgrpart.Text.ToUpper()
                             select row;
                foreach (DataGridViewRow onlineOrder in query1)
                {
                    onlineOrder.Selected = true; //onlineOrder.Cells[0].Selected = true;
                    dgv1Pending.FirstDisplayedScrollingRowIndex = onlineOrder.Index;
                    cSearchFound = 1;
                    break;
                }
            }
            tfdnpartnumber.Text = cBufferData.cDNPartumber;
            tfrecmfgrpart.Text = cBufferData.cMFGPart;
            tfdatecode.Text = cBufferData.cDateCode;
            tfrecqty.Text = cBufferData.cRecQty;
            tflotno.Text = cBufferData.cLotNumber;
            tfmfgdate.Text = cBufferData.cMfgDate;
            tfexpiredate.Text = cBufferData.cExpiredate;

            pbrecmfgpart.Image = cBufferData.cPMFGPart;
            pbdatecode.Image = cBufferData.cPDateCode;
            pbrecqty.Image = cBufferData.cPRecQty;
            pblotnumber.Image = cBufferData.cPLotNumber;
            pbmfgdate.Image = cBufferData.cPMfgDate;
            pbexpiredate.Image = cBufferData.cPExpiredate;
            pbdnpartnumber.Image = cBufferData.cPDNPartNumber;
            if (cSearchFound == 0)
            {
                tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = ""; }));
                tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = ""; }));
                tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = ""; }));
                tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = "0"; }));
                tflotno.Invoke(new Action(delegate() { tflotno.Text = ""; }));
                tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.Text = ""; }));
                tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.Text = ""; }));

                pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                pbdatecode.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                pblotnumber.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                pbmfgdate.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                pbexpiredate.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                MessageBox.Show("Can not find Part:[" + tfdnpartnumber.Text + "]/Mfgr:[" + tfrecmfgrpart.Text + "] PartNumber");
            }
            else
            {
                cSearchEnable = 1;
            }

        }
        void handleBeep()
        {
            int cDone;
            cDone = 0;
            if (tfdnpartnumber.Visible)
                if (tfdnpartnumber.Text.Length == 0)
                    cDone += 1;
            if (pbrecmfgpart.Visible)
                if (tfrecmfgrpart.Text.Length == 0)
                    cDone += 1;

            if (pbdatecode.Visible)
                if (tfdatecode.Text.Length == 0)
                    cDone += 1;

            if (pbmfgdate.Visible)
                if (tfmfgdate.Text.Length == 0)
                    cDone += 1;

            if (pbexpiredate.Visible)
                if (tfexpiredate.Text.Length == 0)
                    cDone += 1;

            if (pbrecqty.Visible)
                if (tfrecqty.Text.Length == 0)
                    cDone += 1;

            if (pblotnumber.Visible)
                if (tflotno.Text.Length == 0)
                    cDone += 1;

            String myComm;
            if (cDone == 0)
            {
                if (bStart.Enabled == false)
                {
                    myComm = "P%2650";
                    CodeUtil.NativeMethods.Code_TerminalSendCommand(deviceHandle, myComm, myComm.Length);
                    myComm = "#%01";
                    CodeUtil.NativeMethods.Code_TerminalSendCommand(deviceHandle, myComm, myComm.Length);
                    myComm = "P%260";
                    CodeUtil.NativeMethods.Code_TerminalSendCommand(deviceHandle, myComm, myComm.Length);
                    //captureImage(); //need bug fix
                }
                cLastPrint = DateTime.Now;
                handleAutoPrint();
                cSearchEnable = 0;
            }
        }
        void handleAutoPrint()
        {
            int cCompVal;
            if (cbAutoPrint.Checked == true)
            {
                cCompVal = completeTrans();
                resetForm(0);
            }
        }

        void captureImage()
        { //need bug fix
            byte[] bytes = new byte[1024];
            IntPtr cImage;
            Int32 cImageSize;
            cImageSize = 1024;
            cImage = new IntPtr();

            CodeUtil.OnProgressCallback OnProgresscallback = new CodeUtil.OnProgressCallback(UploadProgress);
            Int32 success = CodeUtil.NativeMethods.Code_UploadImage(deviceHandle, cImage, ref cImageSize, 0, 0, OnProgresscallback);
            if (0 == success)
            {
                Int32 err = CodeUtil.NativeMethods.Code_GetLastError(deviceHandle); //returning error 1002
                return;
            };
            Marshal.PtrToStructure(cImage, bytes);
            Image myImage = getImage(bytes); //internal lib
            //Marshal.Copy(cImage, bytes, 0, bytes.Length);                
            //myImage = Image.FromHbitmap(cImage);

            //pb1.Image = myImage;
            //myImage.Save("c:\\tmp\\myreader.bmp");

        }
        static private Int32 UploadProgress(IntPtr handle, int progress)
        {
            Console.WriteLine("{0}", progress);
            return 0;
        }
        void resetForm(int cFlag)
        {

            tflotno.Text = "";
            tfrecqty.Text = "";
            tfmfgdate.Text = "";
            tfexpiredate.Text = "";
            tfdatecode.Text = "";
            tfrecmfgrpart.Text = "";
            tfdnpartnumber.Text = "";

            tfrecmfgrpart.BackColor = Color.White;
            tfrecqty.BackColor = Color.White;
            tfcumqty.BackColor = Color.White;
            tfmfgpart.BackColor = Color.White;
            tfdatecode.BackColor = Color.White;
            tfexpiredate.BackColor = Color.White;
            tfmfgdate.BackColor = Color.White;
            tflotno.BackColor = Color.White;

            pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbdatecode.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbexpiredate.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbmfgdate.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pblotnumber.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");

            if (cFlag == 1)
            {
                //tfmfgpart.Text = "";
                //tfvendor.Text = "";
                //tfpartno.Text = "";
                //tfrirno.Text = "";
                //tfcumqty.Text = "";
                //tfdnqty.Text = "";
            }
            setFields();
        }
        void setPIMLData()
        {
            String cSelDNNo, cSelPONo, cSelPOLine, cSelDNDate, cSelVendor;
            //SqlDataReader myReader;
            String[] cRec = new String[14];
            DataGridViewRow cR = new DataGridViewRow();
            Double cCumQty;
            int i;
            cCumQty = 0;

            cSelDNNo = "";
            cSelDNDate = "";
            cSelPOLine = "";
            cSelVendor = "";
            cSelPONo = "";
            tfdnqty.Text = "";
            tfsite.Text = "";

            if (_usePrintPI)
            {
                if (dgv5PIPending.SelectedRows.Count <= 0)
                {
                    tfhdndate.Invoke(new Action(delegate() { tfhdndate.Text = cSelDNDate; }));
                    tfvendor.Invoke(new Action(delegate() { tfvendor.Text = cSelVendor; }));
                    tfpartno.Invoke(new Action(delegate() { tfpartno.Text = ""; }));
                    tfrirno.Invoke(new Action(delegate() { tfrirno.Text = ""; }));
                    tfmfgpart.Invoke(new Action(delegate() { tfmfgpart.Text = ""; }));
                }
                else
                {
                    cR = dgv5PIPending.SelectedRows[0];
                    //cSelDNNo = cR.Cells["PI_NO"].Value.ToString();
                    cSelDNDate = cR.Cells["pi_cre_time"].Value.ToString();
                    //cSelPOLine = cR.Cells["POLine"].Value.ToString();
                    cSelVendor = cR.Cells["pi_mfgr"].Value.ToString();
                    cSelPONo = cR.Cells["PI_PO"].Value.ToString();

                    tfdnqty.Text = Convert.ToInt32(cR.Cells["PI_QTY"].Value).ToString();
                    tfsite.Text = cR.Cells["PI_SITE"].Value.ToString();
                    //tfhdnno.Text = cSelDNNo;
                    //tfhvendor.Text = cSelVendor;
                    tfhdndate.Invoke(new Action(delegate() { tfhdndate.Text = cSelDNDate; }));
                    tfvendor.Invoke(new Action(delegate() { tfvendor.Text = cSelVendor; }));
                    tfpartno.Invoke(new Action(delegate() { tfpartno.Text = cR.Cells["PI_PART"].Value.ToString(); }));
                    tfrirno.Invoke(new Action(delegate() { tfrirno.Text = cR.Cells["PI_LOT"].Value.ToString(); }));
                    tfmfgpart.Invoke(new Action(delegate() { tfmfgpart.Text = cR.Cells["pi_mfgr_part"].Value.ToString(); }));
                }
            }
            else
            {

                //cR = dataGridView1.CurrentRow;
                if (dgv1Pending.SelectedRows.Count <= 0)
                {

                    tfhdndate.Invoke(new Action(delegate() { tfhdndate.Text = cSelDNDate; }));
                    tfvendor.Invoke(new Action(delegate() { tfvendor.Text = cSelVendor; }));
                    tfpartno.Invoke(new Action(delegate() { tfpartno.Text = ""; }));
                    tfrirno.Invoke(new Action(delegate() { tfrirno.Text = ""; }));
                    tfmfgpart.Invoke(new Action(delegate() { tfmfgpart.Text = ""; }));
                }
                else
                {
                    cR = dgv1Pending.SelectedRows[0];
                    cSelDNNo = cR.Cells["DNNo"].Value.ToString();
                    cSelDNDate = cR.Cells["DNDate"].Value.ToString();
                    cSelPOLine = cR.Cells["POLine"].Value.ToString();
                    cSelVendor = cR.Cells["Vendor"].Value.ToString();
                    cSelPONo = cR.Cells["PONumber"].Value.ToString();
                    tfdnqty.Text = cR.Cells["DNQty"].Value.ToString();
                    tfsite.Text = cR.Cells["DNSite"].Value.ToString();
                    //tfhdnno.Text = cSelDNNo;
                    //tfhvendor.Text = cSelVendor;
                    tfhdndate.Invoke(new Action(delegate() { tfhdndate.Text = cSelDNDate; }));
                    tfvendor.Invoke(new Action(delegate() { tfvendor.Text = cSelVendor; }));
                    tfpartno.Invoke(new Action(delegate() { tfpartno.Text = cR.Cells["PartNumber"].Value.ToString(); }));
                    tfrirno.Invoke(new Action(delegate() { tfrirno.Text = cR.Cells["RIRNo"].Value.ToString(); }));
                    tfmfgpart.Invoke(new Action(delegate() { tfmfgpart.Text = cR.Cells["MFGPartNo"].Value.ToString(); }));
                }
            }
        }
        void getTemplate()
        {
            String cQuery, cSelVendor;
            SqlDataReader myReader;
            String cRec;
            DataGridViewRow cR = new DataGridViewRow();
            String cXMLTemplate;
            byte[] cImageData;
            lXML = new List<String>();
            lVendorLabelImage = new List<byte[]>();
            //cR = dataGridView1.CurrentRow;
            //add

            //
            if (dgv1Pending.SelectedRows.Count <= 0)
            {
                cSelVendor = "";
            }
            else
            {
                cR = dgv1Pending.SelectedRows[0];
                cSelVendor = cR.Cells[2].Value.ToString();
            }
            cSelVendor = tfdnno.Text;
            cQuery = "select TemplateID,xmlVendorData,templateImage from PIMLVendorTemplate where VendorID='" + cSelVendor + "' Order By isDefault desc,TemplateID ";
            //cQuery = "select TemplateID,xmlVendorData from PIMLVendorTemplate where VendorID='" + cSelVendor + "' Order By isDefault desc,TemplateID ";
            dgv3VendorTemplate.Rows.Clear();
            try
            {
                using (SqlConnection conn = new SqlConnection(_cConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(cQuery, conn);
                    myReader = cmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        cRec = myReader.GetValue(0).ToString();
                        cXMLTemplate = myReader.GetValue(1).ToString();
                        if (cXMLTemplate.Length > 0)
                        {
                            dgv3VendorTemplate.Rows.Add(cRec);
                            lXML.Add(cXMLTemplate);
                            cImageData = new byte[0];
                            try
                            {
                                cImageData = (byte[])myReader[2];
                            }
                            catch (Exception) { cImageData = new byte[0]; }
                            lVendorLabelImage.Add(cImageData);
                        }
                    }
                    myReader.Close();
                    setDataFieldLabel();
                }
            }
            catch (Exception) { initDG3(); }
            finally { }
        }
        public bool initPiPrintModel(PI_Print piPrintModel, DataGridView dgv)
        {
            var cr = dgv.SelectedRows[0];
            if (cr == null)
            {
                tool_lbl_Msg.Text = "please a row in PI pending";
                return false;
            }
            long tmpmaxLine = 1;
            if (_dbWHOperation.PI_Print.Count() > 0)
            {
                tmpmaxLine = _dbWHOperation.PI_Print.Max(p => p.PI_Line);
            }
            piPrintModel.PI_Line = tmpmaxLine + 1;
            piPrintModel.PI_NO = txt1PIID.Text.Trim();
            piPrintModel.PI_PO = cr.Cells["PI_PO"].Value.ToString().Trim();
            piPrintModel.PI_PART = cr.Cells["PI_PART"].Value.ToString().Trim();
            piPrintModel.pi_mfgr_part = cr.Cells["pi_mfgr_part"].Value.ToString().Trim();
            piPrintModel.PI_LOT = cr.Cells["PI_LOT"].Value.ToString().Trim();
            piPrintModel.PI_PO = cr.Cells["PI_PO"].Value.ToString().Trim();
            piPrintModel.pi_mfgr = cr.Cells["pi_mfgr"].Value.ToString().Trim();
            piPrintModel.PI_QTY = Convert.ToDecimal(cr.Cells["PI_QTY"].Value);
            if (string.IsNullOrEmpty(tfrecqty.Text))
            {
                tfrecqty.Text = "0";
            }
            var ttlPrint = Convert.ToInt32(tfnoofcartons.Text.Trim()) *
                                      Convert.ToInt32(tfnooflabels.Text.Trim()) *
                                      Convert.ToInt32(tfrecqty.Text.Trim());// +Convert.ToDecimal(dgv.CurrentRow.Cells["PI_Print_QTY"].Value);
            piPrintModel.PI_Print_QTY = ttlPrint;


            if (piPrintModel.PI_QTY < (ttlPrint + Convert.ToDecimal(cr.Cells["PI_Print_QTY"].Value)))
            {
                tool_lbl_Msg.Text = "PI Qty:" + piPrintModel.PI_QTY + " < Print Qty:" + ttlPrint + "=" + tfnoofcartons.Text + " * " + tfnoofcartons.Text + " * " + tfrecqty.Text + " + " + dgv.CurrentRow.Cells["PI_Print_QTY"].Value;
                return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            /*if (cbSmartScan.Checked == true)
            {
                if (tfdnpartnumber.Text.Length > 0 && tfrecmfgrpart.Text.Length > 0 && cSearchEnable == 0)
                {

                    SearchDNPart();
                }
            }*/
            if (_usePrintPI)
            {
                int cVal;
                lStatus.Invoke(new Action(delegate() { lStatus.Text = "Processing..."; }));
                cVal = valData(dgv5PIPending);
                if (cVal == 0)
                {
                    PI_Print tmpPrint = new PI_Print();

                    if (initPiPrintModel(tmpPrint, dgv5PIPending))
                    {

                        _dbWHOperation.PI_Print.Add(tmpPrint);
                        var saveflag = _dbWHOperation.SaveChanges();
                        if (saveflag > 0)
                        {
                            updDataPrintForPI(dgv5PIPending, piid);

                            dgv5PIPending.SelectedRows[0].Cells["PI_Print_QTY"].Value = Convert.ToDecimal(dgv5PIPending.SelectedRows[0].Cells["PI_Print_QTY"].Value) + tmpPrint.PI_Print_QTY;
                            checkPrintNumger(dgv5PIPending, _dtPIRemote, dgv6PICompele);
                            // btn2PIID_Click(sender, e);
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Data Validation failed");
                }

            }
            else
            {
                int cVal;
                lStatus.Invoke(new Action(delegate() { lStatus.Text = "Processing..."; }));
                cVal = valData();

                if (cVal == 0)
                {
                    updData();
                    if (tfrecmfgrpart.Text.Length > 0)
                    {
                        if (tfrecmfgrpart.Text.ToUpper() != tfmfgpart.Text.ToUpper())
                        {
                            //MessageBox.Show("PO QPL Part & Received QPL Part mismatch");
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Data Validation failed");
                }

            }
            lStatus.Invoke(new Action(delegate() { lStatus.Text = ""; }));
            tfscanarea.Focus();
            //enableScan();

        }
        String getPIMSData()
        {
            String cRet;
            DataRow cR;
            DataSet pimlData;
            StreamReader cRetReader;
            cRet = "";
            pimlData = new DataSet("pimlData");
            cRetReader = callMFGService(cbsystem.Text, "wsas003", cbsystem.Text);
            try
            {
                pimlData.ReadXml(cRetReader);
                if (pimlData.Tables.IndexOf("row") >= 0)
                {
                    if (pimlData.Tables["row"].Rows.Count > 0)
                    {
                        cR = pimlData.Tables["Row"].Rows[0];
                        cRet = cR.ItemArray[0].ToString();
                    }
                }
            }
            catch (Exception serEx) { MessageBox.Show("PIMS Label Service Error:\n" + serEx.Message.ToString(), "System Message"); }
            return cRet;
        }

        void updDataPrintForPI(DataGridView dgv, string piid)
        {
            String cQuery, cPIMSNumber, cCartonQty;
            DataGridViewRow cR = new DataGridViewRow();
            DataGridViewRow cR1 = new DataGridViewRow();
            List<String> lPIMSData = new List<String>();
            int cCartonLoop, cNoOfCartons;
            int i;
            Double cPIMSQty;
            //cR = dataGridView1.CurrentRow;

            if (dgv.SelectedRows.Count <= 0)
                return;

            cR = dgv.SelectedRows[0];
            String[] cRec = new String[cR.Cells.Count];
            for (i = 0; i <= cR.Cells.Count - 1; i += 1)
            {
                cRec[i] = cR.Cells[i].Value.ToString();
            }

            cPIMSNumber = "tmpPIMS";
            cPIMSNumber = getPIMSData();
            try
            {
                int cPrintLoop;
                int cNoOfLabels;
                cPrintLoop = 1;
                cNoOfLabels = Convert.ToInt32(tfnooflabels.Text);
                while (cPrintLoop <= cNoOfLabels)
                {
                    if (cbprintcartonlabel.Checked == true && cPrintLoop == 1)
                    {
                        cCartonLoop = 1;
                        cNoOfCartons = Convert.ToInt32(tfnoofcartons.Text);
                        while (cCartonLoop <= cNoOfCartons)
                        {
                            cPIMSNumber = getPIMSData();
                            lPIMSData = updateMFGPro(cPIMSNumber, dgv5PIPending, "PI_LOT");
                            if (lPIMSData == null)
                            {
                                break;
                            }
                            if (lPIMSData[0].ToString() == "-2") { }
                            else
                            {
                                cCartonQty = "0";
                                cPIMSQty = (Convert.ToDouble(tfrecqty.Text) * Convert.ToDouble(tfnooflabels.Text)) / cNoOfCartons;
                                try
                                {
                                    if (Convert.ToDouble(cCartonQty) > 0)
                                        lPIMSData[7] = cCartonQty;
                                    else
                                        //lPIMSData[7] = (Convert.ToDouble(tfrecqty.Text) * Convert.ToDouble(tfnooflabels.Text)).ToString();
                                        lPIMSData[7] = cPIMSQty.ToString();
                                }
                                catch (Exception ex) { lPIMSData[7] = "0"; }
                                printPIML(lPIMSData, 1);
                            }
                            cCartonLoop += 1;
                        }
                    }
                    cPIMSNumber = getPIMSData();
                    lPIMSData = updateMFGPro(cPIMSNumber, dgv5PIPending, "PI_LOT");
                    if (lPIMSData == null)
                    {
                        break;
                    }
                    if (lPIMSData.Count > 0)
                    {
                        if (lPIMSData[0].ToString() == "-2") { MessageBox.Show("Must Input Date Code or Lot No"); }
                        else
                        {

                            if (lPIMSData[5].ToUpper().Contains("MRB"))
                            {
                                cQuery = "insert into PIMSMRBException (DNNo,DNDate,RIRNo,SupplierID,MfgrID,MG,PIMS,PartNumber,ReqMfgrPart,RecMfgrPart,CustPart,RecQty) " +
                                    "values('" + piid + "','" + cRec[9] + "','" + tfrirno.Text + "','" + cRec[2] + "','" + lPIMSData[6] + "','" + cRec[10] + "','" + cPIMSNumber + "','" + cRec[3] + "','" + tfmfgpart.Text + "','" + tfrecmfgrpart.Text + "','" + cRec[14] + "','" + tfrecqty.Text + "')";
                                SQLUpdate(cQuery);
                            }
                            printPIML(lPIMSData, 0);
                        }
                    }
                    cPrintLoop += 1;
                }

                setPIMLData();

            }
            catch (Exception ex) { }
            finally
            {
                Thread.Sleep(3000); enableScan();
            }
        }
        void updData()
        {
            String cQuery, cPIMSNumber, cTotQty, cDNNo, cCartonQty;
            DataGridViewRow cR = new DataGridViewRow();
            DataGridViewRow cR1 = new DataGridViewRow();
            List<String> lPIMSData = new List<String>();
            int cCartonLoop, cNoOfCartons;
            int i;
            Double cPIMSQty;
            //cR = dataGridView1.CurrentRow;

            if (dgv1Pending.SelectedRows.Count <= 0)
                return;

            //disableScan();

            cR = dgv1Pending.SelectedRows[0];
            String[] cRec = new String[cR.Cells.Count];
            for (i = 0; i <= cR.Cells.Count - 1; i += 1)
            {
                cRec[i] = cR.Cells[i].Value.ToString();
            }

            cPIMSNumber = "tmpPIMS";
            cPIMSNumber = getPIMSData();
            cTotQty = (Convert.ToDouble(tfrecqty.Text) * Convert.ToDouble(tfnooflabels.Text)).ToString();

            try
            {
                int cPrintLoop;
                int cNoOfLabels;
                cPrintLoop = 1;
                cNoOfLabels = Convert.ToInt32(tfnooflabels.Text);
                while (cPrintLoop <= cNoOfLabels)
                {
                    if (cbprintcartonlabel.Checked == true && cPrintLoop == 1)
                    {
                        cCartonLoop = 1;
                        cNoOfCartons = Convert.ToInt32(tfnoofcartons.Text);
                        while (cCartonLoop <= cNoOfCartons)
                        {
                            cPIMSNumber = getPIMSData();
                            lPIMSData = updateMFGPro(cPIMSNumber);
                            if (lPIMSData == null)
                            {
                                break;
                            }
                            if (lPIMSData[0].ToString() == "-2") { }
                            else
                            {
                                cCartonQty = "0";
                                cPIMSQty = (Convert.ToDouble(tfrecqty.Text) * Convert.ToDouble(tfnooflabels.Text)) / cNoOfCartons;
                                try
                                {
                                    if (Convert.ToDouble(cCartonQty) > 0)
                                        lPIMSData[7] = cCartonQty;
                                    else
                                        //lPIMSData[7] = (Convert.ToDouble(tfrecqty.Text) * Convert.ToDouble(tfnooflabels.Text)).ToString();
                                        lPIMSData[7] = cPIMSQty.ToString();
                                }
                                catch (Exception ex) { lPIMSData[7] = "0"; }
                                printPIML(lPIMSData, 1);
                            }
                            cCartonLoop += 1;
                        }
                    }
                    cPIMSNumber = getPIMSData();
                    lPIMSData = updateMFGPro(cPIMSNumber);
                    if (lPIMSData == null)
                    {
                        break;
                    }
                    if (lPIMSData.Count > 0)
                    {
                        if (lPIMSData[0].ToString() == "-2") { MessageBox.Show("Must Input Date Code or Lot No"); }
                        else
                        {
                            Double cPrintQty;
                            if (string.IsNullOrEmpty(tfrecqty.Text))
                            {
                                tfrecqty.Text = "0";
                            }
                            cDNNo = dgv0DNNumber.CurrentRow.Cells[0].Value.ToString();
                            cPrintQty = getCompleteQty(cDNNo, cRec[6], cRec[1], tfrirno.Text, cRec[9], cRec[2]);
                            if (cPrintQty == 0 && cPrintLoop == 1)
                            {
                                cQuery = "Insert into PIMLDetail (SystemID,TransID,TransLine,DNNo,DNDate,VendorID,PONo,POLine,PartNumber,DNQty,LineQty,LotNo,RIRNo,MFGPartNumber,ExpDate,DateCode, " +
                                        " t_site,t_urg,t_loc,t_msd,t_cust_part,t_shelf_life,t_wt,t_wt_ind,t_conn,mfgDate,PIMSNumber,NoOfLabels) " +
                                        " values('" + cbsystem.Text + "','001','001','" + cDNNo + "','" + cRec[9] + "','" + cRec[2] + "','" + cRec[6] + "','" + cRec[1] + "','" + cRec[3] + "','" + cRec[8] + "','" + tfrecqty.Text + "','" + tflotno.Text + "','" + tfrirno.Text + "','" + tfmfgpart.Text + "','" + tfexpiredate.Text + "','" + tfdatecode.Text + "', " +
                                        " '" + cRec[10] + "','" + cRec[11] + "','" + cRec[12] + "','" + cRec[13] + "','" + cRec[14] + "','" + cRec[15] + "','" + cRec[16] + "','" + cRec[17] + "','" + cRec[18] + "','" + tfmfgdate.Text + "','" + cPIMSNumber + ";','1') ";
                            }
                            else
                            {
                                //cPrintQty = Convert.ToDouble(tfrecqty.Text) * Convert.ToDouble(tfnooflabels.Text);
                                cQuery = "update PIMLDetail set LineQty=LineQty + '" + tfrecqty.Text + "',NoOfLabels=NoofLabels+1,PIMSNumber=PIMSNumber+'" + cPIMSNumber + ";' where DNNo='" + cDNNo + "' and PONo='" + cRec[6] + "' and PoLine='" + cRec[1] + "' and RIRNo='" + tfrirno.Text + "' and DNDate='" + cRec[9] + "' and VendorID='" + cRec[2] + "'";
                            }
                            SQLUpdate(cQuery);
                            if (lPIMSData[5].ToUpper().Contains("MRB"))
                            {
                                cQuery = "insert into PIMSMRBException (DNNo,DNDate,RIRNo,SupplierID,MfgrID,MG,PIMS,PartNumber,ReqMfgrPart,RecMfgrPart,CustPart,RecQty) " +
                                    "values('" + cDNNo + "','" + cRec[9] + "','" + tfrirno.Text + "','" + cRec[2] + "','" + lPIMSData[6] + "','" + cRec[10] + "','" + cPIMSNumber + "','" + cRec[3] + "','" + tfmfgpart.Text + "','" + tfrecmfgrpart.Text + "','" + cRec[14] + "','" + tfrecqty.Text + "')";
                                SQLUpdate(cQuery);
                            }
                            setCompleteDN();
                            printPIML(lPIMSData, 0);
                        }
                    }
                    cPrintLoop += 1;
                }

                setPIMLData();

            }
            catch (Exception ex) { }
            finally
            {
                Thread.Sleep(3000); enableScan();
            }
        }
        void SQLUpdate(String cQuery)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_cConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(cQuery, conn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { MessageBox.Show("SQL Error:" + ex.Message.ToString()); }
            finally { }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cPIMSNumber"></param>
        /// <param name="dgv"></param>
        /// <param name="strcellRiRNO">PI_LOT,RIRNo</param>
        /// <returns></returns>
        List<String> updateMFGPro(String cPIMSNumber, DataGridView dgv, string strcellRiRNO)
        {
            int i;
            String cServiceID, cLocalSysID;
            StringBuilder cPara = new StringBuilder();
            StreamReader cRetReader;
            DataSet pimsData;
            DataGridViewRow cR = new DataGridViewRow();
            DataRow cDR;
            List<String> lPIMSData = new List<String>();
            //cR = dataGridView1.CurrentRow;
            cR = dgv.SelectedRows[0];
            cServiceID = "wsas002";
            pimsData = new DataSet("pimlData");
            cLocalSysID = cbsystem.Text;
            /*cPara.Append(cR.Cells["DNSite"].Value.ToString()+","+cR.Cells["PartNumber"].Value.ToString()+
                "," + cR.Cells["RIRNo"].Value.ToString() + ",'',''," + tfrecqty.Text + "," + tfmfgpart.Text + "," + cUserID + "," + tflotno.Text + ",''," +
                tfexpiredate.Text+",'',"+cR.Cells["t_shelf_life"].Value.ToString()+",'YES','NO','R'");*/
            cPara.Append(cPIMSNumber + "," + cR.Cells[strcellRiRNO].Value.ToString() + "," + tfdatecode.Text + "," + tfmfgdate.Text + "," + tfexpiredate.Text + "," + tfrecqty.Text + "," + cUserID + "," + tflotno.Text + "," + tfrecmfgrpart.Text);
            cRetReader = callMFGService(cLocalSysID, cServiceID, cPara.ToString());
            try
            {
                pimsData.ReadXml(cRetReader);
                if (pimsData.Tables["Row"].Rows.Count > 0)
                {
                    cDR = pimsData.Tables["Row"].Rows[0];
                    i = 0;
                    while (i <= cDR.ItemArray.Length - 1)
                    {
                        lPIMSData.Add(cDR.ItemArray[i].ToString());
                        i += 1;
                    }
                }
                else
                {

                }
            }
            catch (Exception serEx) { MessageBox.Show("PIMS Label Data MFGPro Service Error:\n" + serEx.Message.ToString(), "System Message"); return null; }

            return lPIMSData;
        }
        List<String> updateMFGPro(String cPIMSNumber)
        {
            int i;
            String cServiceID, cLocalSysID;
            StringBuilder cPara = new StringBuilder();
            StreamReader cRetReader;
            DataSet pimsData;
            DataGridViewRow cR = new DataGridViewRow();
            DataRow cDR;
            List<String> lPIMSData = new List<String>();
            //cR = dataGridView1.CurrentRow;
            cR = dgv1Pending.SelectedRows[0];
            cServiceID = "wsas002";
            pimsData = new DataSet("pimlData");
            cLocalSysID = cbsystem.Text;
            /*cPara.Append(cR.Cells["DNSite"].Value.ToString()+","+cR.Cells["PartNumber"].Value.ToString()+
                "," + cR.Cells["RIRNo"].Value.ToString() + ",'',''," + tfrecqty.Text + "," + tfmfgpart.Text + "," + cUserID + "," + tflotno.Text + ",''," +
                tfexpiredate.Text+",'',"+cR.Cells["t_shelf_life"].Value.ToString()+",'YES','NO','R'");*/
            cPara.Append(cPIMSNumber + "," + cR.Cells["RIRNo"].Value.ToString() + "," + tfdatecode.Text + "," + tfmfgdate.Text + "," + tfexpiredate.Text + "," + tfrecqty.Text + "," + cUserID + "," + tflotno.Text + "," + tfrecmfgrpart.Text);
            cRetReader = callMFGService(cLocalSysID, cServiceID, cPara.ToString());
            try
            {
                pimsData.ReadXml(cRetReader);
                if (pimsData.Tables["Row"].Rows.Count > 0)
                {
                    cDR = pimsData.Tables["Row"].Rows[0];
                    i = 0;
                    while (i <= cDR.ItemArray.Length - 1)
                    {
                        lPIMSData.Add(cDR.ItemArray[i].ToString());
                        i += 1;
                    }
                }
                else
                {

                }
            }
            catch (Exception serEx) { MessageBox.Show("PIMS Label Data MFGPro Service Error:\n" + serEx.Message.ToString(), "System Message"); return null; }

            return lPIMSData;
        }
        String getLastRec()
        {
            String cQuery, cRet;
            SqlDataReader myReader;
            cQuery = "select top 1 TransID from PIMLDetail Order by TransID desc";
            cRet = "00000000";
            try
            {
                using (SqlConnection conn = new SqlConnection(_cConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(cQuery, conn);
                    myReader = cmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        cRet = myReader.GetValue(0).ToString();
                    }
                    myReader.Close();
                    if (cRet.Length > 0)
                        cRet = (Convert.ToInt32(cRet) + 1).ToString("00000000");
                    else
                        cRet = "00000001";
                }
            }
            catch (Exception) { }
            finally { }
            return cRet;
        }
        String getLastLine(String cTransID)
        {
            String cQuery, cRet;
            SqlDataReader myReader;
            cQuery = "select top 1 TransLine from PIMLDetail where TransID='" + cTransID + "' Order by TransLine desc";
            cRet = "000";
            try
            {
                using (SqlConnection conn = new SqlConnection(_cConnStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(cQuery, conn);
                    myReader = cmd.ExecuteReader();
                    while (myReader.Read())
                    {
                        cRet = myReader.GetValue(0).ToString();
                    }
                    myReader.Close();
                    if (cRet.Length > 0)
                        cRet = (Convert.ToInt32(cRet) + 1).ToString("000");
                    else
                        cRet = "001";
                }
            }
            catch (Exception) { }
            finally { }
            return cRet;
        }
        void removePrefix()
        {
            String cPX, cFN, cFieldVal;
            var xx = from x1 in lVendorLabel select new { x1.cFieldName, x1.cPrefix };
            foreach (var tt in xx)
            {
                cFN = tt.cFieldName;
                cPX = tt.cPrefix.ToUpper();

                if (cFN.ToUpper() == "LOTNUMBER")
                {
                    cFieldVal = tflotno.Text.ToUpper();
                    if (cFieldVal.Length > cPX.Length && cPX.Length > 0)
                    {
                        if (cFieldVal.Substring(0, cPX.Length) == cPX)
                            tflotno.Invoke(new Action(delegate() { tflotno.Text = cFieldVal.Replace(cPX, ""); }));
                    }
                }
                if (cFN.ToUpper() == "MFGRPART")
                {
                    cFieldVal = tfrecmfgrpart.Text.ToUpper();
                    if (cFieldVal.Length > cPX.Length && cPX.Length > 0)
                    {
                        if (cFieldVal.Substring(0, cPX.Length) == cPX)
                            tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = cFieldVal.Replace(cPX, ""); }));
                    }
                }
                if (cFN.ToUpper() == "MFGDATE")
                {
                    cFieldVal = tfmfgdate.Text.ToUpper();
                    if (cFieldVal.Length > cPX.Length && cPX.Length > 0)
                    {
                        if (cFieldVal.Substring(0, cPX.Length) == cPX)
                            tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.Text = cFieldVal.Replace(cPX, ""); }));
                    }
                }
                if (cFN.ToUpper() == "EXPIREDATE")
                {
                    cFieldVal = tfexpiredate.Text.ToUpper();
                    if (cFieldVal.Length > cPX.Length && cPX.Length > 0)
                    {
                        if (cFieldVal.Substring(0, cPX.Length) == cPX)
                            tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.Text = cFieldVal.Replace(cPX, ""); }));
                    }
                }
                if (cFN.ToUpper() == "DATECODE")
                {
                    cFieldVal = tfdatecode.Text.ToUpper();
                    if (cFieldVal.Length > cPX.Length && cPX.Length > 0)
                    {
                        if (cFieldVal.Substring(0, cPX.Length) == cPX)
                            tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = cFieldVal.Replace(cPX, ""); }));
                    }
                }
                if (cFN.ToUpper() == "DNPARTNUMBER")
                {
                    cFieldVal = tfdnpartnumber.Text.ToUpper();
                    if (cFieldVal.Length > cPX.Length && cPX.Length > 0)
                    {
                        if (cFieldVal.Substring(0, cPX.Length) == cPX)
                            tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = cFieldVal.Replace(cPX, ""); }));
                    }
                }
                if (cFN.ToUpper() == "RECQTY")
                {
                    cFieldVal = tfrecqty.Text.ToUpper();
                    if (cFieldVal.Length > cPX.Length && cPX.Length > 0)
                    {
                        if (cFieldVal.Substring(0, cPX.Length) == cPX)
                            tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = cFieldVal.Replace(cPX, ""); }));
                    }
                }
            }
        }
        void setMandField()
        {
            String cErrMsg, cSpecialPartVal, cExpireDatePartVal, cQuery;
            DateTime cOldMfgDate;
            tflotno.BackColor = Color.White;
            cOldMfgDate = DateTime.Now.AddDays(-730);
            MiscDLL1.dbClass mydbClass = new MiscDLL1.dbClass();
            cErrMsg = ""; cExpireDatePartVal = ""; cSpecialPartVal = "";
            cQuery = "select tmp_Part from tmp_tab where tmp_system='wse869a4' and tmp_part='" + tfpartno.Text + "' and tmp_site='" + tfsite.Text + "'";
            cSpecialPartVal = mydbClass.getSingleFieldData(_cConnStr, cQuery);
            cQuery = "select tmp_Part from tmp_tab where tmp_system='expidate' and tmp_part='" + tfpartno.Text + "' ";
            cExpireDatePartVal = mydbClass.getSingleFieldData(_cConnStr, cQuery);
            lMDateCode.Visible = false; lMExpireDate.Visible = false; lMLotNumber.Visible = false;
            lMRecMfgPart.Visible = true;
            //lMDateCode.ForeColor = Color.Black; lMLotNumber.ForeColor = Color.Black;
            //"\nRequire Rec Mfgr Part Number";
            if (cSpecialPartVal.Length > 0)
            {
                lMDateCode.Visible = true; lMLotNumber.Visible = true; //lMDateCode.ForeColor = Color.DarkBlue; lMLotNumber.ForeColor = Color.DarkBlue;
            }
            if (cExpireDatePartVal.Length > 0) lMExpireDate.Visible = true;

            if (tfsite.Text.ToUpper() == "MG0337") { lMLotNumber.Visible = true; lMDateCode.Visible = true; }

            if (tfsite.Text.ToUpper() == "MG7024" || tfsite.Text.ToUpper() == "MG5007" || tfsite.Text.ToUpper() == "MG7030" || tfsite.Text.ToUpper() == "MG7029" || tfsite.Text.ToUpper() == "MG5008" || tfsite.Text.ToUpper() == "MG0248" || tfsite.Text.ToUpper() == "MG7028" ||
                tfsite.Text.ToUpper() == "MG7022" || tfsite.Text.ToUpper() == "MG0208" || tfsite.Text.ToUpper() == "MG0220" || tfsite.Text.ToUpper() == "MG0217")
            {
                if (tfpartno.Text.Substring(0, 1) == "1" || tfpartno.Text.Substring(0, 1) == "2" || tfpartno.Text.Substring(0, 1) == "3" || tfpartno.Text.Substring(0, 1) == "5" || tfpartno.Text.Substring(0, 2) == "70")
                {
                    //"nDateCode or Lot Number required for 1x,2x,3x,5x,70x parts";
                    lMDateCode.Visible = true; lMLotNumber.Visible = true; //lMDateCode.ForeColor = Color.DarkBlue; lMLotNumber.ForeColor = Color.DarkBlue;
                }
            }
            //lMDateCode.ForeColor = Color.DarkBlue; lMLotNumber.ForeColor = Color.DarkBlue;
            return;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="strcellDNQty"></param>
        /// <param name="strcellPrintedQty"></param>
        /// <returns></returns>
        int valData(DataGridView dgv)
        {
            String cErrMsg;
            int cRet;
            DataGridViewRow cR;
            DateTime value;
            Double cTemp;
            DateTime cMfgDate;
            DateTime cOldMfgDate;
            tflotno.BackColor = Color.White;
            cOldMfgDate = DateTime.Now.AddDays(-730);

            cRet = 0;
            cErrMsg = "";
            if (dgv.Rows.Count <= 0)
            {
                return 0;
            }
            cR = dgv.SelectedRows[0];
            /*toolTip1.SetToolTip(tfcumqty, "");
            toolTip1.SetToolTip(tfrecqty, "");
            toolTip1.SetToolTip(tfexpiredate, "");
            toolTip1.SetToolTip(tfmfgdate, "");
            toolTip1.SetToolTip(tflotno, "");
            toolTip1.SetToolTip(tfdatecode, "");
            */
            // tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = tfrecqty.Text.Trim(); }));

            tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.White; }));
            tfrecqty.Invoke(new Action(delegate() { tfrecqty.BackColor = Color.White; }));
            tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.BackColor = Color.White; }));
            tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.White; }));
            tflotno.Invoke(new Action(delegate() { tflotno.BackColor = Color.White; }));
            tfdatecode.Invoke(new Action(delegate() { tfdatecode.BackColor = Color.White; }));
            tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.BackColor = Color.White; }));

            if (tfrecmfgrpart.Text.Length == 0)
            {
                cRet += 1;
                //tfrecmfgrpart.BackColor = Color.Red;
                tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.BackColor = Color.Red; }));
                cErrMsg += "\nRequire Rec Mfgr Part Number";
            }
            else
            {
                tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.BackColor = Color.White; }));
            }

            if (!Double.TryParse(tfrecqty.Text, out cTemp))
            {
                cRet += 1;
                //tfrecqty.BackColor = Color.Red;
                tfrecqty.Invoke(new Action(delegate() { tfrecqty.BackColor = Color.Red; }));
                cErrMsg += "\nRequire Number in received Qty";
            }
            else
            {
                //tfrecqty.BackColor = Color.White;
                tfrecqty.Invoke(new Action(delegate() { tfrecqty.BackColor = Color.White; }));
            }
            if (!string.IsNullOrEmpty(tfdnqty.Text))
            {
                var tmpint = Convert.ToInt32(tfnoofcartons.Text) * Convert.ToInt32(tfnooflabels.Text) * Convert.ToInt32(tfrecqty.Text);
                if (tmpint > Convert.ToInt32(tfdnqty.Text))
                {

                    tool_lbl_Msg.Text = "超出 dn qty 数量:" + tfnoofcartons.Text + " * " + tfnooflabels.Text + " * " + tfrecqty.Text + " = " + tmpint + " > " + tfdnqty.Text;
                    cErrMsg += "\n" + tool_lbl_Msg.Text;
                    cRet += 1;
                }
            }
            /*if (!Double.TryParse(tfcumqty.Text, out cTemp)) {
                cRet += 1;
                //tfcumqty.BackColor = Color.Red;
                tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.Red; }));
                cErrMsg += "\nInvalid Cumulative Qty";
            } else {
                tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.White; }));
            }*/
            if (tfsite.Text.ToUpper() == "MG0337")
            {
                if (tflotno.Text.Length == 0 && tfdatecode.Text.Length == 0)
                {
                    cRet += 1;
                    //tflotno.BackColor = Color.Red;
                    tflotno.Invoke(new Action(delegate() { tflotno.BackColor = Color.Red; }));
                    cErrMsg += "\nLot Number/DateCode can not be empty for MG0337";
                }
            }
            if (tfsite.Text.ToUpper() == "MG7024" || tfsite.Text.ToUpper() == "MG5007" || tfsite.Text.ToUpper() == "MG7030" || tfsite.Text.ToUpper() == "MG7029" || tfsite.Text.ToUpper() == "MG5008" || tfsite.Text.ToUpper() == "MG0248" || tfsite.Text.ToUpper() == "MG7028" ||
                tfsite.Text.ToUpper() == "MG7022" || tfsite.Text.ToUpper() == "MG0208" || tfsite.Text.ToUpper() == "MG0220" || tfsite.Text.ToUpper() == "MG0217")
            {
                if (tfpartno.Text.Substring(0, 1) == "1" || tfpartno.Text.Substring(0, 1) == "2" || tfpartno.Text.Substring(0, 1) == "3" || tfpartno.Text.Substring(0, 1) == "5" || tfpartno.Text.Substring(0, 2) == "70")
                {
                    if (tfdatecode.Text.Length == 0 && tflotno.Text.Length == 0)
                    {
                        cRet += 1;
                        tfdatecode.Invoke(new Action(delegate() { tfdatecode.BackColor = Color.Red; }));
                        cErrMsg += "\nDateCode or Lot Number required for 1x,2x,3x,5x,70x parts";
                    }
                }
            }
            if (tfmfgdate.Text.Length > 0)
            {
                if (!DateTime.TryParse(tfmfgdate.Text, out value))
                {
                    cRet += 1;
                    tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.Red; }));
                    cErrMsg += "\nInvalid Date in Mfgr Date";
                }
                else
                {
                    //tfmfgdate.Text = Convert.ToDateTime(tfmfgdate.Text).ToString("MM/dd/yy");
                    tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.Text = Convert.ToDateTime(tfmfgdate.Text).ToString("MM/dd/yy"); }));
                    cMfgDate = Convert.ToDateTime(tfmfgdate.Text);
                    if (cMfgDate.CompareTo(DateTime.Now) > 0)
                    {
                        cRet += 1;
                        //tfmfgdate.BackColor = Color.Red;
                        tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.Red; }));
                        cErrMsg += "\nMfgr Date should not be later than today";
                    }
                    else if (cMfgDate.CompareTo(cOldMfgDate) < 0)
                    {
                        cRet += 1;
                        tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.White; }));
                        cErrMsg += "\nMfgr Date is too old";
                    }
                    else
                    {
                        tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.White; }));
                    }
                }
            }
            if (tfexpiredate.Text.Length > 0)
            {
                if (!DateTime.TryParse(tfexpiredate.Text, out value))
                {
                    cRet += 1;
                    tfexpiredate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.Red; }));
                    cErrMsg += "\nInvalid expire date";

                }
                else
                {
                    tfexpiredate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.White; }));
                    tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.Text = Convert.ToDateTime(tfexpiredate.Text).ToString("MM/dd/yy"); }));
                }
            }

            /* to be removed, suggested by business users
            try
            {
                if (Double.TryParse(tfrecqty.Text, out cTemp) && Double.TryParse(tfcumqty.Text, out cTemp) && Double.TryParse(tfdnqty.Text, out cTemp)) {
                    if ((Convert.ToDouble(tfcumqty.Text) + Convert.ToDouble(tfrecqty.Text)) > Convert.ToDouble(tfdnqty.Text))
                    {
                        cRet += 1;
                        tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.Red; }));
                        cErrMsg += "\nPIMS Already printed for all DN QTY/\nInvalid Receive Qty";
                    } else {
                        tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.White; }));
                    }
                }
            }
            catch (Exception) { } */
            if (cErrMsg.Length > 0)
            {
                MessageBox.Show(cErrMsg, "Error Message");
            }
            return cRet;
        }
        int valData()
        {
            String cErrMsg, cSpecialPartVal, cExpireDatePartVal, cQuery;
            int cRet;
            DataGridViewRow cR;
            DateTime value;
            Double cTemp;
            DateTime cMfgDate;
            DateTime cOldMfgDate;
            tflotno.BackColor = Color.White;
            cOldMfgDate = DateTime.Now.AddDays(-730);
            MiscDLL1.dbClass mydbClass = new MiscDLL1.dbClass();
            cRet = 0;
            cErrMsg = ""; cExpireDatePartVal = ""; cSpecialPartVal = "";
            if (dgv1Pending.Rows.Count <= 0)
            {
                return 0;
            }
            cR = dgv1Pending.SelectedRows[0];
            /*toolTip1.SetToolTip(tfcumqty, "");
            toolTip1.SetToolTip(tfrecqty, "");
            toolTip1.SetToolTip(tfexpiredate, "");
            toolTip1.SetToolTip(tfmfgdate, "");
            toolTip1.SetToolTip(tflotno, "");
            toolTip1.SetToolTip(tfdatecode, "");
            */
            // tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = tfrecqty.Text.Trim(); }));
            removePrefix();
            tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.White; }));
            tfrecqty.Invoke(new Action(delegate() { tfrecqty.BackColor = Color.White; }));
            tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.BackColor = Color.White; }));
            tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.White; }));
            tflotno.Invoke(new Action(delegate() { tflotno.BackColor = Color.White; }));
            tfdatecode.Invoke(new Action(delegate() { tfdatecode.BackColor = Color.White; }));
            tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.BackColor = Color.White; }));
            String cPrintQty, cDNQty;
            //cPrintQty = dataGridView1.CurrentRow.Cells["PrintedQty"].Value.ToString();
            //cDNQty = dataGridView1.CurrentRow.Cells["DNQty"].Value.ToString();
            cPrintQty = dgv1Pending.SelectedRows[0].Cells["PrintedQty"].Value.ToString();
            cDNQty = dgv1Pending.SelectedRows[0].Cells["DNQty"].Value.ToString();
            Double dLinePrintQty;
            //cPrintQty = getCompleteQty(cR["t_dn"].ToString(), cR["t_po"].ToString(), cR["t_id"].ToString(), cR["t_rir"].ToString(), cR["t_deli_date"].ToString(), cR["t_supp"].ToString()); 
            dLinePrintQty = getCompleteQty(cR.Cells["DNNo"].Value.ToString(), cR.Cells["PONumber"].Value.ToString(), cR.Cells["POLine"].Value.ToString(), tfrirno.Text, tfhdndate.Text, tfvendor.Text);
            cPrintQty = dLinePrintQty.ToString();
            if (cPrintQty.Length == 0) cPrintQty = "0";
            if (cDNQty.Length == 0) cDNQty = "0";
            if (tfrecqty.Text.Length == 0) tfrecqty.Text = "0";
            if (Convert.ToDouble(cPrintQty) + (Convert.ToDouble(tfrecqty.Text) * Convert.ToDouble(tfnooflabels.Text)) > Convert.ToDouble(cDNQty))
            {
                cRet += 1;
                cErrMsg += "\nCannot Print PIMS more than DNQty";
            }
            cQuery = "select tmp_Part from tmp_tab where tmp_system='wse869a4' and tmp_part='" + tfpartno.Text + "' and tmp_site='" + tfsite.Text + "'";
            cSpecialPartVal = mydbClass.getSingleFieldData(_cConnStr, cQuery);
            cQuery = "select tmp_Part from tmp_tab where tmp_system='expidate' and tmp_part='" + tfpartno.Text + "' ";
            cExpireDatePartVal = mydbClass.getSingleFieldData(_cConnStr, cQuery);

            if (tfrecmfgrpart.Text.Length == 0)
            {
                cRet += 1;
                //tfrecmfgrpart.BackColor = Color.Red;
                tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.BackColor = Color.Red; }));
                cErrMsg += "\nRequire Rec Mfgr Part Number";
            }
            else
            {
                tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.BackColor = Color.White; }));
            }
            if (cSpecialPartVal.Length > 0)
            {
                if (tfdatecode.Text.Length == 0 && tflotno.Text.Length == 0)
                {
                    cRet += 1;
                    tfdatecode.Invoke(new Action(delegate() { tfdatecode.BackColor = Color.Red; }));
                    cErrMsg += "\nDate Code or Lot Number Required for this Parts";
                }
            }
            if (cExpireDatePartVal.Length > 0)
            {
                if (tfexpiredate.Text.Length == 0)
                {
                    cRet += 1;
                    //tfdatecode.BackColor = Color.Red;
                    tfdatecode.Invoke(new Action(delegate() { tfdatecode.BackColor = Color.Red; }));
                    cErrMsg += "\nExpire Date Required for this Part";
                }
            }
            if (!Double.TryParse(tfrecqty.Text, out cTemp))
            {
                cRet += 1;
                //tfrecqty.BackColor = Color.Red;
                tfrecqty.Invoke(new Action(delegate() { tfrecqty.BackColor = Color.Red; }));
                cErrMsg += "\nRequire Number in received Qty";
            }
            else
            {
                //tfrecqty.BackColor = Color.White;
                tfrecqty.Invoke(new Action(delegate() { tfrecqty.BackColor = Color.White; }));
            }
            /*if (!Double.TryParse(tfcumqty.Text, out cTemp)) {
                cRet += 1;
                //tfcumqty.BackColor = Color.Red;
                tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.Red; }));
                cErrMsg += "\nInvalid Cumulative Qty";
            } else {
                tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.White; }));
            }*/
            if (tfsite.Text.ToUpper() == "MG0337")
            {
                if (tflotno.Text.Length == 0 && tfdatecode.Text.Length == 0)
                {
                    cRet += 1;
                    //tflotno.BackColor = Color.Red;
                    tflotno.Invoke(new Action(delegate() { tflotno.BackColor = Color.Red; }));
                    cErrMsg += "\nLot Number/DateCode can not be empty for MG0337";
                }
            }
            if (tfsite.Text.ToUpper() == "MG7024" || tfsite.Text.ToUpper() == "MG5007" || tfsite.Text.ToUpper() == "MG7030" || tfsite.Text.ToUpper() == "MG7029" || tfsite.Text.ToUpper() == "MG5008" || tfsite.Text.ToUpper() == "MG0248" || tfsite.Text.ToUpper() == "MG7028" ||
                tfsite.Text.ToUpper() == "MG7022" || tfsite.Text.ToUpper() == "MG0208" || tfsite.Text.ToUpper() == "MG0220" || tfsite.Text.ToUpper() == "MG0217")
            {
                if (tfpartno.Text.Substring(0, 1) == "1" || tfpartno.Text.Substring(0, 1) == "2" || tfpartno.Text.Substring(0, 1) == "3" || tfpartno.Text.Substring(0, 1) == "5" || tfpartno.Text.Substring(0, 2) == "70")
                {
                    if (tfdatecode.Text.Length == 0 && tflotno.Text.Length == 0)
                    {
                        cRet += 1;
                        tfdatecode.Invoke(new Action(delegate() { tfdatecode.BackColor = Color.Red; }));
                        cErrMsg += "\nDateCode or Lot Number required for 1x,2x,3x,5x,70x parts";
                    }
                }
            }
            if (tfmfgdate.Text.Length > 0)
            {
                if (!DateTime.TryParse(tfmfgdate.Text, out value))
                {
                    cRet += 1;
                    tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.Red; }));
                    cErrMsg += "\nInvalid Date in Mfgr Date";
                }
                else
                {
                    //tfmfgdate.Text = Convert.ToDateTime(tfmfgdate.Text).ToString("MM/dd/yy");
                    tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.Text = Convert.ToDateTime(tfmfgdate.Text).ToString("MM/dd/yy"); }));
                    cMfgDate = Convert.ToDateTime(tfmfgdate.Text);
                    if (cMfgDate.CompareTo(DateTime.Now) > 0)
                    {
                        cRet += 1;
                        //tfmfgdate.BackColor = Color.Red;
                        tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.Red; }));
                        cErrMsg += "\nMfgr Date should not be later than today";
                    }
                    else if (cMfgDate.CompareTo(cOldMfgDate) < 0)
                    {
                        cRet += 1;
                        tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.White; }));
                        cErrMsg += "\nMfgr Date is too old";
                    }
                    else
                    {
                        tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.White; }));
                    }
                }
            }
            if (tfexpiredate.Text.Length > 0)
            {
                if (!DateTime.TryParse(tfexpiredate.Text, out value))
                {
                    cRet += 1;
                    tfexpiredate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.Red; }));
                    cErrMsg += "\nInvalid expire date";

                }
                else
                {
                    tfexpiredate.Invoke(new Action(delegate() { tfmfgdate.BackColor = Color.White; }));
                    tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.Text = Convert.ToDateTime(tfexpiredate.Text).ToString("MM/dd/yy"); }));
                }
            }

            /* to be removed, suggested by business users
            try
            {
                if (Double.TryParse(tfrecqty.Text, out cTemp) && Double.TryParse(tfcumqty.Text, out cTemp) && Double.TryParse(tfdnqty.Text, out cTemp)) {
                    if ((Convert.ToDouble(tfcumqty.Text) + Convert.ToDouble(tfrecqty.Text)) > Convert.ToDouble(tfdnqty.Text))
                    {
                        cRet += 1;
                        tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.Red; }));
                        cErrMsg += "\nPIMS Already printed for all DN QTY/\nInvalid Receive Qty";
                    } else {
                        tfcumqty.Invoke(new Action(delegate() { tfcumqty.BackColor = Color.White; }));
                    }
                }
            }
            catch (Exception) { } */
            if (cErrMsg.Length > 0)
            {
                MessageBox.Show(cErrMsg, "Error Message");
            }
            return cRet;
        }
        void setDataFieldLabel()
        {
            int cRow;
            String cXMLData;
            byte[] cImage;
            List<String> cFieldList = new List<String>();
            if (dgv3VendorTemplate.Rows.Count <= 0)
            {
                return;
            }
            cRow = dgv3VendorTemplate.CurrentRow.Index;
            if (cRow < lXML.Count)
                cXMLData = lXML[cRow];
            else
                return;
            lVendorLabel = new List<vendorLabelDefinition>();
            setFields(lVendorLabel = parseTempXMLTest(cXMLData));
            try
            {
                cImage = lVendorLabelImage[dgv3VendorTemplate.CurrentRow.Index];
                if (cImage.Length == 0)
                    pb1.ImageLocation = Application.StartupPath + @"\images\notavailable.png";
                else
                    pb1.Image = getImage(cImage);
            }
            catch (Exception ex) { }
            /*if (cTemplateType.ToUpper() == "GENERAL") {
                cbAutoPrint.Checked = false;
                cbAutoPrint.Enabled = false;
            } else {
                cbAutoPrint.Checked = true;
                cbAutoPrint.Enabled = true;
            }*/
        }
        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            initDG3();
        }

        private void initDG3()
        {
            cCaptureData cDG3 = new cCaptureData();
            cDG3.cDNPartumber = tfdnpartnumber.Text;
            cDG3.cMFGPart = tfrecmfgrpart.Text;
            cDG3.cDateCode = tfdatecode.Text;
            cDG3.cRecQty = tfrecqty.Text;
            cDG3.cLotNumber = tflotno.Text;
            cDG3.cMfgDate = tfmfgdate.Text;
            cDG3.cExpiredate = tfexpiredate.Text;

            resetForm(0);
            setDataFieldLabel();

            tfdnpartnumber.Text = cDG3.cDNPartumber;
            tfrecmfgrpart.Text = cDG3.cMFGPart;
            tfdatecode.Text = cDG3.cDateCode;
            tfrecqty.Text = cDG3.cRecQty;
            tflotno.Text = cDG3.cLotNumber;
            tfmfgdate.Text = cDG3.cMfgDate;
            tfexpiredate.Text = cDG3.cExpiredate;

            if (dgv3VendorTemplate.Rows.Count <= 0)
            {
                chk5NoSplit.Checked = true;
            }
            else
            {
                chk5NoSplit.Checked = false;
            }
        }
        StreamReader callMFGService(String cSystemID, String progID, String cParam)
        {
            String cRet;
            cRet = "";
            try
            {
                cRet = MFGProService.GetTable(cSystemID, progID, cParam);
            }
            catch (Exception) { }
            byte[] byteArray = Encoding.ASCII.GetBytes(cRet);
            MemoryStream stream2 = new MemoryStream(byteArray);
            StreamReader cSReader = new StreamReader(stream2);
            return cSReader;
        }
        void getMFGDNData()
        {
            DataRow cR;
            StreamReader cRetReader;
            int cFound;
            List<String> lDNNumber = new List<string>();
            dsDNDetail = new DataSet("dsDNDetail");
            cRetReader = callMFGService(cbsystem.Text, "wsas001", tfdnno.Text + "," + tfdndate.Text + "," + tftodndate.Text);
            //cRetReader = callMFGService(cbsystem.Text, "wsas001", tfdnno.Text + "," + tfdndate.Text);
            try
            {
                dsDNDetail.ReadXml(cRetReader);
            }
            catch (Exception serEx) { MessageBox.Show("MFGPro Service Error:\n" + serEx.Message.ToString(), "System Message"); return; }

            int i = 0;
            //t1 dataGridView1.Rows.Clear();

            int cRowCount;
            if (dsDNDetail.Tables.Count >= 7)
            {
                dsDNDetail.Tables[6].Columns.Add("PrintedQty");
                dsDNDetail.Tables[6].Columns.Add("RowID");

                dgv0DNNumber.Rows.Clear();
                cRowCount = dsDNDetail.Tables[6].Rows.Count;
                while (i <= dsDNDetail.Tables[6].Rows.Count - 1)
                {
                    cR = dsDNDetail.Tables[6].Rows[i];
                    //t1 dataGridView1.Rows.Add(cR.ItemArray[0], cR.ItemArray[10], cR.ItemArray[7], cR.ItemArray[4], cR.ItemArray[3], cR.ItemArray[9], "",cR.ItemArray[2], cR.ItemArray[6], cR.ItemArray[1], cR.ItemArray[5], cR.ItemArray[11], cR.ItemArray[12], cR.ItemArray[13], cR.ItemArray[14], cR.ItemArray[15], cR.ItemArray[16], cR.ItemArray[17], cR.ItemArray[18],"0");
                    var query = from p in lDNNumber
                                where lDNNumber.Contains(cR.ItemArray[0])
                                select p;
                    cFound = 0;
                    foreach (String t in query)
                    {
                        cFound += 1;
                    }
                    if (cFound == 0)
                    {
                        lDNNumber.Add(cR.ItemArray[0].ToString());
                    }

                    i += 1;
                }
                var xx = from t in lDNNumber select t;
                foreach (String t1 in xx)
                    dgv0DNNumber.Rows.Add(t1);

            }
            else
            {
                //t1 dataGridView1.Rows.Clear();
                dgv1Pending.Rows.Clear();
                dgv3VendorTemplate.Rows.Clear();
                dgv0DNNumber.Rows.Clear();
                resetForm(1);
                MessageBox.Show("No Data Found");
            }
        }
        List<String> parseTempXML(String cXMLData)
        {
            DataRow cR;
            List<String> lRet = new List<String>();
            DataSet dsAuthors = new DataSet("Template");
            byte[] byteArray = Encoding.ASCII.GetBytes(cXMLData);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader xx1 = new StreamReader(stream);

            dsAuthors.ReadXml(xx1);
            int i = 0;
            while (i <= dsAuthors.Tables[0].Rows.Count - 1)
            {
                cR = dsAuthors.Tables[0].Rows[i];
                lRet.Add(cR.ItemArray[0].ToString());
                i += 1;
            }
            return lRet;
        }
        List<vendorLabelDefinition> parseTempXMLTest(String cXMLData)
        {
            DataRow cR;
            List<vendorLabelDefinition> lRet = new List<vendorLabelDefinition>();
            DataSet dsAuthors = new DataSet("Template");
            byte[] byteArray = Encoding.ASCII.GetBytes(cXMLData);
            MemoryStream stream = new MemoryStream(byteArray);
            StreamReader xx1 = new StreamReader(stream);

            dsAuthors.ReadXml(xx1);
            vendorLabelDefinition vendorLabel = new vendorLabelDefinition();

            int i = 0;
            while (i <= dsAuthors.Tables["Field"].Rows.Count - 1)
            {
                cR = dsAuthors.Tables["Field"].Rows[i];
                vendorLabel = new vendorLabelDefinition();
                vendorLabel.cFieldName = cR.ItemArray[0].ToString();
                vendorLabel.cPrefix = cR.ItemArray[1].ToString();
                vendorLabel.cSeperator = cR.ItemArray[2].ToString();
                vendorLabel.cIndex = cR.ItemArray[3].ToString();
                lRet.Add(vendorLabel);
                i += 1;
            }
            c2DSeperator = "";
            if (dsAuthors.Tables.IndexOf("Header") >= 0)
            {
                cTemplateType = dsAuthors.Tables["Header"].Rows[0].ItemArray[1].ToString();
                if (dsAuthors.Tables["Header"].Rows[0].ItemArray.Length > 2)
                    c2DSeperator = dsAuthors.Tables["Header"].Rows[0].ItemArray[2].ToString();

                if (c2DSeperator == "\\r\\n")
                {
                    c2DSeperator = "\r\n";
                }
                if (c2DSeperator == "\\t")
                {
                    c2DSeperator = "\t";
                }
            }
            else
            {
                cTemplateType = "Single";
                c2DSeperator = "";
            }
            return lRet;
        }
        void toPrinter(StringBuilder cStringToPrint, String cPIMS)
        {
            String cSelPort;
            cSelPort = "LPT1";
            lStatus.Invoke(new Action(delegate() { lStatus.Text = "Printing...."; }));
            cbport.Invoke(new Action(delegate() { cSelPort = cbport.SelectedItem.ToString(); }));
            StreamWriter outputfile = new StreamWriter("c://tmp//PIMS/spool//piml" + cPIMS + ".txt", false, Encoding.UTF8);
            try
            {
                PrinterHandle.LPTControl printHandle = new PrinterHandle.LPTControl(cSelPort);
                if (printHandle.Open())
                {
                    printHandle.Write(cStringToPrint.ToString());
                    printHandle.Close();
                }
                outputfile.Write(cStringToPrint.ToString());
            }
            catch (Exception prEx) { MessageBox.Show("Print Error :\n" + prEx.Message.ToString()); }
            finally { outputfile.Close(); }
            lStatus.Invoke(new Action(delegate() { lStatus.Text = ""; }));
        }

        void printPIML(List<String> lPIMSData, int cLabelType)
        {
            disableScan();
            StringBuilder cRet = new StringBuilder();
            PIMLPrint pimlPrint = new PIMLPrint();
            String cSelPrinter;
            int cNoLabel;
            //DataGridViewRow cR = new DataGridViewRow();
            ////cR = dataGridView1.CurrentRow;
            //cR = dgv1Pending.SelectedRows[0];
            cSelPrinter = "1";
            cNoLabel = Convert.ToInt32(tfnooflabels.Text);
            //cSelPrinter = (cbprintertype.SelectedIndex + 1).ToString();
            cbprintertype.Invoke(new Action(delegate() { cSelPrinter = (cbprintertype.SelectedIndex + 1).ToString(); }));
            try
            {
                /* cRet = pimlPrint.genPIML(
                        tfdndate.Text.Substring(tfdndate.Text.Length - 2, 2), 
                        "*IQC", tflotno.Text.ToUpper(), tfpartno.Text.ToUpper(), cR.Cells["DNSite"].Value.ToString(), 
                        tfrecqty.Text, tfdnqty.Text, "Ref", cR.Cells["t_loc"].Value.ToString(),
                        tfexpiredate.Text, "R", tfmfgpart.Text.ToUpper(), cR.Cells["t_cust_part"].Value.ToString(), 
                        cPIMSNumber, tfdatecode.Text,
                        cSelPrinter, "by", cR.Cells["t_wt_ind"].Value.ToString(), cR.Cells["t_wt"].Value.ToString(), 
                        cR.Cells["t_MSD"].Value.ToString(), cUserID, cR.Cells["t_msd"].Value.ToString(), "",cNoLabel
                ); */
                //6=type;3=Part;4=Site;8=Qty_Per;9=Qty_Tot;7=Ref;5=Loc;10=ExpiDate;11=ExpiType;12=MfgrPart;13;CustPart;1=PIMSNnbr;14=DateCode
                //15=by;16=wt;17=msd;

                cRet = pimlPrint.genPIML(
                            tfdndate.Text.Substring(tfdndate.Text.Length - 2, 2),
                            lPIMSData[5].ToString().ToUpper(), tflotno.Text.ToUpper(), lPIMSData[2].ToString().ToUpper(), lPIMSData[3].ToString().ToUpper(),
                            lPIMSData[7].ToString().ToUpper(), tfdnqty.Text, lPIMSData[6].ToString().ToUpper(), lPIMSData[4].ToString().ToUpper(),
                            lPIMSData[9].ToString().ToUpper(), lPIMSData[10].ToString().ToUpper(), lPIMSData[11].ToString().ToUpper(), lPIMSData[12].ToString().ToUpper(),
                            lPIMSData[0].ToString().ToUpper(), lPIMSData[13].ToString().ToUpper(),
                            cSelPrinter, lPIMSData[14].ToString().ToUpper(), lPIMSData[15].ToString().ToUpper(), lPIMSData[15].ToString().ToUpper(),
                            lPIMSData[16].ToString().ToUpper(), cUserID, lPIMSData[16].ToString().ToUpper(), "", 1, tfrirno.Text.ToUpper(), lPIMSData[17].ToString().ToUpper()
                 );
                toPrinter(cRet, lPIMSData[0].ToString());

                getQRcode = "";
                _strNoPrefixlit.Clear();
                _strNoPrefixlitTmp.Clear();

                if (cLabelType == 0)
                    setDSPrintedQty();
                // EnableScan();
            }
            catch (Exception labEr)
            {
                enableScan();
                getQRcode = "";
                _strNoPrefixlit.Clear();
                _strNoPrefixlitTmp.Clear();
                MessageBox.Show("Data Error:" + labEr.Message.ToString());
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (dsDNDetail.Tables.Count >= 7)
                setGV1();
        }
        void getlinq1()
        {
            int[] iArray = { 1, 2, 4, 7, 3, 41, 5, 6 };
            var x = from myArray in iArray where myArray > 2 orderby myArray select myArray;
            foreach (var x1 in x)
            {

            }
        }
        void setGV1()
        {
            int i = 0;
            String cDNNo;
            DataRow cR;
            DataGridViewRow cDGR;
            dgv1Pending.Rows.Clear();
            DataTable dt = new DataTable();
            dt = (DataTable)dsDNDetail.Tables[6];
            cDGR = dgv0DNNumber.CurrentRow;
            cDNNo = cDGR.Cells["DNNumber"].Value.ToString();
            while (i <= dsDNDetail.Tables[6].Rows.Count - 1)
            {
                cR = dsDNDetail.Tables[6].Rows[i];
                if (cbfiltertype.SelectedIndex == 0)
                {
                    if ((cR.ItemArray[3].ToString().ToUpper().StartsWith(textBox2.Text.ToUpper()) && cR.ItemArray[0].ToString() == cDNNo) || (textBox2.Text.Length == 0 && cR.ItemArray[0].ToString() == cDNNo))
                    {
                        dgv1Pending.Rows.Add(cR.ItemArray[0], cR.ItemArray[10], cR.ItemArray[7], cR.ItemArray[3], cR.ItemArray[9], cR.ItemArray[2], cR.ItemArray[4], "", cR.ItemArray[6], cR.ItemArray[1], cR.ItemArray[5], cR.ItemArray[11], cR.ItemArray[12], cR.ItemArray[13], cR.ItemArray[14], cR.ItemArray[15], cR.ItemArray[16], cR.ItemArray[17], cR.ItemArray[18], cR.ItemArray[20], i.ToString());
                    }
                }
                else
                {
                    if ((cR.ItemArray[9].ToString().ToUpper().StartsWith(textBox2.Text.ToUpper()) && cR.ItemArray[0].ToString() == cDNNo) || (textBox2.Text.Length == 0 && cR.ItemArray[0].ToString() == cDNNo))
                    {
                        dgv1Pending.Rows.Add(cR.ItemArray[0], cR.ItemArray[10], cR.ItemArray[7], cR.ItemArray[3], cR.ItemArray[9], cR.ItemArray[2], cR.ItemArray[4], "", cR.ItemArray[6], cR.ItemArray[1], cR.ItemArray[5], cR.ItemArray[11], cR.ItemArray[12], cR.ItemArray[13], cR.ItemArray[14], cR.ItemArray[15], cR.ItemArray[16], cR.ItemArray[17], cR.ItemArray[18], cR.ItemArray[20], i.ToString());
                    }
                }
                i += 1;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {

        }
        Image getImage(byte[] cByte)
        {
            MemoryStream ms = new MemoryStream(cByte);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            StartCodeReader();
        }
        /*private void tsStart_Click(object sender, EventArgs e)
        {
            tsStart.Text = "Running";
            tsStop.Enabled = true;
            readerThread = new Thread(new ThreadStart(startThread));
            readerThread.Start();
        }*/
        private void button2_Click(object sender, EventArgs e)
        {
            bStart.Text = "Starting";
            bStart.Enabled = false;

            readerThread = new Thread(new ThreadStart(startThread));
            readerThread.Start();

        } // End NewData()
        private void startThread()
        {
            CodeReaderhandle = StartCodeReader();

            MessageBox.Show("Time Expired/Device not available\nRestart Again", "Notice");
            if (CodeReaderhandle.ToString() != "0")
            {
                StopCodeReader(CodeReaderhandle);
            }
            bStart.Invoke(new Action(delegate() { bStart.Text = "Start"; }));
            bStart.Invoke(new Action(delegate() { bStart.Enabled = true; }));
            bStop.Invoke(new Action(delegate() { bStop.Enabled = false; }));

        }

        /*private void tsStop_Click(object sender, EventArgs e)
        {
            StopCodeReader(CodeReaderhandle);
            readerThread.Abort();
            tsStart.Text = "Start";
            tsStop.Enabled = false;
        }*/
        private void bStop_Click(object sender, EventArgs e)
        {
            StopCodeReader(CodeReaderhandle);
            readerThread.Abort();
            bStart.Text = "Start";
            bStart.Invoke(new Action(delegate() { bStart.Enabled = true; }));
            bStop.Invoke(new Action(delegate() { bStop.Enabled = false; }));
        }
        delegate void TextBoxDelegate(string message);
        public void UpdatingTextBox(string msg)
        {
            if (tfscanarea.InvokeRequired)
                tfscanarea.Invoke(new TextBoxDelegate(UpdatingTextBox), new object[] { msg });
            else
                this.tfscanarea.Text = msg;
        }

        //---
        IntPtr deviceHandle;
        Int32 success;
        public IntPtr StartCodeReader()
        {
            IntPtr hardwareDetector = CodeUtil.NativeMethods.Code_CreateHardwareDetector(null);
            uint maxSize = 5000;
            Int32 commandLength = 1024;
            StringBuilder hardwareXml = new StringBuilder((int)maxSize + 1);
            CodeUtil.NativeMethods.Code_SwitchKeyboardToHidNative();
            Thread.Sleep(5000);

            maxSize = CodeUtil.NativeMethods.Code_DetectHardwareXML(hardwareDetector, hardwareXml, maxSize, false);
            CodeUtil.NativeMethods.Code_DestroyHardwareDetector(hardwareDetector);

            List<string> devices = ParseHardwareList(hardwareXml.ToString());
            string deviceInfo = SelectHardwareDevice(devices, "Hid_Native", "");
            if (0 == deviceInfo.Length)
                return deviceHandle;

            deviceHandle = CodeUtil.NativeMethods.Code_CreateDevice(deviceInfo, deviceInfo.Length);


            StringBuilder buffer = new StringBuilder(1024);
            int info = 0;

            /* Upload CodeUtil Version String */
            CodeUtil.NativeMethods.Code_GetVersionString(buffer, buffer.Capacity);
            /* Upload Reader Info */
            info = CodeUtil.NativeMethods.Code_GetReaderInfo(deviceHandle, buffer, buffer.Capacity);
            /* Upload Communication Settings */
            info = CodeUtil.NativeMethods.Code_GetCommSettings(deviceHandle, buffer, buffer.Capacity);
            /* Upload Last Error */
            info = CodeUtil.NativeMethods.Code_GetLastError(deviceHandle);
            /* Upload Configuration */
            info = CodeUtil.NativeMethods.Code_GetConfiguration(deviceHandle, buffer, buffer.Capacity);
            /* Upload File List */
            info = CodeUtil.NativeMethods.Code_GetFileList(deviceHandle, "", 0, buffer, buffer.Capacity);
            /* Open a Terminal connection to the Reader */
            CodeUtil.OnNewDataCallback onNewDataCallback = new CodeUtil.OnNewDataCallback(NewData);
            success = CodeUtil.NativeMethods.Code_TerminalStart(deviceHandle, onNewDataCallback, true);
            if (0 == success)
            {
                Int32 err = CodeUtil.NativeMethods.Code_GetLastError(deviceHandle);
                CodeUtil.NativeMethods.Code_DestroyDevice(deviceHandle);
                return deviceHandle;
            };
            Console.WriteLine();
            Console.WriteLine("For the next 15 mins, scan a bar code or Ctrl+C to exit");
            bStart.Invoke(new Action(delegate() { bStart.Enabled = false; }));
            bStop.Invoke(new Action(delegate() { bStop.Enabled = true; }));
            bStart.Invoke(new Action(delegate() { bStart.Text = "Running"; }));
            String myComm;
            myComm = "P%260";
            CodeUtil.NativeMethods.Code_TerminalSendCommand(deviceHandle, myComm, myComm.Length);
            Thread.Sleep(1800000);

            return deviceHandle;
        }
        public void StopCodeReader(IntPtr deviceHandleMain)
        {

            /* Close the Terminal connection to the Reader */
            try
            {
                success = CodeUtil.NativeMethods.Code_TerminalStop(deviceHandle);
                if (0 == success)
                {
                    Int32 err = CodeUtil.NativeMethods.Code_GetLastError(deviceHandle);
                    CodeUtil.NativeMethods.Code_DestroyDevice(deviceHandle);
                    return;
                }
                CodeUtil.NativeMethods.Code_DestroyDevice(deviceHandle);
                //Console.Write("Press Key to end");
                //Console.ReadKey();
            }
            catch (Exception) { }
        }
        List<string> ParseHardwareList(string hardware)
        {
            List<string> devices = new List<string>();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(hardware);
            XmlNodeList xmlDevices = doc.SelectNodes("/codedevices/device");

            foreach (XmlNode device in xmlDevices)
            {
                devices.Add(device.OuterXml);
            }

            return devices;
        } // End ParseHardwareList()
        string SelectHardwareDevice(List<string> devices, string type, string path)
        {
            foreach (string device in devices)
            {
                if (device.Contains(type))
                {
                    if (path.Length == 0 || device.Contains(path))
                    {
                        return device;
                    }
                }
            }
            return "";
        }
        string GetErrorText(int number)
        {
            switch (number)
            {
                case 0:
                    return "Success";
                case 1000:
                    return "CodeUtilErrorFatal";
                case 1001:
                    return "CodeUtilErrorNoDevice";
                case 1002:
                    return "CodeUtilErrorCommError";
                case 1003:
                    return "CodeUtilErrorFileInstall";
                case 1004:
                    return "CodeUtilErrorReboot";
                case 1005:
                    return "CodeUtilErrorNoFile";
                case 1006:
                    return "CodeUtilErrorInvalidLength";
                case 1007:
                    return "CodeUtilErrorUnsupportedFile";
                case 1008:
                    return "CodeUtilErrorNoTerminal";
                case 1009:
                    return "CodeUtilErrorInvalidCommand";
                case 1010:
                    return "CodeUtilErrorCanceled";
                default:
                    return "Not a CodeUtil error: " + number.ToString();
            }
        } // End GetErrorText()
        int CheckScanStatus()
        {
            DateTime cLP = new DateTime();
            cDisable = 1;
            DateTime cThisTime = new DateTime();
            cThisTime = DateTime.Now;
            cLP = cLastPrint;
            cLP = cLP.AddSeconds(5);
            if (cLP.CompareTo(cThisTime) > 0)
                cDisable = 1;
            else
                cDisable = 0;

            return cDisable;
        }

        private Int32 NewData(IntPtr handle, IntPtr data, Int32 length)
        {
            int cCompVal;
            Form1 form1 = new Form1();
            //Int32 commandLength = 1024;
            string dataString = Marshal.PtrToStringAnsi(data);
            //Console.WriteLine();
            Console.WriteLine("Data from Reader:");
            Console.WriteLine(dataString);
            //this.tfscanarea.Text += dataString;
            //MessageBox.Show(dataString);
            /*MethodInvoker action = delegate
            { tfscanarea.Text += dataString; };
            tfscanarea.BeginInvoke(action); */
            if (dataString.Length > 3)
            {
                if (cbAutoPrint.Checked == true)
                {
                    if (CheckScanStatus() == 1)
                        return 0;
                }
                if (dataString.ToUpper() == "<|>SAVE" || dataString.ToUpper() == "<|>PRINT")
                {
                    if (cLastLabel != dataString)
                    {
                        cCompVal = completeTrans();
                        if (cCompVal == 0)
                        {
                            cLastLabel = "<|>SAVE";
                            makeBeep();
                        }
                        else
                        {
                            cLastLabel = "";
                        }
                    }
                }
                else
                {
                    tfscanarea.Invoke(new Action(delegate() { tfscanarea.Text += dataString; }));
                    ParseLabelData();
                }
            }

            return 0;
        }
        void makeBeep()
        {
            String myComm;
            myComm = "P%2650";
            CodeUtil.NativeMethods.Code_TerminalSendCommand(deviceHandle, myComm, myComm.Length);
            myComm = "#%02";
            CodeUtil.NativeMethods.Code_TerminalSendCommand(deviceHandle, myComm, myComm.Length);
            myComm = "P%260";
            CodeUtil.NativeMethods.Code_TerminalSendCommand(deviceHandle, myComm, myComm.Length);
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            /*
           For testing...
           String xmlData;
           lVendorLabel = new List<vendorLabelDefinition>();
           xmlData = "<Header><Field><Name>LOTNUMBER</Name><Prefix>&lt;LL&gt;</Prefix></Field> " +
                             "<Field><Name>RECQTY</Name><Prefix>LQ</Prefix></Field> " +
                             "<Field><Name>DATECODE</Name><Prefix>DC</Prefix></Field> " +
                             "<Field><Name>expiredate</Name><Prefix>ex</Prefix></Field> " +
                             "<type>Single</type>" +
                             "</Header>";
           setFields(lVendorLabel = parseTempXMLTest(xmlData));
           GrabLabelData(); */
            resetAll();
            getMFGDNData();
        }

        //---
        public void resetAll()
        {
            dgv1Pending.Rows.Clear();
            dgv3VendorTemplate.Rows.Clear();

            tfvendor.Text = "";
            tfpartno.Text = "";
            tfrirno.Text = ""; tfmfgpart.Text = ""; tfhdndate.Text = ""; tfsite.Text = "";
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            _usePrintPI = false;
            dgv1Pending.Refresh();
            bGo.Text = "...";
            bGo.Enabled = false;
            getMFGDNData();
            bGo.Text = "Go";
            bGo.Enabled = true;

            tabControl2_pending.SelectedIndex = 0;

            getTemplate(); //added 25-06-2013

            dgv3VendorTemplate.Refresh();

            if (dgv3VendorTemplate.Rows.Count <= 0)
            {
                chk5NoSplit.Checked = true;
            }
            else
            {
                chk5NoSplit.Checked = false;
            }

        }

        /* private void bDisableScan_Click(object sender, EventArgs e)
        {
            HH_Lib hwh = new HH_Lib();
            string[] devices = new string[1];
            devices[0] = "xx";
            //hwh.SetDeviceState(devices, false);
        }

        private void bEnableScan_Click(object sender, EventArgs e)
        {
            HH_Lib hwh = new HH_Lib();
            string[] devices = new string[1];
            devices[0] = "xx";
            //hwh.SetDeviceState(devices, true);
        } */

        private void cbSmartScan_CheckedChanged(object sender, EventArgs e)
        {
            cSearchEnable = 0;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text += Program._version;
            dgv1Pending.ReadOnly = true;
            dgv3VendorTemplate.ReadOnly = true;
            dgv0DNNumber.ReadOnly = true;
            dgv1Complete.ReadOnly = true;
            dgv5PIPending.ReadOnly = true;

            bDisableScan.Enabled = false;
            bEnableScan.Enabled = true;
            tfscanarea.ReadOnly = true;
            tfscanarea.Focus();
            _splitStringTmp = "";
            tfnooflabels.Leave += new EventHandler(tfnooflabels_Leave);
            tfnooflabels.KeyDown += new KeyEventHandler(txtkeypress);

            _splitStrTample = new List<prefixCheckbox>() {
               new prefixCheckbox(",",chk0dh),
               new prefixCheckbox("-",chk1jh),
               new prefixCheckbox(" ",chk3Space),
               new prefixCheckbox("*",chk3xh),
               new prefixCheckbox("$",chk5_meiyuan),
               new prefixCheckbox("/",chk7_zuoxiegang)
            };
            txt00Prefix.Text = _splitPrefix;
        }
        private void tfnooflabels_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                tfnooflabels.Text = "1";
            }
            else if (e.KeyCode == Keys.F2)
            {
                tfnooflabels.Text = "2";
            }
            else if (e.KeyCode == Keys.F3)
            {
                tfnooflabels.Text = "3";
            }
            else if (e.KeyCode == Keys.F4)
            {
                tfnooflabels.Text = "4";
            }
            else if (e.KeyCode == Keys.F5)
            {
                tfnooflabels.Text = "5";
            }
            else if (e.KeyCode == Keys.F6)
            {
                tfnooflabels.Text = "6";
            }
            else if (e.KeyCode == Keys.F7)
            {
                tfnooflabels.Text = "7";
            }
            else if (e.KeyCode == Keys.F8)
            {
                tfnooflabels.Text = "8";
            }
            else if (e.KeyCode == Keys.F9)
            {
                tfnooflabels.Text = "9";
            }
            else if (e.KeyCode == Keys.F10)
            {
                tfnooflabels.Text = "10";
            }
            else if (e.KeyCode == Keys.F11)
            {
                tfnooflabels.Text = "11";
            }
            else if (e.KeyCode == Keys.F12)
            {
                tfnooflabels.Text = "12";
            }
            var ek = new KeyEventArgs(Keys.Enter);
            initGoto(tfscanarea, ek);
        }

        private void initGoto(Control cl, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {
                cl.Focus();
            }
            getStrQRcode = "";
        }

        private void tfnoofcartons_KeyDown(object sender, KeyEventArgs e)
        {
            var ek = new KeyEventArgs(Keys.Enter);
            initGoto(tfscanarea, ek);
        }

        private void tfdndate_KeyDown(object sender, KeyEventArgs e)
        {
            initGoto(tfscanarea, e);
        }

        private void tftodndate_KeyDown(object sender, KeyEventArgs e)
        {
            initGoto(tfscanarea, e);
        }

        private void tfdnno_KeyDown(object sender, KeyEventArgs e)
        {
            initGoto(tfscanarea, e);
        }
        void txtkeypress(object sender, KeyEventArgs e)
        {
            initGoto(tfscanarea, e);
        }

        void tfnooflabels_Leave(object sender, EventArgs e)
        {
            getStrQRcode = "";
            //throw new NotImplementedException();
        }
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            if (kbh != null)
            {
                kbh.Stop();
            }
        }
        private void tfnooflabels_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber(tfnooflabels.Text.Trim()))
            {
                tfnooflabels.Text = "1";
            }
            else
            {
                if (!string.IsNullOrEmpty(tfdnqty.Text))
                {
                    var tmpint = Convert.ToInt32(tfnoofcartons.Text) * Convert.ToInt32(tfnooflabels.Text) * Convert.ToInt32(tfrecqty.Text);
                    if (tmpint > Convert.ToInt32(tfdnqty.Text))
                    {
                        tool_lbl_Msg.Text = "超出 dn qty 数量:" + tfnoofcartons.Text + " * " + tfnooflabels.Text + " * " + tfrecqty.Text + " = " + tmpint + " > " + tfdnqty.Text;
                        tfnooflabels.Focus();
                        return;
                    }
                }
            }


        }


        //C#中判断扫描枪输入与键盘输入
        private void tfnooflabels_KeyPress(object sender, KeyPressEventArgs e)
        {
            setEhandle(sender, e, 30);
        }

        private void setEhandle(object sender, KeyPressEventArgs e, int spanint)
        {
            if (_spanint > spanint)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void setEhandle(object sender, KeyEventArgs e, int spanint)
        {
            if (_spanint > spanint)
            {

                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        public void getInto()
        {
            DateTime tempDt = DateTime.Now;          //保存按键按下时刻的时间点
            TimeSpan ts = tempDt.Subtract(_dt);     //获取时间间隔
            //txt0ListKeyMsg.Items.Add(ts.Milliseconds);
            _spanint = ts.Milliseconds;
            //判断时间间隔，如果时间间隔大于50毫秒，则将TextBox清空

        }

        private void tfnooflabels_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            _dt = DateTime.Now;
        }
        private void tfnooflabels_KeyUp(object sender, KeyEventArgs e)
        {
            getInto();
            setEhandle(sender, e, 30);
        }
        public void ShowFrmlist(Control cl1_content, Control cl2_prefix)
        {
            getQRcode = "";
            _strlit.Clear();
            _firstOpenSelectList += 1;

            for (int i = 0; i < _strScanlit.Count; i++)
            {
                string item = _strScanlit[i].ToString();
                if (string.IsNullOrEmpty(cl2_prefix.Text))
                {
                    _strlit.Add(item);
                    for (int j = 0; j < _prefixcontList.Count; j++)
                    {
                        if (item.StartsWith(_prefixcontList[j]._prefix, true, null))
                        {
                            _strlit.Remove(item);
                            break;
                        }
                    }

                }
                else if (item.StartsWith(cl2_prefix.Text, true, null))
                {
                    _strlit.Add(item);
                }
            }
            frmlist fl = new frmlist(this, cl1_content, cl2_prefix);
            fl.ShowDialog();
        }

        public void ShowFrmlist(Control cl1_content)
        {
            getQRcode = "";
            _strlit.Clear();
            _firstOpenSelectList += 1;

            for (int i = 0; i < _strScanlit.Count; i++)
            {
                string item = _strScanlit[i].ToString();
                _strlit.Add(item);
                for (int j = 0; j < _prefixcontList.Count; j++)
                {
                    if (item.StartsWith(_prefixcontList[j]._prefix, true, null))
                    {
                        _strlit.Remove(item);
                        break;
                    }
                }


            }
            frmlist fl = new frmlist(this, cl1_content);
            fl.ShowDialog();
        }
        private void btn1Data_code_Click(object sender, EventArgs e)
        {
            ShowFrmlist(tfdatecode);
        }

        private void btn2RecMfgrPartNo_Click(object sender, EventArgs e)
        {
            ShowFrmlist(tfrecmfgrpart);

        }

        private void button3_Click(object sender, EventArgs e)
        {

            ShowFrmlist(tfmfgdate);
        }

        private void btn5RecQty_Click(object sender, EventArgs e)
        {

            ShowFrmlist(tfrecqty);
        }

        private void btn0RecPartNum_Click(object sender, EventArgs e)
        {

            ShowFrmlist(tfdnpartnumber);
        }

        private void btn4ExpireDate_Click(object sender, EventArgs e)
        {

            ShowFrmlist(tfexpiredate);
        }

        private void bnt6LotNumber_Click(object sender, EventArgs e)
        {

            ShowFrmlist(tflotno);
        }

        private void tfdnpartnumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void tfrecmfgrpart_TextChanged(object sender, EventArgs e)
        {
            //if (cbSmartScan.Checked == true)
            //{
            //    if (tfrecmfgrpart.Text.Length > 0)
            //    {
            //        SearchDNPart2(tfrecmfgrpart.Text.Trim());
            //    }
            //}
        }

        private void bDisableScan_Click(object sender, EventArgs e)
        {
            disableScan();
        }

        private void disableScan()
        {
            bDisableScan.Enabled = false;
            bEnableScan.Enabled = true;
            tfscanarea.ReadOnly = true;
        }
        private void enableScan()
        {


            bDisableScan.Enabled = true;
            bEnableScan.Enabled = false;
            tfscanarea.ReadOnly = false;
            initSet();

        }
        public void initSet()
        {
            tabControl1.SelectedIndex = 1;
            this.AcceptButton = null;

            lib0ScanDataListBox.Items.Clear();
            _strScanlit.Clear();
            _strlit.Clear();
            lib1SplitListBox.Items.Clear();
            _strNoPrefixlit.Clear();
            _strNoPrefixlitTmp.Clear();


            tfdnpartnumber.Invoke(new Action(delegate() { tfdnpartnumber.Text = ""; }));
            tfrecmfgrpart.Invoke(new Action(delegate() { tfrecmfgrpart.Text = ""; }));
            tfdatecode.Invoke(new Action(delegate() { tfdatecode.Text = ""; }));
            tfrecqty.Invoke(new Action(delegate() { tfrecqty.Text = ""; }));
            tflotno.Invoke(new Action(delegate() { tflotno.Text = ""; }));
            tfmfgdate.Invoke(new Action(delegate() { tfmfgdate.Text = ""; }));
            tfexpiredate.Invoke(new Action(delegate() { tfexpiredate.Text = ""; }));

            pbrecmfgpart.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbdnpartnumber.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbdatecode.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pblotnumber.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbmfgdate.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
            pbexpiredate.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");


            tool_lbl_Msg.Text = "";
            chk0dh.Checked = false;
            chk1jh.Checked = false;
            chk3Space.Checked = false;
            chk3xh.Checked = false;
            //chk6Ohter.Checked = false;
            chk5_meiyuan.Checked = false;
            chk7_zuoxiegang.Checked = false;
            txt5SplitOther.Text = "";


            tfscanarea.Text = "";
            tfscanarea.Focus();
        }
        private void bEnableScan_Click(object sender, EventArgs e)
        {
            enableScan();
        }



        public void initScanList()
        {
            _scanList = new List<prefixContent>();
            var scan_tfdnpartnumber = new prefixContent() { _prefix = ldnpartnumber.Text, _cl = tfdnpartnumber, _currcl = false, _currclSplit = false };
            var scan_tfrecmfgrpart = new prefixContent() { _prefix = lrecmfgpart.Text, _cl = tfrecmfgrpart, _currcl = false, _currclSplit = false };
            var scan_tfexpiredate = new prefixContent() { _prefix = lexpiredate.Text, _cl = tfexpiredate, _currcl = false, _currclSplit = false };
            var scan_tflotno = new prefixContent() { _prefix = llotnumber.Text, _cl = tflotno, _currcl = false, _currclSplit = false };

            var scan_tfdatecode = new prefixContent() { _prefix = ldatecode.Text, _cl = tfdatecode, _currcl = false, _currclSplit = false };
            var scan_tfmfgdate = new prefixContent() { _prefix = lmfgdate.Text, _cl = tfmfgdate, _currcl = false, _currclSplit = false };
            var scan_tfrecqty = new prefixContent() { _prefix = lrecqty.Text, _cl = tfrecqty, _currcl = false, _currclSplit = false };

            _scanList.Add(scan_tfdnpartnumber);
            _scanList.Add(scan_tfrecmfgrpart);
            _scanList.Add(scan_tfexpiredate);
            _scanList.Add(scan_tflotno);

            _scanList.Add(scan_tfdatecode);
            _scanList.Add(scan_tfmfgdate);
            _scanList.Add(scan_tfrecqty);

        }
        private void initCurrSelectTxt(Control cl)
        {
            for (int i = 0; i < _scanList.Count; i++)
            {
                if (_scanList[i]._cl == cl)
                {
                    _scanList[i]._currcl = true;
                    _scanList[i]._currclSplit = true;
                }
                else
                {
                    _scanList[i]._currcl = false;
                    _scanList[i]._currclSplit = false;
                }
            }
        }
        private void tfdnpartnumber_Enter(object sender, EventArgs e)
        {
            initCurrSelectTxt(tfdnpartnumber);
        }

        private void tfrecmfgrpart_Enter(object sender, EventArgs e)
        {
            initCurrSelectTxt(tfrecmfgrpart);
        }

        private void tfexpiredate_Enter(object sender, EventArgs e)
        {
            initCurrSelectTxt(tfexpiredate);
        }

        private void tflotno_Enter(object sender, EventArgs e)
        {
            initCurrSelectTxt(tflotno);
        }

        private void tfdatecode_Enter(object sender, EventArgs e)
        {
            initCurrSelectTxt(tfdatecode);
        }

        private void tfmfgdate_Enter(object sender, EventArgs e)
        {
            initCurrSelectTxt(tfmfgdate);
        }

        private void tfrecqty_Enter(object sender, EventArgs e)
        {
            initCurrSelectTxt(tfrecqty);
        }

        private void listbox0ScanData_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void listbox0ScanData_Click(object sender, EventArgs e)
        {
            selectValueToTextField(_scanList, lib0ScanDataListBox, false);
            if (lib0ScanDataListBox.SelectedItem != null)
            {
                var tmpselet = lib0ScanDataListBox.SelectedItem.ToString();
                foreach (var item in _splitStrTample)
                {
                    if (tmpselet.Contains(item._split))
                    {
                        if (item._cb.Checked)
                        {
                            item._cb.Checked = false;
                        }
                        item._cb.Checked = true;
                    }
                }
            }
        }

        private void selectValueToTextField(List<prefixContent> lt, ListBox lbvalue, bool isSplit)
        {
            if (lbvalue.SelectedItem != null)
            {
                for (int i = 0; i < lt.Count; i++)
                {
                    var item = lt[i];
                    if (!isSplit)
                    {
                        if (item._currcl == true)
                        {
                            var strselect = lbvalue.SelectedItem.ToString();
                            var index = lbvalue.SelectedIndex;
                            var strsplit = strselect.Split('|');

                            if (strsplit.Length > 0)
                            {
                                item._cl.Text = strsplit[0].ToString();
                                lbvalue.Items[index] = strsplit[0].ToString() + "|" + item._prefix.ToString();

                            }
                            else
                            {
                                item._cl.Text = strselect;
                                lbvalue.Items[index] = strselect + "|" + item._prefix.ToString();
                            }
                            item._currcl = false;
                            break;

                        }
                    }
                    else
                    {
                        if (item._currclSplit == true)
                        {
                            var strselect = lbvalue.SelectedItem.ToString();
                            item._cl.Text = strselect;
                            item._currclSplit = false;
                            break;

                        }
                    }

                }
            }
        }

        #region split from checklist


        public string[] splitFromStringWithChar(string strFrom, string strWithChar, bool useLongStringOne)
        {
            if (string.IsNullOrEmpty(strWithChar) || string.IsNullOrEmpty(strFrom))
            {
                return null;
            }
            string[] tmpreturn = null;
            if (useLongStringOne)
            {
                var tmphasIndex = strFrom.IndexOf(strWithChar);
                if (tmphasIndex > -1)
                {
                    var tmpstrLeft = strFrom.Substring(0, tmphasIndex + strWithChar.Length);
                    var tmpstrRight = strFrom.Substring(tmphasIndex + strWithChar.Length);
                    tmpreturn = new string[2] { tmpstrLeft, tmpstrRight };
                    return tmpreturn;
                }
                else
                {
                    tmpreturn = new string[1] { strFrom };
                    return tmpreturn;
                }
            }
            else
            {
                var tmparr = strFrom.Split(strWithChar.ToArray());
                return tmparr;

            }
            return tmpreturn;
        }

        public void splitFromStringWithChar(string strFrom, string strWithChar, bool useLongStringOne, ListBox lbToAdd)
        {
            var tmparr = splitFromStringWithChar(strFrom, strWithChar, useLongStringOne);
            if (tmparr == null)
            {
                return;
            }
            foreach (var item in tmparr)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                if (!lbToAdd.Items.Contains(item))
                {
                    lbToAdd.Items.Add(item);

                }
                searchByItem(item);
                searchByItemByPrefix(item, _splitPrefix, lib1SplitListBox);
            }
        }
        public void splitFromStringWithChar(ListBox lbSelect, string strWithChar, bool useLongStringOne, ListBox lbToAdd)
        {
            var strSelect = lbSelect.SelectedItem;
            if (strSelect != null)
            {
                splitFromStringWithChar(strSelect.ToString(), strWithChar, useLongStringOne, lbToAdd);
            }
        }

        public void splitFromStringWithChar(CheckBox cb, string cvalue)
        {
            lib1SplitListBox.Items.Clear();
            if (cb.Checked)
            {
                if (!_splitStringTmp.Contains(cvalue))
                {
                    _splitStringTmp += cvalue;
                }
            }
            else
            {
                _splitStringTmp = _splitStringTmp.Replace(cvalue, "");
            }
            splitFromStringWithChar(lib0ScanDataListBox, _splitStringTmp, false, lib1SplitListBox);
        }
        private void splitFromControl(CheckBox cl, char spchar)
        {

            if (cl.Checked)
            {
                _splitChar_list.Add(spchar);
            }
            else
            {
                _splitChar_list.Remove(spchar);
            }

            addItemToListFromListSplit(lib0ScanDataListBox, lib1SplitListBox);
        }

        public void addItemToListFromListSplit(ListBox fromlb, ListBox tolb)
        {
            tolb.Items.Clear();

            if (fromlb.Items.Count <= 0)
            {
                return;
            }
            if (fromlb.SelectedItem == null)
            {
                return;
            }

            string tmpselect_listbox = fromlb.SelectedItem.ToString();

            var strsplit = tmpselect_listbox.Split('|');

            foreach (var item in _splitChar_list)
            {
                if (strsplit.Length > 0)
                {
                    addItemToList(item, strsplit[0], tolb);
                }
                else
                {
                    addItemToList(item, tmpselect_listbox, tolb);
                }
            }
        }
        private void addItemToList(char dh, string strsplit, ListBox lb)
        {
            string[] tmpSplit = strsplit.Split(dh);

            foreach (var item in tmpSplit)
            {

                if (lb.Items.IndexOf(item) > 0)
                {
                    continue;
                }
                else
                {
                    lb.Items.Add(item);
                    if (chk5NoSplit.Checked)
                    {
                        if (!IsNumber(item.ToUpper()))
                        {
                            if (_usePrintPI)
                            {
                                SearchDNPart2(item.ToUpper().Trim(), dgv5PIPending, "PI_PART", "pi_mfgr_part");
                            }
                            else
                            {
                                SearchDNPart2(item.ToUpper().Trim(), dgv1Pending, "PartNumber", "MFGPartNo");
                            }
                        }
                        else
                        {
                            tfrecqty.Text = item.Trim();
                            pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\tick100.png");
                        }
                    }

                }


            }
        }
        #endregion
        private void chk0dh_CheckedChanged(object sender, EventArgs e)
        {

            splitFromStringWithChar(chk0dh, ",");
        }

        private void chk1jh_CheckedChanged(object sender, EventArgs e)
        {
            splitFromStringWithChar(chk1jh, "-");
        }

        private void chk3Space_CheckedChanged(object sender, EventArgs e)
        {
            splitFromStringWithChar(chk3Space, " ");
        }

        private void chk3xh_CheckedChanged(object sender, EventArgs e)
        {
            splitFromStringWithChar(chk3xh, "*");
        }

        private void list1boxSplit_Click(object sender, EventArgs e)
        {
            selectValueToTextField(_scanList, lib1SplitListBox, true);
        }
        public static bool IsNumber(string inputData)
        {
            if (inputData.Length > 10)
            {
                return false;
            }
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }
        public static bool IsDecimal(string inputData)
        {
            if (inputData.Length > 10)
            {
                return false;
            }
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }

        private void tfrecqty_TextChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(tfrecqty.Text))
            {
                if (!IsNumber(tfrecqty.Text))
                {
                    tfrecqty.Text = "";
                    tool_lbl_Msg.Text = "请输入正确的数字";
                    return;

                }
                tool_lbl_Msg.Text = "";
            }
            else
            {
                return;
            }
            if (!string.IsNullOrEmpty(tfdnqty.Text))
            {
                var tmpint = Convert.ToInt32(tfnoofcartons.Text) * Convert.ToInt32(tfnooflabels.Text) * Convert.ToInt32(tfrecqty.Text);
                if (tmpint > Convert.ToInt32(tfdnqty.Text))
                {
                    tool_lbl_Msg.Text = "超出 dn qty 数量:" + tfnoofcartons.Text + " * " + tfnooflabels.Text + " * " + tfrecqty.Text + " = " + tmpint + " > " + tfdnqty.Text;
                    tfrecqty.Text = "";
                    pbrecqty.Image = Image.FromFile(Application.StartupPath + @"\images\bdelete.jpg");
                    return;
                }
            }

        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                tfnooflabels.Text = "1";
            }
            else if (e.KeyCode == Keys.F2)
            {
                tfnooflabels.Text = "2";
            }
            else if (e.KeyCode == Keys.F3)
            {
                tfnooflabels.Text = "3";
            }
            else if (e.KeyCode == Keys.F4)
            {
                tfnooflabels.Text = "4";
            }
            else if (e.KeyCode == Keys.F5)
            {
                tfnooflabels.Text = "5";
            }
            else if (e.KeyCode == Keys.F6)
            {
                tfnooflabels.Text = "6";
            }
            else if (e.KeyCode == Keys.F7)
            {
                tfnooflabels.Text = "7";
            }
            else if (e.KeyCode == Keys.F8)
            {
                tfnooflabels.Text = "8";
            }
            else if (e.KeyCode == Keys.F9)
            {
                tfnooflabels.Text = "9";
            }
            else if (e.KeyCode == Keys.F10)
            {
                tfnooflabels.Text = "10";
            }
            else if (e.KeyCode == Keys.F11)
            {
                tfnooflabels.Text = "11";
            }
            else if (e.KeyCode == Keys.F12)
            {
                tfnooflabels.Text = "12";
            }

            base.OnKeyDown(e);
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                tfnooflabels.Text = "1";
            }
            else if (e.KeyCode == Keys.F2)
            {
                tfnooflabels.Text = "2";
            }
            else if (e.KeyCode == Keys.F3)
            {
                tfnooflabels.Text = "3";
            }
            else if (e.KeyCode == Keys.F4)
            {
                tfnooflabels.Text = "4";
            }
            else if (e.KeyCode == Keys.F5)
            {
                tfnooflabels.Text = "5";
            }
            else if (e.KeyCode == Keys.F6)
            {
                tfnooflabels.Text = "6";
            }
            else if (e.KeyCode == Keys.F7)
            {
                tfnooflabels.Text = "7";
            }
            else if (e.KeyCode == Keys.F8)
            {
                tfnooflabels.Text = "8";
            }
            else if (e.KeyCode == Keys.F9)
            {
                tfnooflabels.Text = "9";
            }
            else if (e.KeyCode == Keys.F10)
            {
                tfnooflabels.Text = "10";
            }
            else if (e.KeyCode == Keys.F11)
            {
                tfnooflabels.Text = "11";
            }
            else if (e.KeyCode == Keys.F12)
            {
                tfnooflabels.Text = "12";
            }
        }

        private void tfscanarea_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.F1)
            {
                tfnooflabels.Text = "1";
            }
            else if (e.KeyCode == Keys.F2)
            {
                tfnooflabels.Text = "2";
            }
            else if (e.KeyCode == Keys.F3)
            {
                tfnooflabels.Text = "3";
            }
            else if (e.KeyCode == Keys.F4)
            {
                tfnooflabels.Text = "4";
            }
            else if (e.KeyCode == Keys.F5)
            {
                tfnooflabels.Text = "5";
            }
            else if (e.KeyCode == Keys.F6)
            {
                tfnooflabels.Text = "6";
            }
            else if (e.KeyCode == Keys.F7)
            {
                tfnooflabels.Text = "7";
            }
            else if (e.KeyCode == Keys.F8)
            {
                tfnooflabels.Text = "8";
            }
            else if (e.KeyCode == Keys.F9)
            {
                tfnooflabels.Text = "9";
            }
            else if (e.KeyCode == Keys.F10)
            {
                tfnooflabels.Text = "10";
            }
            else if (e.KeyCode == Keys.F11)
            {
                tfnooflabels.Text = "11";
            }
            else if (e.KeyCode == Keys.F12)
            {
                tfnooflabels.Text = "12";
            }
        }

        private void tfdnno_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = bGo;
        }

        private void tfdnno_Leave(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        public bool removeStr(string o)
        {
            if (lib0ScanDataListBox.SelectedItem == null)
            {
                return false;
            }
            var strselect = lib0ScanDataListBox.SelectedItem.ToString();
            var index = lib0ScanDataListBox.SelectedIndex;
            var strsplit = strselect.Split('|');

            if (strsplit.Length > 1)
            {
                lib0ScanDataListBox.Items[index] = strsplit[0].ToUpper().Replace(o.ToUpper().Trim(), " ").Trim() + "|" + strsplit[1].ToString();
            }
            else
            {
                lib0ScanDataListBox.Items[index] = strselect.ToUpper().Replace(o.ToUpper().Trim(), " ").Trim();
            }
            return true;
        }
        private void chk5_3n1_CheckedChanged(object sender, EventArgs e)
        {
            splitFromStringWithChar(chk5_meiyuan, "$");
        }

        private void chk7_3n2_CheckedChanged(object sender, EventArgs e)
        {
            splitFromStringWithChar(chk7_zuoxiegang, "/");
        }

        private void txt5SplitOther_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt5SplitOther.Text))
            {
                return;
            }
            else
            {
                lib1SplitListBox.Items.Clear();
                splitFromStringWithChar(lib0ScanDataListBox, _splitStringTmp, false, lib1SplitListBox);
            }
            if (txt5SplitOther.Text.Length == 1)
            {
                splitFromStringWithChar(lib0ScanDataListBox, txt5SplitOther.Text, false, lib1SplitListBox);
            }
            else
            {
                splitFromStringWithChar(lib0ScanDataListBox, txt5SplitOther.Text, true, lib1SplitListBox);
            }
        }

        private void btn2PIID_Click(object sender, EventArgs e)
        {
            chk5NoSplit.Checked = true;
            _usePrintPI = true;
            this.AcceptButton = null;
            piid = txt1PIID.Text;
            //PI_NO,PI_LINE,
            string tmpsql = @"select  PI_PART,pi_mfgr_part,PI_LOT,PI_PO,pi_mfgr,PI_QTY,'0' as PI_Print_QTY,PI_PO_price,PI_SITE,pi_cre_time from piRemote7.pi.dbo.pi_det where pi_no='" + piid + "' and (pi_lot<> NUll or pi_lot <>'') order by pi_line";
            if (!string.IsNullOrEmpty(piid))
            {
                tabControl2_pending.SelectedIndex = 2;
                _dtPIRemote = getDataSetBySql(tmpsql).Tables[0];
                dtcomplete = _dtPIRemote.Clone();

                addPrintQtyToDGV(piid, _dtPIRemote, dgv5PIPending);

                setDGVHeaderPi(dgv5PIPending);

                checkPrintNumger(dgv5PIPending, _dtPIRemote, dgv6PICompele);

                setDGVHeaderPi(dgv6PICompele);

                if (dgv5PIPending.RowCount > 0)
                {
                    dgv5PIPending.ClearSelection();
                }
                enableScan();
            }
        }

        public void addPrintQtyToDGV(string piid, DataTable dt, DataGridView dgv)
        {
            if (dt.Rows.Count < 0)
            {
                return;
            }
            using (var db = new dbWHOperation())
            {
                var tmpPrintQty = db.vpi_sumPrintQty.Where(p => p.PI_NO.Equals(piid.Trim())).ToList();
                foreach (DataRow item in dt.Rows)
                {
                    var tmpExist = tmpPrintQty.Where(p => p.PI_PART.Equals(item["PI_PART"].ToString().Trim()) &&
                        p.pi_mfgr_part.Equals(item["pi_mfgr_part"].ToString().Trim()) &&
                        p.PI_LOT.Equals(item["PI_LOT"].ToString().Trim()) &&
                        p.PI_PO.Equals(item["PI_PO"].ToString().Trim()) &&
                        p.pi_mfgr.Equals(item["pi_mfgr"].ToString().Trim())
                        ).ToList();
                    if (tmpExist.Count > 0)
                    {

                        item["PI_Print_QTY"] = tmpExist[0].sumPrintQty.Value;
                    }
                }
            }
            dgv.DataSource = dt.DefaultView;

        }
        public void addPrintQtyToDGV(string piid, DataGridView dgv)
        {
            if (dgv.Rows.Count < 0)
            {
                return;
            }
            using (var db = new dbWHOperation())
            {
                var tmpPrintQty = db.vpi_sumPrintQty.Where(p => p.PI_NO.Equals(piid.Trim())).ToList();
                foreach (DataGridViewRow item in dgv.Rows)
                {
                    var tmpExist = tmpPrintQty.Where(p => p.PI_PART.Equals(item.Cells["PI_PART"].Value.ToString().Trim()) &&
                        p.pi_mfgr_part.Equals(item.Cells["pi_mfgr_part"].Value.ToString().Trim()) &&
                        p.PI_LOT.Equals(item.Cells["PI_LOT"].Value.ToString().Trim()) &&
                        p.PI_PO.Equals(item.Cells["PI_PO"].Value.ToString().Trim()) &&
                        p.pi_mfgr.Equals(item.Cells["pi_mfgr"].Value.ToString().Trim())
                        ).ToList();
                    if (tmpExist.Count > 0)
                    {

                        item.Cells["PI_Print_QTY"].Value = tmpExist[0].sumPrintQty.Value.ToString("#,###");
                    }
                }
            }


        }

        public void checkPrintNumger(DataGridView dgv, DataTable dt, DataGridView dgvComplete)
        {
            var printNumber = dt.AsEnumerable().Where(p => Convert.ToDecimal(p["PI_QTY"]).ToString("#,###").Equals(Convert.ToDecimal(p["PI_Print_QTY"]).ToString("#,###"))).ToList();


            foreach (var item in printNumber)
            {
                DataRow dr = dtcomplete.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = item[i];
                }
                dtcomplete.Rows.Add(dr);
            }

            dgvComplete.DataSource = dtcomplete.DefaultView;
            dgvComplete.Refresh();

            foreach (var item in printNumber)
            {
                dt.Rows.Remove(item);
            }

            dgv.DataSource = dt;
            dt.AcceptChanges();
            dgv.Refresh();

        }
        public void checkPrintNumger(DataGridView dgv, DataGridView dgvComplete)
        {
            // dgv.Refresh();
            dgvComplete.Rows.Clear();

            var printNumber = from DataGridViewRow row in dgv.Rows
                              where Convert.ToDecimal(row.Cells["PI_QTY"].Value).ToString("#,###").Equals(Convert.ToDecimal(row.Cells["PI_Print_QTY"].Value).ToString("#,###"))
                              select row;

            if (printNumber.ToList().Count > 0)
            {
                foreach (DataGridViewColumn item in dgv.Columns)
                {
                    dgvComplete.Columns.Add(item.Name, item.HeaderText);
                }

                foreach (DataGridViewRow item in printNumber)
                {
                    addDGVRowToDGVother(dgvComplete, item);
                }
                foreach (DataGridViewRow item in printNumber)
                {
                    dgv.Rows.Remove(item);
                }

                dgvComplete.Refresh();
            }

        }

        private static void addDGVRowToDGVother(DataGridView dgv, DataGridViewRow item)
        {
            DataGridViewRow row = (DataGridViewRow)item.Clone();
            for (int i = 0; i < item.Cells.Count; i++)
            {
                row.Cells[i].Value = item.Cells[i].Value;
            }
            dgv.Rows.Add(row);
        }
        private void setDGVHeaderPi(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.Columns["pi_mfgr_part"].Width = 130;
            dgv.Columns["PI_PO"].Width = 60;
            dgv.Columns["pi_mfgr"].Width = 60;
            dgv.Columns["PI_QTY"].Width = 60;
            dgv.Columns["PI_Print_QTY"].Width = 60;
            dgv.Columns["PI_PO_price"].Width = 60;
            dgv.Columns["PI_QTY"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["PI_QTY"].DefaultCellStyle.Format = "#,###";
            dgv.Columns["PI_Print_QTY"].DefaultCellStyle.BackColor = Color.LightGreen;
            dgv.Columns["PI_Print_QTY"].DefaultCellStyle.Format = "#,###";
            dgv.Columns["PI_PO_price"].DefaultCellStyle.Format = "#,###";

            dgv.Columns["PI_PART"].HeaderText = "Part Number";
            dgv.Columns["pi_mfgr_part"].HeaderText = "PO QPL Part No";
            dgv.Columns["PI_LOT"].HeaderText = "RIR Number";
            dgv.Columns["PI_PO"].HeaderText = "PO Number";
            dgv.Columns["pi_mfgr"].HeaderText = "ASN MFG P/N";
            dgv.Columns["PI_QTY"].HeaderText = "PI Qty";
            dgv.Columns["PI_Print_QTY"].HeaderText = "Printed QTY";
            dgv.Columns["PI_PO_price"].HeaderText = "MPQ";
            dgv.Columns["PI_SITE"].HeaderText = "PI SITE";
            dgv.Columns["pi_cre_time"].HeaderText = "PI Date";

            //dgv.Columns.Add("PI_Print_QTY","PrintedQTY");

            if (dgv.Rows.Count > 0)
            {
                dgv.ClearSelection();
            }
        }

        public string getCountPIdet(string piid)
        {
            //select * from pi_det where pi_no='P140033' order by pi_line
            string tmpsql = @"select count(*) from piRemote7.pi.dbo.pi_det where pi_no='" + piid + "'";// order by pi_line";
            try
            {
                using (SqlConnection conn = new SqlConnection(_cConnStrPI))
                {
                    SqlCommand cmd = new SqlCommand(tmpsql, conn);
                    conn.Open();
                    var tmpread = cmd.ExecuteReader();
                    while (tmpread.Read())
                    {
                        return tmpread[0].ToString();
                    }
                    tmpread.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return "";
        }
        public DataSet getDataSetBySql(string strsql)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(_cConnStrPI))
            {
                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(strsql, conn);
                    da.Fill(ds);
                    return ds;
                }
                catch (Exception)
                {
                    conn.Close();
                    throw;
                }
            }

        }

        private void txt1PIID_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = btn2PIID;
        }


        public bool _usePrintPI { get; set; }

        public string piid { get; set; }

        public DataTable _dtPIRemote { get; set; }

        public DataTable dtcomplete { get; set; }

        private void chk5NoSplit_CheckedChanged(object sender, EventArgs e)
        {
            if (chk5NoSplit.Checked)
            {
                enableScan();
            }
        }

        private void tfnoofcartons_TextChanged(object sender, EventArgs e)
        {
            if (!IsNumber(tfnoofcartons.Text.Trim()))
            {
                tfnoofcartons.Text = "1";
            }
            else
            {
                if (!string.IsNullOrEmpty(tfdnqty.Text))
                {
                    var tmpint = Convert.ToInt32(tfnoofcartons.Text) * Convert.ToInt32(tfnooflabels.Text) * Convert.ToInt32(tfrecqty.Text);
                    if (tmpint > Convert.ToInt32(tfdnqty.Text))
                    {
                        tool_lbl_Msg.Text = "超出 dn qty 数量:" + tfnoofcartons.Text + " * " + tfnooflabels.Text + " * " + tfrecqty.Text + " = " + tmpint + " > " + tfdnqty.Text;
                        tfnoofcartons.Focus();
                        return;
                    }
                }
            }
        }

        private void lib1SplitListBox_ControlAdded(object sender, ControlEventArgs e)
        {
            tool_lbl_Msg.Text = "dd";
        }

        private void txt00Prefix_TextChanged(object sender, EventArgs e)
        {
            _splitPrefix = txt00Prefix.Text;
        }
    }

    public class vendorLabelDefinition
    {
        private String FieldName, Prefix, RecQty, ExpireDate, MfgDate, Seperator, Index;
        public string cFieldName
        {
            get { return FieldName; }
            set { FieldName = value; }
        }
        public string cPrefix
        {
            get { return Prefix; }
            set { Prefix = value; }
        }
        public string cSeperator
        {
            get { return Seperator; }
            set { Seperator = value; }
        }
        public string cIndex
        {
            get { return Index; }
            set { Index = value; }
        }
        public string cRecQty
        {
            get { return RecQty; }
            set { RecQty = value; }
        }
        public string cExpireDate
        {
            get { return ExpireDate; }
            set { ExpireDate = value; }
        }
        public string cMfgDate
        {
            get { return MfgDate; }
            set { MfgDate = value; }
        }
    }
    //---
    class CaptureBarCode
    {

    }
    //---





}