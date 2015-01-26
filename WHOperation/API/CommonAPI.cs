using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using MessageBox = System.Windows.Forms.MessageBox;

using System.Data.Entity;

using System.IO;
using System.Data.SqlClient;
using System.Threading;
using System.Data;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using System.Linq.Expressions;
using System.Drawing;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WHOperation.API
{
    class CommonAPI
    {
        //attr
        public bool _isSortAscending { get; set; }
        public DataGridViewColumn _sortColumn { get; set; }


        public int _intnext { get; set; }

        Form1 _frmDefault;

        public CommonAPI() { initPara(); }

        public CommonAPI(Form1 frmDefault)
        {
            _frmDefault = frmDefault;
            initPara();
        }

        private void initPara()
        {
            _isSortAscending = false;
            _intnext = 0;
        }
        #region base function

        /// <summary>
        ///  ControlIsNullOrEmpty
        /// </summary>
        /// <param name="msg">messageBox msg</param>
        /// <param name="cl">control name</param>
        /// <returns>true,false</returns>
        public bool ControlIsNullOrEmpty(string msg, Control cl)
        {
            if (string.IsNullOrEmpty(cl.Text))
            {
                MessageBox.Show(msg);
                cl.Focus();
                return true;
            }
            return false;
        }

        /// <summary>
        ///  ControlIsNullOrEmpty
        /// </summary>
        /// <param name="msg">label msg</param>
        /// <param name="cl">control name</param>
        /// <returns>true,false</returns>
        public bool ControlIsNullOrEmpty(string msg, Control cl, Control clnotice)
        {
            if (string.IsNullOrEmpty(cl.Text))
            {
                cl.Focus();
                clnotice.Text = msg;
                return true;
            }
            return false;
        }
        /// <summary>
        /// set control text
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="strMsg"></param>
        /// <param name="enable"></param>
        /// <param name="visible"></param>
        public void setControlText(System.Windows.Forms.Control ctl, string strMsg, bool enable, bool visible)
        {
            _frmDefault.Invoke(new Action(delegate
            {
                ctl.Text = strMsg;
                ctl.Enabled = enable;
                ctl.Visible = visible;
            }));

        }
        /// <summary>
        /// button enable
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="strMsg"></param>
        /// <param name="enable"></param>
        /// <param name="visible"></param>
        public void setControlText(System.Windows.Forms.Control ctl, bool enable, bool visible)
        {
            _frmDefault.Invoke(new Action(delegate
            {
                ctl.Enabled = enable;
                ctl.Visible = visible;
            }));

        }
        /// <summary>
        /// set toolstriptitem state bar
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="strMsg"></param>
        /// <param name="enable"></param>
        /// <param name="visible"></param>
        public void setControlText(System.Windows.Forms.ToolStripItem ctl, string strMsg, bool enable, bool visible)
        {
            _frmDefault.Invoke(new Action(delegate
            {
                ctl.Text = strMsg;
                ctl.Enabled = enable;
                ctl.Visible = visible;
            }));

        }

        /// <summary>
        /// dgv true: rowcount,false: column count
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="isRowCount">true: rowcount,false: column count</param>
        /// <returns></returns>
        public int getControlInt(DataGridView dgv, bool isRowCount)
        {
            int tmpText = 0;
            _frmDefault.Invoke(new Action(delegate
            {
                if (isRowCount)
                {
                    tmpText = dgv.Rows.Count;
                }
                else
                {
                    tmpText = dgv.ColumnCount;
                }
            }));
            return tmpText;
        }
        public string getControlText(Control ctl)
        {
            string tmpText = "";
            _frmDefault.Invoke(new Action(delegate
            {
                tmpText = ctl.Text;
            }));
            return tmpText;
        }
        public object getControlText(DataGridView dgv, int xindex, int yxindex)
        {
            object tmpText = null;
            _frmDefault.Invoke(new Action(delegate
            {
                tmpText = dgv.Rows[xindex].Cells[yxindex].Value;
            }));
            return tmpText;
        }
        public string getControlText(DataGridView dgv, int yindex)
        {
            string tmpText = "";
            _frmDefault.Invoke(new Action(delegate
            {
                tmpText = dgv.Columns[yindex].HeaderText;
            }));
            return tmpText;
        }
        #endregion

        /// <summary>
        /// EnquireByPart
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="cellsHeader"></param>
        /// <param name="strcontains"></param>
        public void EnquireByPart(DataGridView dgv, string cellsHeader, string strcontains)
        {
            _frmDefault.Invoke(new Action(delegate()
            {
                int rowcount = dgv.Rows.Count;

                if (rowcount > 0)
                {
                    for (int i = _intnext; i < rowcount - 1; i++)
                    {
                        if (dgv.Rows[i].Cells[cellsHeader].Value.ToString().ToLower().Contains(strcontains.ToLower()))
                        {

                            dgv.Rows[i].Cells[cellsHeader].Selected = true;

                            _intnext = i + 1;
                            if (_intnext >= rowcount - 1)
                            {
                                _intnext = 0;
                            }
                            break;
                        }
                        if (i >= rowcount - 2)
                        {
                            _intnext = 0;
                            dgv.ClearSelection();
                        }
                    }
                }
            }));


        }

        public string EnquireByHeadText(DataGridView dgv, string headerText, string strcontains)
        {
            var tmpStartnext = _intnext;
            var _columnIndex = 2;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (dgv.Columns[i].HeaderText.Equals(headerText))
                {
                    _columnIndex = i;
                    break;
                }
            }
            int rowcount = dgv.Rows.Count;
            if (rowcount > 0)
            {
                for (int i = _intnext; i < rowcount - 1; i++)
                {
                    if (dgv.Rows[i].Cells[_columnIndex].Value.ToString().ToLower().Contains(strcontains.ToLower()))
                    {

                        dgv.Rows[i].Cells[_columnIndex].Selected = true;

                        _intnext = i + 1;
                        if (_intnext >= rowcount - 1)
                        {
                            _intnext = 0;
                        }

                        return "success: find in [" + headerText + "] with " + strcontains + " at " + _intnext + " Row," + " start " + tmpStartnext;
                        break;
                    }
                    if (i >= rowcount - 2)
                    {
                        _intnext = 0;
                        dgv.ClearSelection();
                    }
                }
                return "Error: No find in [" + headerText + "] with " + strcontains + " start " + tmpStartnext;
            }
            return "No Data";


        }
        #region down xls for data
        #region xls
        MemoryStream GetExcelStream()
        {
            //Write the stream data of workbook to the root directory
            MemoryStream file = new MemoryStream();
            hssfworkbook_xls.Write(file);
            return file;
        }

        void GenerateData()
        {
            ISheet sheet1 = hssfworkbook_xls.CreateSheet("Sheet1");

            sheet1.CreateRow(0).CreateCell(0).SetCellValue("This is a Sample");
            int x = 1;
            for (int i = 1; i <= 15; i++)
            {
                IRow row = sheet1.CreateRow(i);
                for (int j = 0; j < 15; j++)
                {
                    row.CreateCell(j).SetCellValue(x++);
                }
            }
        }

        void InitializeWorkbook()
        {
            hssfworkbook_xls = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "NPOI Team";
            hssfworkbook_xls.DocumentSummaryInformation = dsi;

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "NPOI SDK Example";
            hssfworkbook_xls.SummaryInformation = si;
        }
        #endregion
        #region xlsx

        #endregion
        //遍历获取类的属性及属性的值：
        public string getProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    tStr += string.Format("{0}:{1},", name, value);
                }
                else
                {
                    getProperties(value);
                }
            }
            return tStr;
        }
        public void downLoadExcel<T>(IList<T> listT, string xlsType, string filenamePrefix, string filepath)
          where T : class
        {
            xssfworkbook_xlsx = new XSSFWorkbook();

            string filename = filenamePrefix + "D" + DateTime.Now.ToString("yyMMddHHms") + ".xlsx";//yyyyMMddHHmmssff

            if (string.IsNullOrEmpty(filepath))
            {
                filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"0DownLoadExcel";
            }

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string tmpAllFilepathAndName = System.IO.Path.Combine(filepath, filename);

            ISheet sheet1 = xssfworkbook_xlsx.CreateSheet(filename);


            int tmpColumnsCount = typeof(T).GetProperties().Count();
            int tmpRowsCount = listT.Count;

            int x = 1;
            IRow rowHeader = sheet1.CreateRow(0);
            for (int i = 0; i < tmpColumnsCount; i++)
            {
                rowHeader.CreateCell(i).SetCellValue(x++);
            }

            for (int i = 1; i <= tmpRowsCount; i++)
            {
                IRow row = sheet1.CreateRow(i);

                System.Reflection.PropertyInfo[] properties = listT[i - 1].GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

                for (int j = 0; j < tmpColumnsCount; j++)
                {
                    var tmpCellValue = properties.ElementAt(j).GetValue(listT[i - 1], null);

                    if (tmpCellValue == null)
                    {
                        tmpCellValue = "";
                    }

                    if (tmpCellValue.GetType() == System.TypeCode.String.GetType())
                    {
                        row.CreateCell(j).SetCellValue(tmpCellValue.ToString());
                    }
                    else if (tmpCellValue.GetType() == System.TypeCode.Decimal.GetType())
                    {
                        var tmpCellValue_convert = Convert.ToDouble(tmpCellValue);
                        row.CreateCell(j).SetCellValue(tmpCellValue_convert);
                    }
                    else if (tmpCellValue.GetType() == System.TypeCode.Double.GetType())
                    {
                        var tmpCellValue_convert = Convert.ToDouble(tmpCellValue);
                        row.CreateCell(j).SetCellValue(tmpCellValue_convert);
                    }
                    else if (tmpCellValue.GetType() == System.TypeCode.DateTime.GetType())
                    {
                        var tmpCellValue_convert = Convert.ToDateTime(tmpCellValue);
                        row.CreateCell(j).SetCellValue(tmpCellValue_convert);
                    }
                    else
                    {
                        row.CreateCell(j).SetCellValue(tmpCellValue.ToString());
                    }
                }
            }
            using (var f = File.Create(@tmpAllFilepathAndName))
            {
                xssfworkbook_xlsx.Write(f);
            }
        }
        /// <summary>
        /// DataGridView dgv, string xlsType, string filenamePrefix, string filepath,bool autoOpen
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="xlsType"></param>
        /// <param name="filenamePrefix"></param>
        /// <param name="filepath"></param>
        public void downLoadExcel(object o)
        {

            var dwo = (DoWorkObject)o;
            try
            {
                currmsg = "Start init excel file name and path.";
                setControlText(_frmDefault.tool_lbl_Msg, currmsg, true, true);

                xssfworkbook_xlsx = new XSSFWorkbook();

                string filename = dwo._filenamePrefix + "_" + DateTime.Now.ToString("yyMMddHHmmssff") + ".xlsx";//yyyyMMddHHmmssff

                if (string.IsNullOrEmpty(dwo._filepath))
                {
                    dwo._filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"0DownLoadExcel";

                }
                if (!Directory.Exists(dwo._filepath))
                {
                    Directory.CreateDirectory(dwo._filepath);
                }
                string tmpAllFilepathAndName = System.IO.Path.Combine(dwo._filepath, filename);



                ISheet sheet1 = xssfworkbook_xlsx.CreateSheet(filename);

                int tmpColumnsCount = getControlInt(dwo._dgv, false);//dwo._dgv.Columns.Count;
                int tmpRowsCount = getControlInt(dwo._dgv, true);// dwo._dgv.Rows.Count;

                currmsg = "Start create excel file,it has Rows:" + tmpRowsCount + ",Columns:" + tmpColumnsCount;
                setControlText(_frmDefault.tool_lbl_Msg, currmsg, true, true);

                int x = 1;
                IRow rowHeader = sheet1.CreateRow(0);
                for (int i = 0; i < tmpColumnsCount; i++)
                {
                    var tmpHeadText = getControlText(dwo._dgv, i);//dwo._dgv.Columns[i].HeaderText;
                    if (tmpHeadText == null)
                    {
                        tmpHeadText = x++.ToString();
                    }
                    currmsg = "Start write Header text at:" + (i + 1) + ", value:" + tmpHeadText.ToString();
                    setControlText(_frmDefault.tool_lbl_Msg, currmsg, true, true);

                    rowHeader.CreateCell(i).SetCellValue(tmpHeadText.ToString());
                }

                for (int i = 1; i <= tmpRowsCount; i++)
                {
                    IRow row = sheet1.CreateRow(i);
                    for (int j = 0; j < tmpColumnsCount; j++)
                    {
                        var tmpCellValue = getControlText(dwo._dgv, i - 1, j);// dwo._dgv.Rows[i - 1].Cells[j].Value;

                        if (tmpCellValue == null)
                        {
                            tmpCellValue = "";
                        }

                        if (tmpCellValue.GetType() == System.TypeCode.String.GetType())
                        {
                            row.CreateCell(j).SetCellValue(tmpCellValue.ToString());
                        }
                        else if (tmpCellValue.GetType() == System.TypeCode.Decimal.GetType())
                        {
                            var tmpCellValue_convert = Convert.ToDouble(tmpCellValue);
                            row.CreateCell(j).SetCellValue(tmpCellValue_convert);
                        }
                        else if (tmpCellValue.GetType() == System.TypeCode.Double.GetType())
                        {
                            var tmpCellValue_convert = Convert.ToDouble(tmpCellValue);
                            row.CreateCell(j).SetCellValue(tmpCellValue_convert);
                        }
                        else if (tmpCellValue.GetType() == System.TypeCode.DateTime.GetType())
                        {
                            var tmpCellValue_convert = Convert.ToDateTime(tmpCellValue);
                            row.CreateCell(j).SetCellValue(tmpCellValue_convert);
                        }
                        else
                        {
                            row.CreateCell(j).SetCellValue(tmpCellValue.ToString());
                        }

                        currmsg = "That has Rows:" + tmpRowsCount + ",Columns:" + tmpColumnsCount + ",Start write at Rows:" + (i + 1) + ",Columns:" + (j + 1) + ",Value:" + tmpCellValue.ToString();
                        setControlText(_frmDefault.tool_lbl_Msg, currmsg, true, true);

                    }
                }
                using (var f = File.Create(@tmpAllFilepathAndName))
                {
                    currmsg = "Start save Excel file to " + tmpAllFilepathAndName;
                    setControlText(_frmDefault.tool_lbl_Msg, currmsg, true, true);
                    xssfworkbook_xlsx.Write(f);
                    currmsg = "Success: save Excel file to " + tmpAllFilepathAndName;
                    setControlText(_frmDefault.tool_lbl_Msg, currmsg, true, true);
                    _frmDefault._strDownLoadExcel = tmpAllFilepathAndName;
                }
                if (dwo._autoOpen)
                {
                    OpenFolderAndSelectFile(tmpAllFilepathAndName);
                }
            }
            catch (Exception ex)
            {

                currmsg = "Error:" + ex.Message;
                setControlText(_frmDefault.tool_lbl_Msg, currmsg, true, true);
            }

        }
        public void downLoadExcel_Thread(object o)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(downLoadExcel), o);
        }
        // open file from path
        #region open file from path
        public void OpenFolderAndSelectFile(String fileFullName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + fileFullName;
            //psi.UseShellExecute = true;
            //psi.Verb = "open";
            System.Diagnostics.Process.Start(psi);
        }
        public void initOpenFile(string file, string filename)
        {
            string allfileNamepath;
            string pathname = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + file;
            if (string.IsNullOrEmpty(filename))
            {

                allfileNamepath = pathname;
            }
            else
            {

                allfileNamepath = System.IO.Path.Combine(pathname, filename);
            }
            OpenFolderAndSelectFile(allfileNamepath);
        }
        #endregion
        #region  清理过时的Excel文件

        private void ClearFile(string FilePath)
        {
            String[] Files = System.IO.Directory.GetFiles(FilePath);
            if (Files.Length > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        System.IO.File.Delete(Files[i]);
                    }
                    catch
                    {
                    }

                }
            }
        }
        #endregion


        #endregion
        //////////////////////////////////add new

        public HSSFWorkbook hssfworkbook_xls { get; set; }
        public XSSFWorkbook xssfworkbook_xlsx { get; set; }

        public string currmsg { get; set; }
    }

}
