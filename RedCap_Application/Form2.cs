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
    public partial class Form2 : Form
    {
        string schoolName,oldSchoolName;

        public Form2()
        {
            InitializeComponent();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox2.Text = UserControl3.existingSchoolName;
            oldSchoolName = UserControl3.existingSchoolName;
        }

        private void button1_Click(object sender, EventArgs e)
        {


            StringBuilder s1 = new StringBuilder("UPDATE School ");
            s1.Append("SET School_Name = ");
            s1.Append("'" + schoolName + "'");
            s1.Append("  WHERE ");
            s1.Append("School_Name = ");
            s1.Append("'" + oldSchoolName + "'");


            Program.UpdateSchoolQuery = s1.ToString();
            Program.UpdateSchoolData(Program.sqlite_conn);
            //MessageBox.Show("Update Successful!!");
            label2.Text = "Successful!";
            Program.tempList.Remove(oldSchoolName);
            Program.tempList.Add(schoolName);
            UserControl3.update_School_flag = 0;

            this.Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {

            StringBuilder s1 = new StringBuilder("UPDATE School ");
            s1.Append("SET School_Name = ");
            s1.Append("'" + schoolName + "'");
            s1.Append("  WHERE ");
            s1.Append("School_Name = ");
            s1.Append("'" + oldSchoolName + "'");


            Program.UpdateSchoolQuery = s1.ToString();
            Program.UpdateSchoolData(Program.sqlite_conn);
            //MessageBox.Show("Update Successful!!");
            label2.Text = "Successful!";
            Program.tempList.Remove(oldSchoolName);
            Program.tempList.Add(schoolName);
            UserControl3.update_School_flag = 0;

            this.Close();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
          

            Program.school_Name = textBox2.Text;
            schoolName = Program.school_Name;
        }
    }
}
