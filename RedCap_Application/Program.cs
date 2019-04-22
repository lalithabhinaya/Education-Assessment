
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;

namespace RedCap_Application
{
    public static class Program
    {

        static internal string school_Name;
        //static internal int school_Id;
        // static internal int class_Id;
        static internal string classroom_Name;
        static internal string teacher_LName;
        static internal string teacher_FName;
        static internal string School_status;
        static internal string Year;
        static internal string fetchClassData;
        static internal string ReadStatusQuery;
        static internal string SchoolStatusfromDB;
        static internal int Class_SchoolID, DBClassID;
        static internal int DBMaxSchoolID;

        // static internal string RedCap_School_Code;
        // static internal string RedCap_Teacher_Code;
        static internal SQLiteConnection sqlite_conn;
         static internal List<string> SchoolList = new List<string>();
        static internal string fName;
        static internal string lName;
        static internal List<string> ClassList = new List<string>();
        static internal List<string> tempList = new List<string>();
        static internal List<int> SchoolReportsIDList = new List<int>();
        static internal List<string> SchoolReportsNameList = new List<string>();
        static internal List<int> ClassReportsIDList = new List<int>();
        static internal List<string> ClassReportsSNameList = new List<string>();
        static internal List<string> ClassReportsCNameList = new List<string>();
        static internal List<string> ClassReportsYearList = new List<string>(); 
        static internal List<string> YearList = new List<string>();
        static internal Dictionary<string, int> dict = new Dictionary<string, int>();

        static internal string sqlQuery;
        static internal string insertClassQuery, FetchYearQuery, classTimepointQuery, FetchYearFromTransitionQuery, updateTimepointQuery;
        static internal string UpdateSchoolQuery,DBYear_Status;
        static internal string UpdateSchoolStatusQuery, updatetClassQuery, FetchYearIDQuery;
        static internal string fetchSchoolIDQuery;
        static internal string FetchMaxClassIDQuery;
        static internal string FetchListofClassesQuery;
        static internal string FetchFirstNameLastNameQuery;
        static internal string fetchClassIDQuery;
        static internal int DBSchoolID, DBMaxClassID;
        static internal int flag = 0,yearTableFlag = 0,DBYear_ID,DBYearID;
        static int S_ID,C_ID;static string S_Name, CS_Name, C_Name, Y_Name;

        static internal DataTable dt1 = new DataTable();
        static internal DataTable dt2 = new DataTable();

        static Dictionary<string, int> classRoomIDMap = new Dictionary<string, int>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            sqlite_conn = CreateConnection();
            CreateTable(sqlite_conn);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

        static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {

            }
            return sqlite_conn;
        }

        static void CreateTable(SQLiteConnection conn)
        {
            // IF NOT EXISTS
            SQLiteCommand sqlite_cmd;

            string Createsql = "CREATE TABLE  IF NOT EXISTS School(School_ID INT,School_Name VARCHAR(25),School_Status VARCHAR(25), PRIMARY KEY(School_Name))";
            string Createsql1 = "CREATE TABLE IF NOT EXISTS Class(Classroom_ID INT,School_ID INT IDENTITY(1, 1),Classroom_Name VARCHAR(25),Teacher_FirstName VARCHAR(25),Teacher_LastName VARCHAR(25), PRIMARY KEY(Classroom_Name,Teacher_FirstName,Teacher_LastName));";
            string Createsql3 = "CREATE TABLE IF NOT EXISTS Year(Year_ID INT, Year VARCHAR(25),Year_Status VARCHAR(25),PRIMARY KEY(Year_ID,Year,Year_Status))";
            string Createsql4 = "CREATE TABLE IF NOT EXISTS Class_Timepoint(Classroom_ID INT, Year_ID INT,Class_Status VARCHAR(25),PRIMARY KEY(Classroom_ID, Year_ID))";

            //if (yearTableFlag == 0)
            //{
            
            //    yearTableFlag = 1;
            //}
            // string Createsql3 = "CREATE TABLE IF NOT EXISTS Year(RedCap_SchoolCode VARCHAR(25), RedCap_TeacherCode VARCHAR(25))";
            // string Createsql3 = "CREATE TABLE IF NOT EXISTS Year(RedCap_SchoolCode VARCHAR(25), RedCap_TeacherCode VARCHAR(25))";

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = Createsql1;
            sqlite_cmd.ExecuteNonQuery();
            
            sqlite_cmd.CommandText = Createsql3;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_cmd.CommandText = Createsql4;
            sqlite_cmd.ExecuteNonQuery();
           
                InsertYearTable(conn);
    
            

            //if (sqlite_cmd.ExecuteNonQuery().Equals() == null)
            //{
            // InsertYearTable(conn);
            //    yearTableFlag = 1;
            //}



        }



