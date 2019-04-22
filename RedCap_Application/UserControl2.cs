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
    public partial class UserControl2 : UserControl
    {

        AutoCompleteStringCollection autoText = new AutoCompleteStringCollection();
        string SchoolSS = "Recruited - Ready for testing";
        string schoolName; int count = 0;

        static int schoolID = 100; 
      
        public UserControl2()
        {
            InitializeComponent();
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Text = "Recruited - Ready for testing";
            Program.School_status = "Recruited - Ready for testing";
            
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("You are in the UserControl.Load event.");
            //Add elements to suggest collection
            count = count + 1;
           
            foreach (string i in Program.SchoolList)
            {
                // Read list of school names from DB and add to autotext collection

                autoText.Add(i);
            }

            //textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteMode = AutoCompleteMode.None;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = autoText;
            comboBox1.SelectedIndex = 0;
            hideResults();
        }

       
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
            
        }

        private void RegisterSchoolPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {

            label4.Text = " ";
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            
            Program.readMaxSchoolID(Program.sqlite_conn);

            if (schoolID == 100)
                schoolID = Program.DBSchoolID;

            int id;
            try
            {
                Program.dict.Add(schoolName, ++schoolID);
            StringBuilder s1 = new StringBuilder("INSERT INTO School ");
            s1.Append("(School_ID,School_Name,School_Status) ");
            s1.Append("VALUES ");
            Program.dict.TryGetValue(schoolName, out id);
            s1.Append("(" + "'" + id + "'" + "," + "'" + schoolName + "'" + "," + "'" + SchoolSS + "'" + ")");

            
            Program.sqlQuery = s1.ToString();
            Program.InsertSchoolData(Program.sqlite_conn);
            label4.Text = "Successful!";
            Program.tempList.Add(schoolName);
                autoText.Add(schoolName);

            }

            catch(Exception ex)
            {
                MessageBox.Show(" The School \"" + schoolName + "\" already existing in Database");
                label4.Text = "";
            }

           

            textBox1.Text = "";
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            // To add Auto suggest school names to text box1
            listBox1.Items.Clear();
           
            foreach (String s in textBox1.AutoCompleteCustomSource)
            {
                if ((s.ToLower().Contains(textBox1.Text)) || (s.Contains(textBox1.Text)))  
                {
                   
                    Console.WriteLine("Found text in: " + s);
                    listBox1.Items.Add(s);
                    listBox1.Visible = true;
                }
            }

            Program.school_Name = textBox1.Text;
            schoolName = Program.school_Name;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
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

        private void textBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                hideResults();
                return;
            }

        }
    }
}
