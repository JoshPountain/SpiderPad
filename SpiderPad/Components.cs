using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderPad
{
    
    public abstract class Component
    {
        public int uid { get; protected set; }
        protected string[] fields;
        protected string[] values;
        protected bool uidSet { get; private set; } = false;
        public componentType type;
        public string name { get; protected set; }
        public string text { get; protected set; }
        public FileManager f = new FileManager();
        protected SqlManager sql = new SqlManager();
        public void SetUid(int input)
        {
            if (!uidSet)
            {
                uid = input;
                uidSet = true;
                return;
            }
        }

        public void New()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = f.GetConnentionString();
            //Adding record into UIDs table
            string[] fields = { "UID", "type" };
        }
        public Component(string t) 
        {
            text = t;
        }
        public Component(int i, string t)
        {
            uid = i;
            text = t;
        }

        public void Delete()
        {

        }
        public void Clone()
        {
            //Assign new one new UID
            Component c = this;
            c.uid = f.GenUID();
            //Add to database
        }
        protected void Write(SqlManager.Tables table)
        {
            SqlConnection conn = new SqlConnection();
            //Gets connection string for database
            conn.ConnectionString = f.GetConnentionString();
            //gets sql command
            SqlCommand cmd = new SqlCommand(sql.Insert(table, fields, values), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }

    public class Layer : Component
    {
        int position { get; set; }
        public Layer(string t) : base(t)
        {

        }

    }

    public class Node : Component
    {
        public int[] position { get; private set; } = new int[2];

        List<String> flags = new List<String>();
        new SqlManager.Tables name = SqlManager.Tables.Nodes;
        public Node(int[] location, string name, string text) : base(text)
        {
            
            type = componentType.Node;
            name = "Node";
            location = position;
            text = "Demo text";
            string[] f =
            {
                "UID",
                "LocationX",
                "LocationY",
                "Flag",
                "Text"
            };
            fields = f;
            //Add to database


            //Add to gui

        }

        public void Save()
        {
            ///INSERT INTO [dbo].[Nodes] ([UID], [LocationX], [LocationY], [Flag], [Text]) VALUES (NULL, NULL, NULL, NULL, NULL)
            
            
            string[] v =
            {
                uid.ToString(),
                position[0].ToString(),
                position[1].ToString(),
                //Need to add method to convert flags to string for here
                //flags.ToString(),
                "Placeholder",
                text
            };
            
            values = v;
            Write(name);
        }

        public void Move(int xOff, int yOff)
        {
            //For when moved, get new location, update table and UI
        }

        public void AddFlag(string flag)
        {
            flags.Add(flag);
            //Add to database
        }
        public void RemoveFlag(string flag)
        {
            try
            {
                flags.Remove(flag);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
    }

    public class Link : Component
    {
        int Weight, Direction;
        int Node1UID, Node2UID;

        public Link(string t) : base(t)
        {
            
        }
        public void Connect(int uuid, bool first, int uid) 
        {
            //Check UIDs are nodes and exist

            if (first)
            {
                Node1UID = uuid;
            }
            else
            {
                Node2UID = uuid;
            }
        }

        public void Create()
        {
            uid = f.GenUID();
        }
        public void Save()
        {
            if(Node1UID != null && Node2UID != null)
            {
                //Sql save code here
            }
        }
    }

    public class Type
    {
        private string value;
        public Type(string input)
        {
            
            for (int i = 0; i < 4; i++)
            {
                componentType c = (componentType)i;
                if (input == c.ToString())
                {
                    value = input;
                    break;
                }
            }
        }
        public string Val()
        {
            return value;
        }
    }
    public enum componentType
    {
        Web,
        Layer,
        Node,
        Link
    }
}