        static internal void InsertSchoolData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = sqlQuery;
            sqlite_cmd.ExecuteNonQuery();
        }


        static internal void InsertClassData(SQLiteConnection conn)
        {
            try
            {
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();
                sqlite_cmd.CommandText = insertClassQuery;
                sqlite_cmd.ExecuteNonQuery();
                MessageBox.Show("Successful!!");
            }

            catch(Exception ex)
            {
                MessageBox.Show("Same Class/Teacher combination already exists in database");
            }
        }

        static internal void updateClassData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = updatetClassQuery;
            sqlite_cmd.ExecuteNonQuery();

        }

        //Insert into transition table(class-timepoint table)
        static internal void InsertClass_TimepointData(SQLiteConnection conn)
        {
            try { 
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = classTimepointQuery;
            sqlite_cmd.ExecuteNonQuery();
            }

            catch(Exception ex)
            {
               
            }
        }

        //UPDAte  transition table(class-timepoint table)
        static internal void UpdateClass_TimepointData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = updateTimepointQuery;
            sqlite_cmd.ExecuteNonQuery();
        }

        //Fetch timepoints from transition table
        static internal void FetchYearFromTransition(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = FetchYearFromTransitionQuery;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                YearList.Add(myreader);
            }
        }

        //Fetch Year_ID from Year table
        static internal void FetchYearID(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = FetchYearIDQuery;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                 DBYearID = sqlite_datareader.GetInt32(0);

            }
        }

        static internal void FetchYearData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = FetchYearQuery;
            sqlite_cmd.ExecuteNonQuery();
            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
               DBYear_ID = sqlite_datareader.GetInt32(0);
                DBYear_Status = sqlite_datareader.GetString(1);

            }
            sqlite_datareader.Close();

        }

        static internal void UpdateSchoolData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = UpdateSchoolQuery;
            sqlite_cmd.ExecuteNonQuery();

            //sqlite_conn.Close();



        }

        static internal void fetchSchoolID(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = fetchSchoolIDQuery;

            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
                Class_SchoolID = sqlite_datareader.GetInt32(0);

            }
            sqlite_datareader.Close();
        }

        static internal void fetchFirstNameLastName(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = FetchFirstNameLastNameQuery;

            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
                fName = sqlite_datareader.GetString(0);
                lName = sqlite_datareader.GetString(1);

            }
            sqlite_datareader.Close();
        }

        static internal void UpdateSchoolStatusData(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = UpdateSchoolStatusQuery;
            sqlite_cmd.ExecuteNonQuery();

            //sqlite_conn.Close();
        
        }

        //Insert into Year table for the first time.(Executes only once)
        static internal void InsertYearTable(SQLiteConnection conn)
        {
            try { 
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "insert into Year(Year_ID,Year,Year_Status) values ('1','2018-2019','Active'),('2','Summer-2019','Active'),('3','2019-2020','In-Active'),('4','Summer-2020','In-Active');";
            sqlite_cmd.ExecuteNonQuery();
            }

            catch(Exception ex)
            {

            }

        }


        static internal void FetchClassID(SQLiteConnection conn)
        {
            SQLiteCommand sqlite_cmd;
            SQLiteDataReader sqlite_datareader;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = fetchClassIDQuery;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                DBClassID = sqlite_datareader.GetInt32(0);

            }
            sqlite_datareader.Close();
        }
        //method to retrieve max value of school_ID to insert into the dictionary. 
        static internal void readMaxSchoolID(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = "select max(School_ID) from School";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            // Exception when there is no school in DB
            try { 
            while (sqlite_datareader.Read())
            {
                DBSchoolID = sqlite_datareader.GetInt32(0);

            }
            }
            catch (Exception ex)
            {
                DBSchoolID = 99;

            }


            sqlite_datareader.Close();
        }


        //method to retrieve max value of Class_ID to generate correct classID
        static internal void readMaxClassID(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = FetchMaxClassIDQuery;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            string str = "";
            try
            {
                while (sqlite_datareader.Read())
                {
                    str = sqlite_datareader.GetInt32(0).ToString();

                }
               // DBMaxSchoolID = Int32.Parse(str.Substring(0, 3));
                DBMaxClassID = Int32.Parse(str.Substring(3, 2));
            }
            catch(Exception ex)
            {
                DBMaxClassID = 09;

            }

                
            sqlite_datareader.Close();
        }

        //SELECT DISTINCT School_Name FROM School
        static internal void ReadData(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "SELECT DISTINCT School_Name FROM School";

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                SchoolList.Add(myreader);
            }
        }


        //SELECT  List of classroom_Names based on School_ID
        static internal void fetchListofClasses(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = FetchListofClassesQuery;

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
               ClassList.Add(myreader);
            }
        }

        static internal void ReadStatus(SQLiteConnection conn)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = ReadStatusQuery;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                SchoolStatusfromDB = sqlite_datareader.GetString(0);
               
            }
        }


        static internal void DisplayGrid(SQLiteConnection conn)
        {
            try
            {
                SQLiteDataReader sqlite_datareader;
                SQLiteCommand sqlite_cmd;
                sqlite_cmd = conn.CreateCommand();


                sqlite_cmd.CommandText = fetchClassData;
                sqlite_datareader = sqlite_cmd.ExecuteReader();

                dt1.Load(sqlite_datareader);
          
            }

            catch (Exception ex)
            {
                //MessageBox.Show("Same Class/Teacher combination already exists in database");
            }

        }


        static internal void DisplaySchoolReportsGrid(SQLiteConnection conn)
        {
          
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();

            sqlite_cmd.CommandText = "SELECT School_ID,School_Name FROM School where School_Status = 'Recruited - Ready for testing' ORDER BY School_Name";

            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
                S_ID = sqlite_datareader.GetInt32(0);
                S_Name = sqlite_datareader.GetString(1);
                SchoolReportsIDList.Add(S_ID);
                SchoolReportsNameList.Add(S_Name);
            }
            sqlite_datareader.Close();


        }



        static internal void DisplayClassReportsGrid(SQLiteConnection conn)
        {
           
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            //work on this
            sqlite_cmd.CommandText = "select DISTINCT Class.Classroom_ID,School.School_Name,Class.Classroom_Name,Year.Year FROM School INNER JOIN Class on Class.School_ID = School.School_ID INNER JOIN Class_Timepoint on Class.Classroom_ID = Class_Timepoint.Classroom_ID INNER JOIN Year on Year.Year_ID = Class_Timepoint.Year_ID Where  School.School_Status = 'Recruited - Ready for testing' and Class_Timepoint.Class_Status = 'Active'";

            sqlite_datareader = sqlite_cmd.ExecuteReader();


            while (sqlite_datareader.Read())
            {
                C_ID = sqlite_datareader.GetInt32(0);
                CS_Name = sqlite_datareader.GetString(1);
                C_Name = sqlite_datareader.GetString(2);
                Y_Name = sqlite_datareader.GetString(3);
                ClassReportsIDList.Add(C_ID);
                ClassReportsSNameList.Add(CS_Name);
                ClassReportsCNameList.Add(C_Name);
                ClassReportsYearList.Add(Y_Name);
            }
            sqlite_datareader.Close();


        }

    }

}

