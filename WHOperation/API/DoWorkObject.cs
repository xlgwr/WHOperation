using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WHOperation.API
{
    public class DoWorkObject
    {
        public DataGridView _dgv;
        public DataGridView _dgv1;
        public bool _FrmForRefeHas = false;

        public string _strBatchId;
        public int _selectedindex;
        public int _eX;
        public int _eY;
        public bool _hasLineID;
        public string _voidColumn;

        public Color _colors;
        public string _selectColumnNameValue;
        public string _sameColumnName;
        public string _sameColumnNameCount;
        public string _sameColumnNameCountHeaderText;
        public string _sameColumnNameCountValut;
        public string _deffCellName;
        public string _deffCellValue;
        public string _strCellColName = "Batch_ID";
        public string _FrmForRefeLblMsg;

        public Color _deffcolors;

        ////frm win
        Form1 _frmDefault;

        public int _maxValue;
        public int _minValue;
        public string _compMaxMin;

        //temp
        public string _cartonidenter;
        public int _fori;

        public DoWorkObject()
        {

        }
        /// <summary>
        /// dgv cell click normal
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        public DoWorkObject(DataGridView dgv, int ex, int ey)
        {
            _FrmForRefeHas = false;
            _dgv = dgv;
            _eX = ex;
            _eY = ey;

        }
        /// <summary>
        /// dgv cell click main-item
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="dgv1"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        public DoWorkObject(DataGridView dgv, DataGridView dgv1, int ex, int ey)
        {
            _FrmForRefeHas = false;
            _dgv = dgv;
            _dgv1 = dgv1;
            _eX = ex;
            _eY = ey;

        }
        /// <summary>
        /// for btnmore reference,cell click
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="ex"></param>
        /// <param name="ey"></param>
        /// <param name="strCellColName"></param>
        public DoWorkObject(DataGridView dgv, int ex, int ey, string strCellColName)
        {
            _FrmForRefeHas = false;
            _dgv = dgv;
            _eX = ex;
            _eY = ey;
            _strCellColName = strCellColName;

        }
        /// <summary>
        /// datagridview row enter main-item
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="selectedindex"></param>
        /// <param name="eIndex"></param>
        /// <param name="colors"></param>
        /// <param name="sameColumnName"></param>
        /// <param name="deffCellName"></param>
        /// <param name="deffCellValue"></param>
        /// <param name="Deffcolors"></param>
        public DoWorkObject(DataGridView dgv, int selectedindex, int eIndex, Color colors, string sameColumnName, string deffCellName, string deffCellValue, Color Deffcolors)
        {
            _FrmForRefeHas = false;
            _dgv = dgv;
            _selectedindex = selectedindex;
            _eX = eIndex;
            _colors = colors;
            _sameColumnName = sameColumnName;
            _deffCellName = deffCellName;
            _deffCellValue = deffCellValue;
            _deffcolors = Deffcolors;

        }
        /// <summary>
        /// datagridview row enter normal
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="selectedindex"></param>
        /// <param name="eIndex"></param>
        /// <param name="colors"></param>
        /// <param name="sameColumnNameCount"></param>
        /// <param name="sameColumnNameCountValut"></param>
        /// <param name="sameColumnName"></param>
        /// <param name="deffCellName"></param>
        /// <param name="deffCellValue"></param>
        /// <param name="Deffcolors"></param>
        public DoWorkObject(DataGridView dgv, int selectedindex, int eIndex, Color colors, string sameColumnNameCount, string sameColumnNameCountValut, string sameColumnName, string deffCellName, string deffCellValue, Color Deffcolors)
        {
            _FrmForRefeHas = false;
            _dgv = dgv;
            _selectedindex = selectedindex;
            _eX = eIndex;
            _colors = colors;
            _sameColumnName = sameColumnName;
            _sameColumnNameCount = sameColumnNameCount;
            _sameColumnNameCountHeaderText = dgv.Columns[sameColumnNameCount].HeaderText;
            _sameColumnNameCountValut = sameColumnNameCountValut;
            _deffCellName = deffCellName;
            _deffCellValue = deffCellValue;
            _deffcolors = Deffcolors;

        }
        /// <summary>
        /// down xls for dgv
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="xlsType"></param>
        /// <param name="filenamePrefix"></param>
        /// <param name="filepath">isnull the path use System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"0DownLoadExcel"</param>
        public DoWorkObject(DataGridView dgv, string xlsType, string filenamePrefix, string filepath,bool autoOpen)
        {
            _dgv = dgv;
            _xlsType = xlsType;
            _filenamePrefix = filenamePrefix;
            _filepath = filepath;
            _autoOpen=autoOpen;

        }

        //down xls for dgv
        public string _filepath { get; set; }

        public string _filenamePrefix { get; set; }

        public string _xlsType { get; set; }

        public bool _autoOpen { get; set; }
    }
}
