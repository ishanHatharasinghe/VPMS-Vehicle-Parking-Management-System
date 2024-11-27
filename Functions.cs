using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace Vehicle_Parking_Management_System_Project
{
    class Functions
    {
        private SqlConnection Con;
        private SqlCommand Cmd;
        
        private string ConStr;

        public Functions()
        {
            // connection string
            ConStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=vpms2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            Con = new SqlConnection(ConStr);
            Cmd = new SqlCommand();
            Cmd.Connection = Con;
        }


        // Get data from the database ,    Retrieves data from the database.
        public DataTable GetData(string Query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter(cmd);
                        sda.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error retrieving data: " + ex.Message); // Log errors
                        throw; // Re-throw the exception for further handling if needed
                    }
                }
            }
            return dt;
        }


        // Insert, update, delete operations,   Executes SQL commands that modify the database (e.g., INSERT, UPDATE, DELETE).
        public int SetData(string Query, SqlParameter[] parameters = null)
        {
            int Cnt = 0;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand(Query, con))
                {
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    try
                    {
                        con.Open();
                        Cnt = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error executing command: " + ex.Message); // Log errors
                        throw; // Re-throw the exception for further handling if needed
                    }
                }
            }
            return Cnt;
        }


        public DataTable GetCombinedData()
        {
            DataTable combinedData = new DataTable();

            try
            {
                // SQL query to fetch data from EntryTbl, ExitTbl, and CarTbl based on VNo (Vehicle Number)
                string query = @"
            SELECT 
                e.VNo AS 'Vehicle Number', 
                e.PlateNo AS 'Plate Number', 
                c.VType AS 'Vehicle Type',
                c.DriverName AS 'Driver Name',
                c.Phone AS 'Phone',
                e.EntryTime AS 'Entry Time', 
                x.ExitTime AS 'Exit Time' 
            FROM 
                EntryTbl e
            LEFT JOIN 
                ExitTbl x ON e.VNo = x.VNo
            LEFT JOIN 
                CarTbl c ON e.PlateNo = c.PlateNo
            ORDER BY 
                e.EntryTime";

                // Fetch combined data based on the query
                combinedData = GetData(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving combined data: " + ex.Message);
                throw; // Re-throw the exception for further handling if needed
            }

            return combinedData;
        }



        public void ClearTables()
        {
            try
            {
                // Delete all records from EntryTbl
                string deleteEntryQuery = "DELETE FROM EntryTbl";
                SetData(deleteEntryQuery);

                // Delete all records from ExitTbl
                string deleteExitQuery = "DELETE FROM ExitTbl";
                SetData(deleteExitQuery);

                Console.WriteLine("All data cleared from EntryTbl and ExitTbl.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error clearing tables: " + ex.Message);
                throw; // Re-throw the exception for further handling if needed
            }
        }

        public DataTable GetVehicleData(string plateNo)
        {
            DataTable result = new DataTable();
            try
            {
                // Query to get data from CarTbl, EntryTbl, ExitTbl
                string query = @"
                SELECT 
                    CarTbl.DriverName, 
                    CarTbl.DriverNIC, 
                    CarTbl.Phone,
                    EntryTbl.SName, 
                    EntryTbl.EntryTime, 
                    ExitTbl.ExitTime
                FROM CarTbl
                LEFT JOIN EntryTbl ON CarTbl.PlateNo = EntryTbl.PlateNo
                LEFT JOIN ExitTbl ON CarTbl.PlateNo = ExitTbl.PlateNo
                WHERE CarTbl.PlateNo = @PlateNo";

                Cmd.CommandText = query;
                Cmd.Parameters.Clear();
                Cmd.Parameters.AddWithValue("@PlateNo", plateNo);

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            return result;
        }

        public int GetCount(string query)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        count = (int)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error getting count: " + ex.Message);
                        throw; // Re-throw the exception for further handling if needed
                    }
                }
            }
            return count;
        }












    }
}
