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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }

        public void ResetValues()
        {
            Program.SchoolReportsIDList.Clear();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form9_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = "";
            ResetValues();
            Program.DisplaySchoolReportsGrid(Program.sqlite_conn);

            // Populate School reports line by line in text box.
            this.textBox1.Multiline = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            for (int i=0; i < Program.SchoolReportsIDList.Count; i++)
            {
                // Read list of school names from DB and add to dropdownlist upon formLoad
                // How to call combobox1 from different usercontrol
                textBox1.Text += Program.SchoolReportsIDList[i] + ", " + Program.SchoolReportsNameList[i] + "\r\n";
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
