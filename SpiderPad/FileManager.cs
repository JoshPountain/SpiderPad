using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderPad
{
    internal class FileManager
    {
        string dir;
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
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\JOSH\DOCUMENTS\CODE\GITHUBPROJECTS\SPIDERPAD\SPIDERPAD\COMPONENTS.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
            //conn.ConnectionString = @"Data Source=DESKTOP-O46OL9Q;Initial Catalog=Demodb;";
            //conn.ConnectionString = @"Database=[C:\USERS\JOSH\DOCUMENTS\CODE\GITHUBPROJECTS\SPIDERPAD\SPIDERPAD\COMPONENTS.MDF];Trusted_Connection=true";
            conn.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Layers WHERE LayerUID=3", conn);
            SqlCommand add = new SqlCommand("INSERT INTO [dbo].[Layers] ([LayerUID], [Tab3Link], [Position]) VALUES (3, N'DataLad  ', 4)", conn);
            
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.InsertCommand = add;
            adapter.InsertCommand.ExecuteNonQuery();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string s =reader.GetString(1).ToString();
            reader.Read();
            string s2 = reader.GetString(1).ToString();
            int i = reader.GetInt32(0);
            return "Hello";
        }

        public void SaveFile(Storage file)
        {
            string name = file.name;
            string content = file.content;
            File.WriteAllText(dir + "\\" + name, content);
        }

        public string SetUID()
        {
            string uids, path = dir + "\\UIDs.txt";
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
            uid = manager.SetUID();
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
