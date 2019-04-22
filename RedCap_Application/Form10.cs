using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedCap_Application
{
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            Program.ClassReportsIDList.Clear();
            Program.DisplayClassReportsGrid(Program.sqlite_conn);

            // Populate Class reports line by line in text box.
            this.textBox1.Multiline = true;
            //textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.ScrollBars = ScrollBars.Both;

            for (int i = 0; i < Program.ClassReportsIDList.Count; i++)
            {
                // Read list of school names from DB and add to dropdownlist upon formLoad
                // How to call combobox1 from different usercontrol
                textBox1.Text += Program.ClassReportsIDList[i] + ", " + Program.ClassReportsSNameList[i] + " - " + Program.ClassReportsCNameList[i]+ " (" + Program.ClassReportsYearList[i] + ")" + "\r\n";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
