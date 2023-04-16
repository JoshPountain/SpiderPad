using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderPad
{
    public class DatabaseHandler
    {
        SqlConnection conn = new SqlConnection();
        SqlManager sql = new SqlManager();
        public DatabaseHandler()
        {

        }

        public List<string[]> GetParts()
        {
            List<string[]> parts = new List<string[]>();
            SqlCommand cmd = new SqlCommand(sql.Read(SqlManager.Tables.UIDs), conn);
            conn.Open();
            //Storing referances to 'parts' of the web (nodes, links etc) that are currently saved in the database
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                string[] record = new string[2];
                for (int i = 0; i > 0; i++)
                {
                    //UID
                    record[0] = reader.GetInt32(i).ToString();
                    //Type
                    record[1] = reader.GetString(i);
                    parts.Add(record);
                }

            }
            catch
            {
                //When all records have been read
                
            }
            return parts;
            //return parts;
        }

    }
}
