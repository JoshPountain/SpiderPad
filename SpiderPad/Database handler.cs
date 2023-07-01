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


        public void TestUpdate()
        {
            Nodes n = new Nodes("0",1000, 420, 69, "intup", "rlintup");
            n.Update();
            Links l = new Links("7", 1000, 1, 2, "IntUpdated");
            l.Update();
            Layers lay = new Layers("6", "UpLayer", 1);
            lay.Update();

        }

    
        public void TestUpdatee()
        {
            //get instance of database
            LocalStorage db = Import();
            Nodes n = db.Nodes()[0];
            
            //get uid to identify node to update
            string uid = n.data[0];
            //rewrite data
            SqlManager sql = new SqlManager();

            //  Fields to update
            List<string> fields = new List<string>();
            //      Not including uid in fields
            
            for(int i = 1; i < n.fields.Length; i++)
            {
                fields.Add(n.fields[i]);
            }

            string[] passingFields = fields.ToArray();

            //  Values to update to
            string[] values = { "69", "420", "updated", "updated" };
            
            string query = sql.Update(SqlManager.Tables.Nodes, passingFields, values, n.fields[0], n.data[0]);
            SqlCommand cmd = new SqlCommand(query, conn);
            
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            //compare
        }
        public void TestSetup()
        {
            //Testing read functionality of sql class and shit
            //Nodes n = new Nodes("2", 0, 7, 8, "flgT", "Demo") ;
            //Nodes n = new Nodes("2", 9, 9, 9, "false", "false");
            // n.Import("2");
            // Console.WriteLine("Done");
            Layers l = new Layers("6", "DemoLayer1", 1);
            l.Save();
            Nodes[] n = new Nodes[5];
            for (int i = 0; i < 5; i++) 
            {
                n[i] = new Nodes(i.ToString(), 6, i, i, $"DEMO{i}", $"DEMO{i}");
                n[i].Save();
            }
            Links link = new Links("7", 6, 0, 1, "DemoLink");
            link.Save();
            
        }

        public void TestInitialization()
        {
            //save current database contents
            var content = Import();
            //Wipe database
            WipeDatabase();
            //Create blank component instances to then have auto UID assignment and then populate with old data


            foreach (Nodes n in content.Nodes())
            {
                
            }

        }

        public void TestLinks() 
        {
            Links link = new Links("7", 6, 2, 1, "DemoLink");
            link.Save();
            //link.Delete();
        }

        public void TestDelete()
        {
            LocalStorage comp = Import();
            List<Nodes> n = comp.Nodes();
            List<Links> l = comp.Links();
            List<Layers> lay = comp.Layers();
            foreach (Nodes node in n)
            {
                node.Delete();
            }
            foreach (Links link in l)
            {
                link.Delete();
            }
            foreach (Layers layer in lay)
            {
                layer.Delete();
            }

            //Restore
            TestSetup();
        }

        public void TestImport()
        {
            Import();
        }

        /// <summary>
        /// WARNING! Wipes the database
        /// </summary>
        /// <param name="AreYouSure"></param>
        /// <param name="ReallySure"></param>
        public void WipeDatabase()
        {
            
            LocalStorage comp = Import();
            List<Nodes> n = comp.Nodes();
            List<Links> l = comp.Links();
            List<Layers> lay = comp.Layers();
            foreach (Nodes node in n)
            {
                node.Delete();
            }
            foreach (Links link in l)
            {
                link.Delete();
            }
            foreach (Layers layer in lay)
            {
                layer.Delete();
            }
        }


        public LocalStorage Import()
        {
            try
            {
                TestLinks();
            }
            catch
            {

            }
            LocalStorage web = new LocalStorage();

            SqlManager sql = new SqlManager();
            SqlCommand cmd = new SqlCommand(sql.Read(SqlManager.Tables.UIDs), conn);
            FileManager f = new FileManager();
            conn.ConnectionString = f.GetConnentionString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<int> ids = new List<int>();
            List<string> types = new List<string>();
            
            reader.Read();
            try
            {
                while (true)
                {
                    ids.Add(reader.GetInt32(0));
                    types.Add(reader.GetString(1));
                    reader.Read();
                }
            }
            catch
            {
                
            }
            int i = 0;
            foreach (string s in types)
            {
                switch (s)
                {
                    case "Nodes":
                        //"ids[i]" must be layer uid here
                        Nodes n = new Nodes(ids[i].ToString());
                        web.AddNode(n);
                        break;
                    case "Links":
                        Links l = new Links(ids[i].ToString());
                        web.AddLink(l);
                        break;
                    case "Layers":
                        Layers lay = new Layers(ids[i].ToString());
                        web.AddLayers(lay);
                        break;
                    default:
                        break;
                }
                i++;
            }
            List<string> nodeUIDs = new List<string>();
            List<string> linkUIDs = new List<string>();
            List<string> layerUIDs = new List<string>();
            foreach (Nodes n in web.Nodes())
            {
                nodeUIDs.Add(n.data[0]);
            }
            foreach (Links l in web.Links())
            {
                linkUIDs.Add(l.data[0]);
            }
            foreach (Layers lay in web.Layers())
            {
                layerUIDs.Add(lay.data[0]);
            }
            conn.Close();
            return web;

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
