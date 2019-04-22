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
    public partial class UserControl6 : UserControl
    {
        public UserControl6()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // School Report- School_ID,School_Name

            Form9 fm = new Form9();
            fm.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Class Report - ClassID,School_Name - ClassName
            Form10 fm1 = new Form10();
            fm1.Show();
        }
    }
}
