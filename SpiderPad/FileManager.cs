using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderPad
{

    public class SqlManager
    {
        string conString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Josh\Documents\Code\GithubProjects\Spiderpad\SpiderPad\Database\WebData.mdf;Integrated Security = True; Connect Timeout = 30";
        public enum Mode
        {
            Create,
            Delete,
            Update
        }

        public enum Tables
        {
            Layers,
            Links,
            NLUIDs,
            Nodes,
            UIDs
        }

        protected string modeString(Mode m)
        {
            string s;
            switch (m)
            {
                case Mode.Create:
                    s = "create";
                    break;
                case Mode.Delete: 
                    s = "delete";
                    break;
                case Mode.Update:
                    s = "update";
                    break;
                default:
                    s = null; 
                    break;
            }
            return s.ToUpper();
        }

        public SqlManager()
        {

        }

        public string Insert(Tables table,string[] fields,string[] values)
        {
            ///string interpolation
            ///INSERT INTO [dbo].[#table] (#field1, #field1,...) VALUES (#field1, #field1,...)
            ///INSERT INTO [dbo].[UIDs] ([UID], [type]) VALUES (3, N'Nodes')
            string query = $"INSERT INTO [dbo].[{table.ToString()}] ([{fields[0]}]";
            if(fields.Length != values.Length)
            {
                return $"Error number of fields : {fields.Length}, and number of values : {values.Length}, do not match, fields and data should be 1:1";
            }
            for(int i  = 1; i < fields.Length; i++)
            {
                query += $",  [{fields[i]}]";
                
            }
            query += $") VALUES ({values[0]}";
            for (int i = 1; i < values.Length; i++)
            {
                query += $", N'{values[i]}'";
            }
            query += ")";
            return query;
        }

        public string Delete(Tables table,  string condition, string uid)
        {
            string query = $"DELETE FROM [dbo].[{table}] WHERE [{condition}] = {uid}";

            if(table == Tables.Layers)
            {
                return DeleteLayer(condition);
            }

            return query;
        }

        private string DeleteLayer(string layerUID)
        {
            string query = $"DELETE FROM [dbo].[Layers] WHERE [LayerUID] = {layerUID}";

            return query;
            ///database interaction should only happen in database class
            //SqlConnection conn = new SqlConnection(conString);
            //SqlCommand cmd = new SqlCommand(query, conn);
            //conn.Open();
            //cmd.ExecuteNonQuery();
            //conn.Close();
        }

        public string Update(Tables table, string[] fields, string[] values, string condition, string uid)
        {

            //Untested copilot generated code
            string query = $"UPDATE [dbo].[{table}] SET [{fields[0]}] = {values[0]}";

            for(int i = 1; i < fields.Length; i++)
            {
                query += $", [{fields[i]}] = {values[i]}";
            }
            query += $" WHERE [{condition}] = {uid}";
            return query;
        }

        public string Read(Tables table)
        {
            string query = $"SELECT * FROM [dbo].[{table}]";
            return query;
        }

        public string Read(Tables table, string field, string conditions)
        {
            string[] f = { field };
            string[] c = {conditions };
            return Read(table, f, c);
        }
        private string Read(Tables table, string[] fields, string[] conditions)
        {
            string query = $"SELECT * FROM [dbo].[{table}] WHERE ";
            try
            {
                query += $"{fields[0]}={conditions[0]}";
                for(int i = 1;  i < fields.Length; i++) 
                {
                    query += $"AND {fields[i]}={conditions[i]}";
                }
            }
            catch
            {
                return "Error";
            }
            return query;
        }


    }
    public class FileManager
    {
        string dir;
        DatabaseHandler handler = new DatabaseHandler();
        public FileManager()
        {
            dir = Directory.GetCurrentDirectory();
            if (File.Exists(dir + "\\UIDs.txt"))
            {
                File.Create(dir + "\\UIDs.txt");
            }
        }

        public ArrayList OpenDatabase()
        {
            ArrayList web = new ArrayList();
            //Lists all UIDs out + type of component
            List<string[]> parts = handler.GetParts();
            List<Nodes> nodes = new List<Nodes>();
            List<Links> links = new List<Links>();
            List<Layers> layers = new List<Layers>();

            foreach (string[] part in parts) 
            {
                switch(part[1]) 
                {
                    case "Nodes":
                        //Calls node class constructor which gets remaining data from database and saves object
                        Nodes n = new Nodes(part[0]);
                        nodes.Add(n);
                        break;

                    case "Links":
                        Links l = new Links(part[0]);
                        links.Add(l);
                        break;

                    case "Layers":
                        Layers lay = new Layers(part[0]);
                        layers.Add(lay);
                        break;



                    default:
                        break;
                }
            }

            return web;
        }
        public void Debug()
        {
            //Area to call methods to test in debugging
            //int[] i = { 1, 2 };
            //Node n = new Node(i, "TestNode", "This is a test node");
            //AddComponent(n);

            ///Testing node addition functionality
            //int[] testPos = { 4, 5 };

            //Node n = new Node(testPos, "CodeTestNode", "DemoNode");
            //n.SetUid(GenUID());
            //Console.WriteLine("T");


            //Testing functionality of the sql creator class and the abstraction in webdata class(and children)
            //Nodes n = new Nodes(GenUID().ToString(), 0, 7, 8, "flgT", "Demo");
            //n.New();
            //Console.WriteLine("Done");

            DatabaseHandler handler = new DatabaseHandler();
            //handler.TestSetup();
            //handler.TestImport();
            handler.TestDelete();

            //Testing read functionality of sql class and shit
            //Nodes n = new Nodes("2", 0, 7, 8, "flgT", "Demo") ;
            //Nodes n = new Nodes("2", 9, 9, 9, "false", "false");
            // n.Import("2");
            // Console.WriteLine("Done");
        }
        


        public string GetConnentionString()
        {
            string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Josh\Documents\Code\GithubProjects\Spiderpad\SpiderPad\Database\WebData.mdf;Integrated Security=True;Connect Timeout=30";
            return con;
        }



        public int GenUID()
        {
            int id = 0;
            List<int> taken = new List<int>();

            //Setting up connection to database
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM UIDs", conn);
            conn.ConnectionString = GetConnentionString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            //Actually testing and giving UID to component
            try
            {
                while (true)
                {
                    reader.Read();
                    int i = reader.GetInt32(0);
                    taken.Add(i);
                    if (i == id)
                    {
                        id++;
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                while (true)
                {
                    if (taken.Contains(id))
                    {
                        id++;
                    }
                    else
                    {
                        return id;
                    }
                }
            }
            catch (Exception e)
            {

                return -1;
            }
        }



      
    }
}
    