using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WHOperation
{
    public partial class frmlist : Form
    {
        Form1 _frm1;
        Control _cl;
        Control _cl_prefix;
        static bool _changeSelectp=false;

        public frmlist()
        {
            InitializeComponent();
        }
        public frmlist(Form1 frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }
        public frmlist(Form1 frm1, Control cl_content, Control cl_prefix)
        {
            InitializeComponent();
            _frm1 = frm1;
            _cl = cl_content;
            _cl_prefix = cl_prefix;

            if (_frm1._firstOpenSelectList == 1)
            {
                _frm1._strNoPrefixlit = _frm1._strNoPrefixlitTmp;
            }
            if (!string.IsNullOrEmpty(cl_prefix.Text))
            {
                foreach (var item in _frm1._strlit)
                {
                    listBox1.Items.Add(item);
                }
            }
            else
            {
                foreach (var item in _frm1._strNoPrefixlit)
                {
                    listBox1.Items.Add(item);
                }
            }


        }
        private void frmlist_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Control.MousePosition.X - this.Width / 2, Control.MousePosition.Y + _cl.Height);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }
            //MessageBox.Show(listBox1.SelectedItem.ToString());
            if (string.IsNullOrEmpty(_cl_prefix.Text))
            {
                _frm1._strtmp = _cl.Text;
                _cl.Text = listBox1.SelectedItem.ToString();
                _changeSelectp = true;
            }
            else
            {
                _cl.Text = _frm1.getPrefixOfContent(listBox1.SelectedItem.ToString());
            }
        }
        private void frmList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_changeSelectp)
            {
                _frm1._strNoPrefixlit.Remove(_cl.Text);
                if (!string.IsNullOrEmpty(_frm1._strtmp))
                {
                    _frm1._strNoPrefixlit.Add(_frm1._strtmp);
                }
            }
            _changeSelectp = false;

        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
