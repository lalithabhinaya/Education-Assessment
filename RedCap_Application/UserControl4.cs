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
    public partial class UserControl4 : UserControl
    {
        AutoCompleteStringCollection autoText = new AutoCompleteStringCollection();
        string classSS = "Active";
        //static int classID = 10;
        int DBSchoolID, register_SChool_flag = 0, addTimePointFlag = 0, readClassListFlag = 0, tab1_year_id, tab2_year_id, year_flag = 0;
        int tempClassID, exactClassID, currentClassID, ClassIDfromDB;
        string tab1_teacherLname, tab1_schoolName, tab1_year, tab1_className, tab1_teacherFname;
        string tab2_teacherLname, tab2_schoolName, tab2_year, tab2_className, tab2_teacherFname;

        public UserControl4()
        {
            InitializeComponent();
        }
        public void ResetValues()
        {
            classSS = "Active";
            //static int classID = 10;

            tab1_teacherLname = null; tab1_year = null; tab1_className = null; tab1_teacherFname = null;
            tab2_teacherLname = null; tab2_year = null; tab2_className = null; tab2_teacherFname = null;
            comboBox5.Items.Clear();
            Program.YearList.Clear();
            Program.ClassList.Clear();
            comboBox3.Items.Clear();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void UserControl4_Load(object sender, EventArgs e)
        {
           // Program.ReadData(Program.sqlite_conn);
            foreach (string i in Program.SchoolList)
            {
                // Read list of school names from DB and add to autotext collection

                autoText.Add(i);
            }

            //textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox4.AutoCompleteMode = AutoCompleteMode.None;
            comboBox4.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBox4.AutoCompleteCustomSource = autoText;
            
            hideResults();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Program.teacher_LName = textBox3.Text;
            tab1_teacherLname = Program.teacher_LName;
        }
        /*fetch school ID from school table based on school name-- first combobox
         * insert into school id, classid
         */

        private void button1_Click(object sender, EventArgs e)
        {
            if ((tab1_schoolName != null) && ((tab1_className != null)) && ((tab1_teacherFname != null)) && ((tab1_teacherLname != null)) && ((tab1_year != null)))
            {
                StringBuilder s1 = new StringBuilder("Select max(Classroom_ID) from Class where School_ID = ");
                s1.Append(DBSchoolID);

                Program.FetchMaxClassIDQuery = s1.ToString();
                Program.readMaxClassID(Program.sqlite_conn);
                tempClassID = ++Program.DBMaxClassID;
                exactClassID = int.Parse(DBSchoolID.ToString() + tempClassID.ToString());

                //Fetch Year_ID and Year_Status from Year Table
                StringBuilder s3 = new StringBuilder("Select Year_ID,Year_Status  FROM Year Where Year = ");
                s3.Append("'" + tab1_year + "'");

                Program.FetchYearQuery = s3.ToString();
                Program.FetchYearData(Program.sqlite_conn);
                tab1_year_id = Program.DBYear_ID;

                // Insert new class information into Class table.
                StringBuilder s2 = new StringBuilder("INSERT INTO Class ");
                s2.Append("(Classroom_ID,School_ID,Classroom_Name,Teacher_FirstName,Teacher_LastName,Class_Status) ");
                s2.Append("VALUES ");
                s2.Append("(" + "'" + exactClassID + "'" + "," + "'" + DBSchoolID + "'" + "," + "'" + tab1_className + "'" + "," + "'" + tab1_teacherFname + "'" + "," + "'" + tab1_teacherLname + "'" + "," + "'" + classSS + "'" + ")"); //+ "'" + tab1_year_id + "'" + ","


                Program.insertClassQuery = s2.ToString();
                Program.InsertClassData(Program.sqlite_conn);



                
                //empty the fields on submit
                this.textBox1.Text = "";
                this.textBox2.Text = "";
                this.textBox3.Text = "";
                this.comboBox1.Text = "";
                ResetValues();
            }
            else
            {
                MessageBox.Show("Kindly fill all the fields!!");
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.school_Name = comboBox4.Text;
            tab1_schoolName = Program.school_Name;

            StringBuilder s1 = new StringBuilder("SELECT School_ID FROM School ");
            s1.Append("Where ");
            s1.Append("School_Name = ");

            s1.Append("'" + tab1_schoolName + "'");

            Program.fetchSchoolIDQuery = s1.ToString();
            Program.fetchSchoolID(Program.sqlite_conn);
            DBSchoolID = Program.Class_SchoolID;
            comboBox1.Items.Clear();

            //Add items to Year dropdown upon selecting school.
            comboBox1.Items.Add("2018-2019");
            comboBox1.Items.Add("Summer-2019");
            comboBox1.Items.Add("2019-2020");
            comboBox1.Items.Add("Summer-2020");


            // label13.Text = "Successful!!";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            tab2_className = comboBox3.Text;
            StringBuilder s1 = new StringBuilder("SELECT Teacher_FirstName,Teacher_LastName FROM Class ");
            s1.Append("Where ");
            s1.Append("School_ID =  ");
            s1.Append(DBSchoolID);
            s1.Append(" AND Classroom_ID IN (");

            s1.Append("SELECT Classroom_ID FROM Class ");
            s1.Append(" WHERE Classroom_NAME = ");
            s1.Append("'" + tab2_className + "'");
            s1.Append(")");

            Program.FetchFirstNameLastNameQuery = s1.ToString();
            Program.fetchFirstNameLastName(Program.sqlite_conn);

            textBox5.Text = Program.fName;
            textBox6.Text = Program.lName;
            tab2_teacherFname = Program.fName;
            tab2_teacherLname = Program.lName;

            // Free already existing year fields
            Program.YearList.Clear();
            comboBox5.Items.Clear();

                 //Add items to Year dropdown upon selecting school - Fetch remaining timepoints which are not avaialbe in DB(Transition table and Year table).          
                 //Fetch Class_ID from DB
                 StringBuilder s3 = new StringBuilder("SELECT Classroom_ID FROM Class ");
                s3.Append("Where ");
                s3.Append("Classroom_Name = ");
                s3.Append("'" + tab2_className + "'");
                s3.Append("AND ");
                s3.Append("School_ID = ");
                s3.Append("'" + DBSchoolID + "'");
                Program.fetchClassIDQuery = s3.ToString();
                Program.FetchClassID(Program.sqlite_conn);
                ClassIDfromDB = Program.DBClassID;

                //Fetch remaining timepoints which are not avaialbe in DB(Transition table and Year table).
                StringBuilder s4 = new StringBuilder("SELECT Year FROM Year WHERE Year_ID NOT IN (SELECT Year_ID FROM Class_Timepoint where Classroom_ID = ");
                s4.Append("'" + ClassIDfromDB + "')");
                Program.FetchYearFromTransitionQuery = s4.ToString();
                Program.FetchYearFromTransition(Program.sqlite_conn);

                for (int i = 0; i < Program.YearList.Count; i++)
                {
                    comboBox5.Items.Add(Program.YearList[i]);
                }
                if (comboBox5.Items.Count == 0)
                {
                    MessageBox.Show("All Available timepoints have been assigned to this Teacher/Classroom.");
                    button1.Enabled = false;
                }

               
            

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            //    if ((tab2_schoolName == null) || (tab2_className == null) || (tab2_year == null))
            //    {
            //        button2.Enabled = false;
            //        MessageBox.Show("Kindly fill all the fields!!");
            //        button2.Enabled = true;

            //        if ((tab2_schoolName != null) && (tab2_className != null) && (tab2_year != null))
            //        {
            //            button2.Enabled = true;

            //        }

            //    }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((tab1_schoolName != null) && ((tab1_className != null)) && ((tab1_teacherFname != null)) && ((tab1_teacherLname != null)) && ((tab1_year != null)))
            {
                StringBuilder s1 = new StringBuilder("Select max(Classroom_ID) from Class where School_ID = ");
                s1.Append(DBSchoolID);

                Program.FetchMaxClassIDQuery = s1.ToString();
                Program.readMaxClassID(Program.sqlite_conn);
                tempClassID = ++Program.DBMaxClassID;
                exactClassID = int.Parse(DBSchoolID.ToString() + tempClassID.ToString());

                //Fetch Year_ID and Year_Status from Year Table
                StringBuilder s3 = new StringBuilder("Select Year_ID,Year_Status  FROM Year Where Year = ");
                s3.Append("'" + tab1_year + "'");

                Program.FetchYearQuery = s3.ToString();
                Program.FetchYearData(Program.sqlite_conn);
                tab1_year_id = Program.DBYear_ID;

                // Insert new class information into Class table.
                StringBuilder s2 = new StringBuilder("INSERT INTO Class ");
                s2.Append("(Classroom_ID,School_ID,Classroom_Name,Teacher_FirstName,Teacher_LastName) ");
                s2.Append("VALUES ");
                s2.Append("(" + "'" + exactClassID + "'" + "," + "'" + DBSchoolID + "'" + "," + "'" + tab1_className + "'" + "," + "'" + tab1_teacherFname + "'" + "," + "'" + tab1_teacherLname + "'" + ")");
                Program.insertClassQuery = s2.ToString();
                Program.InsertClassData(Program.sqlite_conn);

                // Insert Year_ID in transition table
                StringBuilder s4 = new StringBuilder("INSERT INTO Class_Timepoint ");
                s4.Append("(Classroom_ID,Year_ID,Class_Status) ");
                s4.Append("VALUES ");
                s4.Append("(" + "'" + exactClassID + "'" + "," + "'" + tab1_year_id + "'" + "," + "'" + classSS + "'" + ")");

                Program.classTimepointQuery = s4.ToString();
                Program.InsertClass_TimepointData(Program.sqlite_conn);


                //MessageBox.Show("Successful!!");
                //empty the fields on submit
                this.textBox1.Text = "";
                this.textBox2.Text = "";
                this.textBox3.Text = "";
                this.comboBox1.Text = "";
                ResetValues();
            }
            else
            {
                MessageBox.Show("Kindly fill all the fields!!");
            }
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            // To add Auto suggest school names to text box1
            listBox1.Items.Clear();

            foreach (String s in comboBox4.AutoCompleteCustomSource)
            {
                if ((s.ToLower().Contains(comboBox4.Text)) || (s.Contains(comboBox4.Text)))
                {

                    Console.WriteLine("Found text in: " + s);
                    listBox1.Items.Add(s);
                    listBox1.Visible = true;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
            hideResults();
        }

        void hideResults()
        {
            listBox1.Visible = false;
        }

        void listBox1_LostFocus(object sender, System.EventArgs e)
        {
            hideResults();
        }



        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            //button2.Enabled = false;
            // verify whether form fields are filled or not
            if ((tab2_schoolName != null) && (tab2_className != null) && (tab2_year != null))
            {
                //Fetch Class_ID from DB
                StringBuilder s1 = new StringBuilder("SELECT Classroom_ID FROM Class ");
                s1.Append("Where ");
                s1.Append("Classroom_Name = ");
                s1.Append("'" + tab2_className + "'");
                Program.fetchClassIDQuery = s1.ToString();
                Program.FetchClassID(Program.sqlite_conn);
                currentClassID = Program.DBClassID;

                //Fetch Year_ID and Year_Status from Year Table
                StringBuilder s3 = new StringBuilder("Select Year_ID,Year_Status  FROM Year Where Year = ");
                s3.Append("'" + tab2_year + "'");

                Program.FetchYearQuery = s3.ToString();
                Program.FetchYearData(Program.sqlite_conn);
                tab2_year_id = Program.DBYear_ID;


                // Insert new timepoint  information of existing class into transitiontable Table

                StringBuilder s2 = new StringBuilder("INSERT INTO Class_Timepoint ");
                s2.Append("(Classroom_ID,Year_ID,Class_Status) ");
                s2.Append("VALUES ");
                s2.Append("(" + "'" + currentClassID + "'" + "," + "'" + tab2_year_id + "'"+ "," + "'" + classSS + "'" + ")");

                Program.classTimepointQuery = s2.ToString();
                Program.InsertClass_TimepointData(Program.sqlite_conn);

                MessageBox.Show("Successful!!");
                this.comboBox3.Text = "";
                this.textBox5.Text = "";
                this.textBox6.Text = "";
                this.comboBox5.Text = "";
                ResetValues();
            }
            else
            {
                MessageBox.Show("Kindly fill all the fields!!");
            }
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {

            //if ((tab1_schoolName == null) || (tab1_className == null) || (tab1_year == null) || (tab1_teacherFname == null) || (tab1_teacherLname == null))
            //{
            //    button1.Enabled = false;
            //    MessageBox.Show("Kindly fill all the fields!!");
            //    button1.Enabled = true;

            //    if ((tab1_schoolName != null) && (tab1_className != null) && (tab1_year != null))
            //    {
            //        button1.Enabled = true;

            //    }

            //}
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.Year = comboBox1.Text;
            tab1_year = Program.Year;

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            tab2_year = comboBox5.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            //button2.Enabled = false;
            // verify whether form fields are filled or not
            if ((tab2_schoolName != null) && (tab2_className != null) && (tab2_year != null))
            {
                //Fetch Class_ID from DB
                StringBuilder s1 = new StringBuilder("SELECT Classroom_ID FROM Class ");
                s1.Append("Where ");
                s1.Append("Classroom_Name = ");
                s1.Append("'" + tab2_className + "'");
                Program.fetchClassIDQuery = s1.ToString();
                Program.FetchClassID(Program.sqlite_conn);
                currentClassID = Program.DBClassID;

                //Fetch Year_ID and Year_Status from Year Table
                StringBuilder s3 = new StringBuilder("Select Year_ID,Year_Status  FROM Year Where Year = ");
                s3.Append("'" + tab2_year + "'");

                Program.FetchYearQuery = s3.ToString();
                Program.FetchYearData(Program.sqlite_conn);
                tab2_year_id = Program.DBYear_ID;


                // Insert new timepoint  information of existing class into transitiontable Table

                StringBuilder s2 = new StringBuilder("INSERT INTO Class_Timepoint ");
                s2.Append("(Classroom_ID,Year_ID) ");
                s2.Append("VALUES ");
                s2.Append("(" + "'" + currentClassID + "'" + "," + "'" + tab2_year_id + "'" + "," + "'" + classSS + "'" + ")");

                Program.classTimepointQuery = s2.ToString();
                Program.InsertClass_TimepointData(Program.sqlite_conn);

                MessageBox.Show("Successful!!");
                this.comboBox3.Text = "";
                this.textBox5.Text = "";
                this.textBox6.Text = "";
                this.comboBox5.Text = "";
                ResetValues();
            }
            else
            {
                MessageBox.Show("Kindly fill all the fields!!");
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.school_Name = comboBox2.Text;
            tab2_schoolName = Program.school_Name;

            StringBuilder s2 = new StringBuilder("SELECT School_ID FROM School ");
            s2.Append("Where ");
            s2.Append("School_Name = ");

            s2.Append("'" + tab2_schoolName + "'");

            Program.fetchSchoolIDQuery = s2.ToString();
            Program.fetchSchoolID(Program.sqlite_conn);
            DBSchoolID = Program.Class_SchoolID;

            // TRy to pull list of classes for selected school here.
            StringBuilder s1 = new StringBuilder("SELECT DISTINCT Classroom_Name FROM Class ");
            s1.Append("Where ");
            s1.Append("School_ID =  ");
            s1.Append(DBSchoolID);
            //Reset values
            ResetValues();
            // Clear already selected class value
            comboBox3.Text= "";
            Program.FetchListofClassesQuery = s1.ToString();
            Program.fetchListofClasses(Program.sqlite_conn);

            //if (readClassListFlag == 0)
            //{
                foreach (string i in Program.ClassList)
                {
                    // Read list of school names from DB and add to dropdownlist upon formLoad
                    // How to call combobox1 from different usercontrol
                    comboBox3.Items.Add(i);
                }

                //readClassListFlag = 1;
            //}

        }

        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            

        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {

            if (addTimePointFlag == 0)
            {
                foreach (string i in Program.tempList)
                {
                    // Read list of school names from DB and add to dropdownlist upon formLoad
                    // How to call combobox1 from different usercontrol
                    comboBox2.Items.Add(i);
                }

                addTimePointFlag = 1;
            }
        }

        private void comboBox4_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (register_SChool_flag == 0)
            {
                foreach (string i in Program.tempList)
                {
                    // Read list of school names from DB and add to dropdownlist upon formLoad
                    // How to call combobox1 from different usercontrol
                    comboBox4.Items.Add(i);
                }

                register_SChool_flag = 1;
            }


            if (textBox1.Text.Length == 0)
            {
                hideResults();
                return;
            }

        }

        private void textBox7_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {

            //foreach (string i in Program.SchoolList)
            //{
            //    // Read list of school names from DB and add to dropdownlist upon formLoad
            //    this.comboBox4.Items.Add(i);
            //}
            //base.OnLoad(e);
        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Program.school_Name = comboBox1.Text;
        //    schoolName = Program.school_Name;
        //   // label13.Text = "Successful!!";
        //}

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Program.teacher_FName = textBox2.Text;
            tab1_teacherFname = Program.teacher_FName;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Program.classroom_Name = textBox1.Text;
            tab1_className = Program.classroom_Name;
        }
    }
}
