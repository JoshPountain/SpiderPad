using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
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
        public void Query(string table, Mode m)
        {
            SqlConnection conn = null;
            SqlCommand cmd;

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
        public string Read(Tables table, string[] fields, string[] conditions)
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
            //Nodes n = new Nodes(GenUID().ToString(), 0, 7, 8, "flgT", "Demo") ;
            //n.New();
            //Console.WriteLine("Done");

            //Testing read functionality of sql class and shit
            //Nodes n = new Nodes("2", 0, 7, 8, "flgT", "Demo") ;
            Nodes n = new Nodes("2", 9, 9, 9, "false", "false");
            n.Import("2");
            Console.WriteLine("Done");
        }
        public FileManager()
        {
            dir = Directory.GetCurrentDirectory();
            if (File.Exists(dir + "\\UIDs.txt"))
            {
                File.Create(dir + "\\UIDs.txt");
            }
        }

        public string TestDatabase()
        {
            int test = 0;
            test = GenUID();

            SqlConnection conn = new SqlConnection();
            //conn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\JOSH\DOCUMENTS\CODE\GITHUBPROJECTS\SPIDERPAD\SPIDERPAD\COMPONENTS.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            conn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Josh\Documents\Code\GithubProjects\Spiderpad\SpiderPad\Database\WebData.mdf;Integrated Security=True;Connect Timeout=30";
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Layers WHERE LayerUID=3", conn);
            SqlCommand add = new SqlCommand("INSERT INTO [dbo].[Layers] ([LayerUID], [Tab3Link], [Position]) VALUES (3, N'DataLad  ', 4)", conn);
            //add.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = add;
            adapter.InsertCommand.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string s = reader.GetString(1).ToString();
            reader.Read();
            string s2 = reader.GetString(1).ToString();
            int i = reader.GetInt32(0);
            return "Hello";
        }

        public string GetConnentionString()
        {
            string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Josh\Documents\Code\GithubProjects\Spiderpad\SpiderPad\Database\WebData.mdf;Integrated Security=True;Connect Timeout=30";
            return con;
        }

        public void AddComponent(Node c)
        {
            //Setting up connection to database
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Nodes", conn);
            conn.ConnectionString = GetConnentionString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            //Actually testing and giving UID to node
            try
            {
                while (true)
                {
                    reader.Read();
                    int i = reader.GetInt32(0);
                    if (i == c.uid)
                    {
                        c.SetUid(GenUID());
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                
                SqlCommand add = new SqlCommand("INSERT INTO [dbo].[Nodes] ([NodeUID], [Name], [Text], [PositionX], [PositionY]) VALUES (" + c.uid + ", N'" + c.name + "', N'" + c.text + "', " + c.position[0] + ", " + c.position[1] + ")", conn);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = add;
                adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void AddComponent(Link c)
        {
            ///UNtested and needs changes from the pasted code from nodes

            //Setting up connection to database
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Links", conn);
            conn.ConnectionString = GetConnentionString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            //Actually testing and giving UID to node
            try
            {
                while (true)
                {
                    reader.Read();
                    int i = reader.GetInt32(0);
                    if (i == c.uid)
                    {
                        c.SetUid(GenUID());
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                //Add to database
                SqlCommand add = new SqlCommand("INSERT INTO [dbo].[Nodes] ([NodeUID], [Name], [Text]) VALUES (" + c.uid + ", N'" + c.name + "', N'" + c.text +")", conn);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = add;
                adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void AddComponmnt(Layer c)
        {
            ///UNtested and needs changes from the pasted code from nodes
            //Setting up connection to database
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Nodes", conn);
            conn.ConnectionString = GetConnentionString();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            //Actually testing and giving UID to node
            try
            {
                while (true)
                {
                    reader.Read();
                    int i = reader.GetInt32(0);
                    if (i == c.uid)
                    {
                        c.SetUid(GenUID());
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                //Add to database
                SqlCommand add = new SqlCommand("INSERT INTO [dbo].[Nodes] ([NodeUID], [Name], [Text], [PositionX], [PositionY]) VALUES (" + c.uid + ", N'" + c.name + "', N'" + c.text + ")", conn);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.InsertCommand = add;
                adapter.InsertCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        protected void AddComponent(string name, SqlConnection con)
        {

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

            //Actually testing and giving UID to node
            try
            {
                while(true)
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
            catch(Exception e) 
            {
                
                return -1;
            }
        }


        public void SaveFile(Storage file)
        {
            string name = file.name;
            string content = file.content;
            File.WriteAllText(dir + "\\" + name, content);
        }

        public string SetUID(string DEPRECIATED)
        {
            string uids, path = dir + "\\NLUIDs.txt";
            string uid = null;
            uids = File.ReadAllText(path);
            List<int> list = new List<int>();
            int i = 0;
            //compile list of existing id's
            foreach (char c in uids)
            {
                string id = "";
                if (c != ',')
                {
                    id += c;
                }
                else
                {
                    list.Add(Convert.ToUInt16(id));
                }

            }
            //Assign a new id
            int tid = 0;
            while (true)
            {
                if (list.Contains(tid))
                {
                    tid++;
                }
                else
                {
                    return tid.ToString();
                }
            }

        }
    }
    public class Storage
    {
        string path;
        public readonly string uid;
        FileManager manager = new FileManager();
        public Storage(string name)
        {
            path = Directory.GetCurrentDirectory() + "\\" + name;
            this.name = name;
            uid = manager.SetUID("UNUSED");
        }

        public string name
        {
            private set { }
            get
            {
                return name;
            }
        }

        public string content
        {
            private set
            {
                if (content == null)
                {
                    content = value;
                }
                else
                {
                    content = content;
                }
            }
            get
            {
                return content;
            }
        }
    }
}
