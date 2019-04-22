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
    public partial class Form8 : Form
    {
        string formClassName, formTeacherFName, formTeacherLName,formClassStatus, classnameBeforeEdit, tempschoolName;
        string formYearID;int ClassIDfromDB, SchoolIDfromDB;

        public Form8()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {//RESET Button
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox2.Text = "";
            comboBox1.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            formClassName = textBox1.Text;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            formYearID = comboBox2.Text;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            textBox1.Text = UserControl5.gridClassName;
            textBox2.Text = UserControl5.gridTeacherFname;
            textBox3.Text = UserControl5.gridTeacherLname;
            comboBox2.Text = UserControl5.gridYear_ID.ToString();
            comboBox1.Text = UserControl5.gridClassStatus;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            formTeacherFName = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            formTeacherLName = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            formClassStatus = comboBox1.Text;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Submit Button
            // Fetch school ID and Class ID to insert into Class table
            tempschoolName = UserControl5.selectedSchool;
            StringBuilder s1 = new StringBuilder("SELECT School_ID FROM School ");
            s1.Append("Where ");
            s1.Append("School_Name = ");

            s1.Append("'" + tempschoolName + "'");

            Program.fetchSchoolIDQuery = s1.ToString();
            Program.fetchSchoolID(Program.sqlite_conn);
            SchoolIDfromDB = Program.Class_SchoolID;

            //Fetch Class_ID from DB
            classnameBeforeEdit =  UserControl5.gridClassName;
            StringBuilder s3 = new StringBuilder("SELECT Classroom_ID FROM Class ");
            s3.Append("Where ");
            s3.Append("Classroom_Name = ");
            s3.Append("'" + classnameBeforeEdit + "'");
            s3.Append("AND ");
            s3.Append("School_ID = ");
            s3.Append("'" + SchoolIDfromDB + "'");
            Program.fetchClassIDQuery = s3.ToString();
            Program.FetchClassID(Program.sqlite_conn);
            ClassIDfromDB = Program.DBClassID;

            // Update new class information into Class table.
            string Year = formYearID;
            StringBuilder s2 = new StringBuilder("UPDATE Class ");
            s2.Append("SET  Classroom_Name = ");
            s2.Append("'" + formClassName + "'" + ",");
            s2.Append("Teacher_FirstName = ");
            s2.Append("'" + formTeacherFName + "'" + ",");
            s2.Append("Teacher_LastName = ");
            s2.Append("'" + formTeacherLName + "'");
            s2.Append(" WHERE Classroom_ID = ");
            s2.Append(ClassIDfromDB);
            s2.Append("  AND School_ID = ");
            s2.Append(SchoolIDfromDB);

            Program.updatetClassQuery = s2.ToString();
            Program.updateClassData(Program.sqlite_conn);

            //Update year values in transition table.
            //Fetch year_ID from year Table
            StringBuilder s5 = new StringBuilder("SELECT Year_ID FROM Year WHERE Year = ");
            s5.Append("'" + Year + "'");
            Program.FetchYearIDQuery = s5.ToString();

            Program.FetchYearID(Program.sqlite_conn);
            int tempYearID = Program.DBYearID;

            //Now we have DBYearID value of updated value
            //Fetch already existing year_ID from table
            String oldyear = UserControl5.gridYear_ID;
            StringBuilder s7 = new StringBuilder("SELECT Year_ID FROM Year WHERE Year = ");
            s7.Append("'" + oldyear + "'");
            Program.FetchYearIDQuery = s7.ToString();
            Program.FetchYearID(Program.sqlite_conn);
            int existingYearID = Program.DBYearID;

            StringBuilder s6 = new StringBuilder("UPDATE Class_Timepoint ");
            s6.Append("SET  Year_ID = ");
            s6.Append(tempYearID + ",");
            s6.Append("Class_Status = ");
            s6.Append("'" + formClassStatus + "'");
            s6.Append(" WHERE Classroom_ID = ");
            s6.Append(ClassIDfromDB);
            s6.Append(" AND Year_ID = ");
            s6.Append(existingYearID);
            
            Program.updateTimepointQuery = s6.ToString();
            Program.UpdateClass_TimepointData(Program.sqlite_conn);

            //// Refresh the datagrid with updated values
            //StringBuilder s4 = new StringBuilder("SELECT Classroom_Name,Teacher_FirstName,Teacher_LastName,Class_Status");
            //s4.Append(" FROM Class ");
            //s4.Append("WHERE School_ID IN (");
            //s4.Append("SELECT School_ID FROM School ");
            //s4.Append(" WHERE School_Name = ");
            //s4.Append("'" + tempschoolName + "'" + ")");

            //Program.fetchClassData = s4.ToString();

            //Program.DisplayGrid(Program.sqlite_conn);

            // Unable to call Datagrid in this screen.

            // UserControl5.DataGridViewImplementation();

            MessageBox.Show("Successful and kindly click on Refresh button!!");
            this.Close();
        }
    }
}
