using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace SpiderPad
{

    public abstract class WebData
    {
        protected enum Attributes
        {
            UID,
            LocationX,
            LocationY,
            Flag,
            Text,
            N1UID,
            N2UID,
            LayerUID,
            Name,
            Position,
            NLUID,
            Type
        }
        protected SqlManager.Tables type;
        protected string con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Josh\Documents\Code\GithubProjects\Spiderpad\SpiderPad\Database\WebData.mdf;Integrated Security=True;Connect Timeout=30";
        protected SqlConnection conn = new SqlConnection();
        protected SqlManager sql = new SqlManager();
        protected WebData()
        {
            conn.ConnectionString = con;
        }
        public string[] Import(SqlManager.Tables table, string uid, string[] fields)
        {
            string[] data = new string[fields.Length];
            //SqlCommand cmd = new SqlCommand(sql.Read(table), conn);
            SqlCommand cmd = new SqlCommand(sql.Read(table, "UID", uid), conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            for (int i = 0; i < fields.Length; i++)
            {
                try
                {
                    data[i] = (reader.GetString(i));
                }
                catch
                {
                    data[i] = (reader.GetInt32(i).ToString());
                }
            }
            conn.Close();
            uid = data[0];
            return data;
        }

        public void Delete(string uid, string field)
        {
            SqlCommand cmd = new SqlCommand(sql.Delete(SqlManager.Tables.UIDs, field, uid), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

    }
    public class Nodes : NLUIDs
    {
        protected new SqlManager.Tables table = SqlManager.Tables.Nodes;
        public new string[] fields = { "UID", "LocationX", "LocationY", "Flag", "Text" };
        protected bool inDatabase = false;
        public string[] data { get; protected set; } = new string[5];

        //nodes value here propogating down to the base class incorrectly
        public Nodes(string uid, int layerUID, int locationX, int locationY, string flag, string text) : base(uid, layerUID)
        {
            string[] d = { uid, locationX.ToString(), locationY.ToString(), flag, text };
            type = SqlManager.Tables.Nodes;
            data = d;
        }
        public Nodes(string uid) : base("0", 0)
        {
            Import(table, uid, fields);

        }

        public new void New()
        {
            if (inDatabase)
            {
                return;
            }
            base.New();
            SqlCommand cmd = new SqlCommand(sql.Insert(table, fields, data), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            inDatabase = true;
        }

        public void Delete()
        {
            base.Delete(uid, fields[0]);
        }
    }

    public class Links : NLUIDs
    {
        protected new SqlManager.Tables table = SqlManager.Tables.Links;
        public new string[] fields = { "UID", "N1UID", "N2UID", "Text" };
        //should data contain uid (no?)
        private string[] data = new string[4];
        public Links(string uid, int layerUID, int n1uid, int n2uid, string text) : base(uid, layerUID)
        {
            string[] d = { uid, n1uid.ToString(), n2uid.ToString(), text };
            data = d;
            type = SqlManager.Tables.Links;
        }

        public Links(string uid) : base("0", 0)
        {
            Import(table, uid, fields);
        }
        public new void New()
        {
            base.New();
            SqlCommand cmd = new SqlCommand(sql.Insert(table, fields, data), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public new void Delete()
        {
            base.Delete(data[0], fields[0]);
        }

    }

    public class NLUIDs : UIDs
    {
        protected new SqlManager.Tables table = SqlManager.Tables.NLUIDs;
        public new string[] fields = { "NLUID", "LayerUID", "Type" };
        private string[] data = new string[3];
        public NLUIDs(string uid, int layerUID) : base(uid)
        {
            string[] d = { uid, layerUID.ToString(), type.ToString() };
            data = d;
        }
        public NLUIDs(string uid) : base(uid)
        {
            string[] d = { uid, null, type.ToString() };
            data = d;
        }

        
        public new void New()
        {
            //data redeclared here because it will not propogate correctly otherwise
            data[2] = type.ToString();
            base.New();
            SqlCommand cmd = new SqlCommand(sql.Insert(table, fields, data), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void Delete()
        {
            base.Delete(uid, fields[0]);
        }
    }

    public class Layers : UIDs
    {

        protected new SqlManager.Tables table = SqlManager.Tables.Layers;
        public new string[] fields = { "LayerUID", "Name", "Position" };
        private string[] data = new string[3];
        public Layers(string uid, string name, int position) : base(uid)
        {
            data[0] = uid;
            data[1] = name;
            data[2] = position.ToString();
            type = SqlManager.Tables.Layers;
        }

        public Layers(string uid) : base("0")
        {
            Import(table, uid, fields);
        }

        public new void New()
        {
            base.New();
            SqlCommand cmd = new SqlCommand(sql.Insert(table, fields, data), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void Delete()
        {
            SqlCommand cmd = new SqlCommand(sql.Delete(SqlManager.Tables.UIDs, fields[0], uid));
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            Delete(uid, "LayerUID");
        }

        
    }

    public class UIDs : WebData
    {
        protected string uid;
        //protected SqlManager.Tables type;
        protected SqlManager.Tables table = SqlManager.Tables.UIDs;
        public string[] fields = { "UID", "type" };
        private string[] data = new string[2];

        public UIDs(string uid)
        {
            data[0] = uid;


        }

        public void New()
        {
            data[1] = type.ToString();
            SqlCommand cmd = new SqlCommand(sql.Insert(table, fields, data), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            //INSERT INTO [dbo].[UIDs] ([UID], [type]) VALUES (2, NULL)
        }
        public void Delete()
        {
            base.Delete(uid, fields[0]);
        }


    }

}