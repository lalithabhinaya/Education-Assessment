using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedCap_Application
{
    public partial class UserControl3 : UserControl
    {
        static internal int update_School_flag = 0;
        static internal string existingSchoolName,existingStatus;
        public UserControl3()
        {
            InitializeComponent();
            
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    Program.ReadData(Program.sqlite_conn);
        //       foreach (string i in Program.SchoolList)
        //        {
        //            // Read list of school names from DB and add to dropdownlist upon formLoad
        //            // How to call combobox1 from different usercontrol
        //            comboBox3.Items.Add(i);                   
        //    }
        //    base.OnLoad(e);
        //}


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            Form2 form = new Form2();
            form.Show();
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == null || comboBox3.Text == "")
            {
                MessageBox.Show("Kindly select School Name!!");
            }
            else
            {
                Form3 form = new Form3();
                form.Show();
            }  
            

        }

        

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           // existingStatus = comboBox2.Text;
        }

        private void ExistingSchoolsPanel_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
             existingSchoolName = comboBox3.Text;
            comboBox2.Enabled = true;
            //Read status for this selected school from DB.
            StringBuilder s1 = new StringBuilder("SELECT School_Status FROM School ");
            s1.Append("Where ");
            s1.Append("School_Name = ");

            s1.Append("'" + existingSchoolName + "'");

            Program.ReadStatusQuery = s1.ToString();
            Program.ReadStatus(Program.sqlite_conn);

            comboBox2.Text = Program.SchoolStatusfromDB;
            existingStatus = comboBox2.Text;
            if (comboBox3.SelectedValue == null)
                comboBox2.Enabled = false;
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (comboBox3.SelectedValue == null)
                comboBox2.Enabled = false;

            //comboBox2.Text = Program.SchoolStatusfromDB;
            //existingStatus = comboBox2.Text;

        }

        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (update_School_flag == 0)
            {
                comboBox3.Items.Clear();
                foreach (string i in Program.tempList)
                {
                    // Read list of school names from DB and add to dropdownlist upon formLoad
                    
                    comboBox3.Items.Add(i);
                }

                update_School_flag = 1;
            }

            comboBox3.Text = "";


        }
    }


    }

