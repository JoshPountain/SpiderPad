using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
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


        public void TestSetup()
        {
            //Testing read functionality of sql class and shit
            //Nodes n = new Nodes("2", 0, 7, 8, "flgT", "Demo") ;
            //Nodes n = new Nodes("2", 9, 9, 9, "false", "false");
            // n.Import("2");
            // Console.WriteLine("Done");
            Layers l = new Layers("6", "DemoLayer1", 1);
            l.New();
            Nodes[] n = new Nodes[5];
            for (int i = 0; i < 5; i++) 
            {
                n[i] = new Nodes(i.ToString(), 6, i, i, $"DEMO{i}", $"DEMO{i}");
                n[i].New();
            }
            //Links link = new Links("7", 6, 0, 1, "DemoLink");
            //link.New();
            
        }

        public void TestLinks() 
        {
            Links link = new Links("7", 6, 2, 1, "DemoLink");
            //link.New();
            link.Delete();
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
