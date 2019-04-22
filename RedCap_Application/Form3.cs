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
    public partial class Form3 : Form
    {

        string schoolStatus;
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder s1 = new StringBuilder("UPDATE School ");
            s1.Append("SET School_Status = ");
            s1.Append("'" + schoolStatus + "'");
            s1.Append("  WHERE ");
            s1.Append("School_Name = "); 
            s1.Append("'" + UserControl3.existingSchoolName + "'");

            Program.UpdateSchoolStatusQuery = s1.ToString();
            Program.UpdateSchoolStatusData(Program.sqlite_conn);
            //MessageBox.Show("Update Successful!!");
            label1.Text = "Successful!!";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.School_status = comboBox2.Text;
            schoolStatus = Program.School_status;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox2.Text = UserControl3.existingStatus;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder s1 = new StringBuilder("UPDATE School ");
            s1.Append("SET School_Status = ");
            s1.Append("'" + schoolStatus + "'");
            s1.Append("  WHERE ");
            s1.Append("School_Name = ");
            s1.Append("'" + UserControl3.existingSchoolName + "'");

            Program.UpdateSchoolStatusQuery = s1.ToString();
            Program.UpdateSchoolStatusData(Program.sqlite_conn);
            //MessageBox.Show("Update Successful!!");
            // label1.Text = "Successful!!";
            this.Close();
        }
    }
}
