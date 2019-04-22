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
    public partial class UserControl5 : UserControl
    {
        AutoCompleteStringCollection autoText = new AutoCompleteStringCollection();
        int updateClassFlag = 0, editLinkFlag = 0;
        string gridYear;
        static internal string selectedSchool;
        static internal string gridClassName, gridTeacherFname, gridTeacherLname, gridClassStatus, gridYear_ID;
        static internal int SchoolIDfromDB, ClassIDfromDB;
        public UserControl5()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Form4 form = new Form4();
            form.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form5 form = new Form5();
            form.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form6 form = new Form6();
            form.Show();
        }

        private void UserControl5_Load(object sender, EventArgs e)
        {
            Program.ReadData(Program.sqlite_conn);
            foreach (string i in Program.SchoolList)
            {
                // Read list of school names from DB and add to autotext collection

                autoText.Add(i);
            }

            //textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox2.AutoCompleteMode = AutoCompleteMode.None;
            comboBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBox2.AutoCompleteCustomSource = autoText;

            hideResults();
            button1.Enabled = false;
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            // To add Auto suggest school names to text box1
            listBox1.Items.Clear();

            foreach (String s in comboBox2.AutoCompleteCustomSource)
            {
                if ((s.ToLower().Contains(comboBox2.Text)) || (s.Contains(comboBox2.Text)))
                {

                    Console.WriteLine("Found text in: " + s);
                    listBox1.Items.Add(s);
                    listBox1.Visible = true;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            comboBox2.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
            hideResults();
            }

            catch(Exception ex)
            {
                MessageBox.Show("Please enter Class details for the Selected School");
            }
        }

        void hideResults()
        {
            listBox1.Visible = false;
        }

        void listBox1_LostFocus(object sender, System.EventArgs e)
        {
            hideResults();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                // on click of edit link, display all the grid values in the edit form
                gridClassName = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Classroom_Name"].Value);
                gridTeacherFname = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Teacher_FirstName"].Value);
                gridTeacherLname = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Teacher_LastName"].Value);
                gridYear_ID = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Year"].Value);
                gridClassStatus = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Class_Status"].Value);

                Form8 fm2 = new Form8();
                fm2.Show();
                // Empty the datagrid view
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();


            }

            if (e.ColumnIndex == 6)
            {
                //DELETE link
                const string message = "Are you sure that you would like to delete the record?";
                const string caption = "Delete School Record";
                var result = MessageBox.Show(message, caption,
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
              

                if (result == DialogResult.Yes)
                {
                   
                    //Fetch Class_ID from DB
                    gridClassName = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Classroom_Name"].Value);
                    StringBuilder s3 = new StringBuilder("SELECT Classroom_ID FROM Class ");
                    s3.Append("Where ");
                    s3.Append("Classroom_Name = ");
                    s3.Append("'" + gridClassName + "'");
                    s3.Append("AND ");
                    s3.Append("School_ID = ");
                    s3.Append("'" + SchoolIDfromDB + "'");
                    Program.fetchClassIDQuery = s3.ToString();
                    Program.FetchClassID(Program.sqlite_conn);
                    ClassIDfromDB = Program.DBClassID;

                    //Fetch Year_ID from DB
                    gridYear = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["Year"].Value);
                    StringBuilder s5 = new StringBuilder("SELECT Year_ID FROM Year WHERE Year = ");
                    s5.Append("'" + gridYear + "'");
                    Program.FetchYearIDQuery = s5.ToString();

                    Program.FetchYearID(Program.sqlite_conn);
                    int tempYearID = Program.DBYearID;

                    // change status of transition table to "Deleted" in DB.
                    StringBuilder s2 = new StringBuilder("UPDATE Class_Timepoint SET Class_Status = ");
                    s2.Append("'Deleted'");
                    s2.Append(" WHERE Classroom_ID = ");
                    s2.Append(ClassIDfromDB);
                    s2.Append(" AND Year_ID = ");
                    s2.Append(tempYearID);

                    Program.updatetClassQuery = s2.ToString();
                    Program.updateClassData(Program.sqlite_conn);

                    // Empty the datagrid view
                    dataGridView1.DataSource = null;
                    dataGridView1.Rows.Clear();
                    dataGridView1.Refresh();
                    MessageBox.Show("Class record is deleted and kindly click on Refresh button!!"); 
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataGridViewImplementation();
            //No of rows in datagrid-- MessageBox.Show(dataGridView1.Rows.Count.ToString());       
        }

        public void DataGridViewImplementation()
            {

            //Fetch School ID from DB
            StringBuilder s1 = new StringBuilder("SELECT School_ID FROM School ");
            s1.Append("Where ");
            s1.Append("School_Name = ");
            s1.Append("'" + selectedSchool + "'");

            Program.fetchSchoolIDQuery = s1.ToString();
            Program.fetchSchoolID(Program.sqlite_conn);
            SchoolIDfromDB = Program.Class_SchoolID;

            //Datagridview implementation
            dataGridView1.DataSource = null;
            Program.dt1 = new DataTable();
            StringBuilder s2 = new StringBuilder("select Class.Classroom_Name,Class.Teacher_FirstName,Class.Teacher_LastName,Class_Timepoint.Class_Status,Year.Year");
            s2.Append(" FROM Class ");
            s2.Append(" INNER JOIN Class_Timepoint on Class.Classroom_ID=Class_Timepoint.Classroom_ID ");
            s2.Append(" INNER JOIN Year on Class_Timepoint.Year_ID = Year.Year_ID ");
            s2.Append(" Where Class.School_ID = ");
            s2.Append("'" + SchoolIDfromDB + "'");
            s2.Append(" and Class_Timepoint.Class_Status = ");
            s2.Append("'Active'");

            Program.fetchClassData = s2.ToString();

            Program.DisplayGrid(Program.sqlite_conn);

            dataGridView1.DataSource = Program.dt1;

            dataGridView1.AutoSize = true;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoResizeRows();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;

            if (editLinkFlag == 0)
            {
                //Edit link in data grid view
                DataGridViewLinkColumn Editlink = new DataGridViewLinkColumn();
                Editlink.UseColumnTextForLinkValue = true;
                Editlink.HeaderText = "Edit";
                Editlink.DataPropertyName = "lnkColumn";
                Editlink.LinkBehavior = LinkBehavior.SystemDefault;
                Editlink.Text = "Edit";
                dataGridView1.Columns.Add(Editlink);

                //Delete link in data grid view
                DataGridViewLinkColumn Deletelink = new DataGridViewLinkColumn();
                Deletelink.UseColumnTextForLinkValue = true;
                Deletelink.HeaderText = "Delete";
                Deletelink.DataPropertyName = "lnkColumn";
                Deletelink.LinkBehavior = LinkBehavior.SystemDefault;
                Deletelink.Text = "Delete";
                dataGridView1.Columns.Add(Deletelink);
                editLinkFlag = 1;
            }
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //comboBox1.Items.Remove(comboBox1.SelectedValue);
            selectedSchool = comboBox2.SelectedItem.ToString();

            if(selectedSchool != null)
            {
                button1.Enabled = true;
            }
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (updateClassFlag == 0)
            {
                foreach (string i in Program.tempList)
                {
                    // Read list of school names from DB and add to dropdownlist upon formLoad
                    // How to call combobox1 from different usercontrol
                    comboBox2.Items.Add(i);
                }

                updateClassFlag = 1;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
