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
        protected string[] Import(SqlManager.Tables table, string uid, string[] fields)
        {
            

            string[] data = new string[fields.Length];
            //SqlCommand cmd = new SqlCommand(sql.Read(table), conn);
            SqlCommand cmd = new SqlCommand(sql.Read(table, fields[0], uid), conn);
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

        public void Delete(string uid)
        {
            SqlCommand cmd = new SqlCommand(sql.Delete(SqlManager.Tables.UIDs, "UID", uid), conn);
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
        protected string layerUID;
        public string[] data { get; protected set; } = new string[5];

        //nodes value here propogating down to the base class incorrectly
        public Nodes(string uid, int layerUID, int locationX, int locationY, string flag, string text) : base(uid, layerUID)
        {
            string[] d = { uid, locationX.ToString(), locationY.ToString(), flag, text };
            data = d;
            setup();
        }

        /// <summary>
        /// For creating node instance that will get data from database
        /// </summary>
        /// <param name="uid"></param>
        public Nodes(string uid) : base("0")
        {
            //For when in database already
            data = Import(table, uid, fields);
            base.uid = uid;
            inDatabase = true;
            setup();
        }

        /// <summary>
        /// For creating new node without any data yet
        /// </summary>
        /// <param name="LayerUID"></param>
        public Nodes(int LayerUID) : base("0", LayerUID)
        {
            FileManager f = new FileManager();
            uid = f.GenUID().ToString();
            data[0] = uid;
            setup();
            
        }

        public void SetLayerUID()
        {
            layerUID = GetLayerUID(data[0]);
        }

        public void Update() 
        {
            SqlCommand cmd = new SqlCommand(sql.Update(table, fields, data, "UID", uid), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void SetLayerUID(string uid)
        {
            layerUID = GetLayerUID(uid);

            //Update function for database if not in it already
            if (!inDatabase)
            {

            }
        }
        private void setup()
        {
            type = SqlManager.Tables.Nodes;
        }

        /*public string[] Import()
        {
            data = Import(table, uid, fields);
            return data;
        }*/

        public int[] GetPos()
        {
            int[] pos = { Convert.ToInt32(data[1]), Convert.ToInt32(data[2]) };
            return pos;
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

        public new void Delete()
        {
            Delete(uid);
        }
    }

    public class Links : NLUIDs
    {
        protected new SqlManager.Tables table = SqlManager.Tables.Links;
        public new string[] fields = { "UID", "N1UID", "N2UID", "Text" };
        string layerUID;
        protected bool inDatabase = false;
        public string[] data { get; private set; } = new string[4];
        public Links(string uid, int layerUID, int n1uid, int n2uid, string text) : base(uid, layerUID)
        {
            string[] d = { uid, n1uid.ToString(), n2uid.ToString(), text };
            data = d;
            type = SqlManager.Tables.Links;
        }

        /// <summary>
        /// For creating link instance that will get data from database
        /// </summary>
        /// <param name="uid"></param>
        public Links(string uid) : base("0")
        {
            data = Import(table, uid, fields);
            base.uid = uid;
            data[0] = uid;
            inDatabase = true;
        }

        /// <summary>
        /// For creating new link without any data yet
        /// </summary>
        /// <param name="layerUID"></param>
        public Links(int layerUID) : base("0", layerUID)
        {
            FileManager f = new FileManager();
            uid = f.GenUID().ToString();
            data[0] = uid;

        }

        private void SetLayerUID()
        {
            layerUID = GetLayerUID(data[0]);
        }

        public void SetLayerUID(string uid)
        {
            layerUID = GetLayerUID(uid);

            //Update function for database if not in it already
            if (!inDatabase)
            {

            }


        }
        


        public new void New()
        {
            base.New();
            SqlCommand cmd = new SqlCommand(sql.Insert(table, fields, data), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public string[] Import()
        {
            data = Import(table, uid, fields);
            return data;
        }
        public new void Delete()
        {
            Delete(data[0]);
        }

        public int[] GetConnected()
        {
            int[] NodeUids = { Convert.ToInt32(data[1]), Convert.ToInt32(data[2]) };

            return NodeUids;
        }
    }
    public class NLUIDs : UIDs
    {
        protected new SqlManager.Tables table = SqlManager.Tables.NLUIDs;
        public new string[] fields = { "NLUID", "LayerUID", "Type" };
        private string[] data = new string[3];
        public NLUIDs(string uid, int layerUID) : base(uid)
        {
            string[] dat = { uid, layerUID.ToString(), type.ToString() };
            data = dat;
        }
        public NLUIDs(string uid) : base(uid)
        {
            string[] d = { uid, null, type.ToString() };
            data = d;
        }

        public NLUIDs() : base("0")
        {
            FileManager f = new FileManager();
            uid = f.GenUID().ToString();
            data[0] = uid;
        }

        protected string GetLayerUID(string uid)
        {
            
            SqlCommand cmd = new SqlCommand(sql.Read(table, fields[0], uid), conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            return reader.GetString(1);
        }

        
        protected void Update(string newUID)
        {
            
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
        
        public new void Delete()
        {
            Delete(uid);
        }
    }

    public class Layers : UIDs
    {

        protected new SqlManager.Tables table = SqlManager.Tables.Layers;
        public new string[] fields = { "LayerUID", "Name", "Position" };
        public string[] data { get; private set; } = new string[3];
        public Layers(string uid, string name, int position) : base(uid)
        {
            data[0] = uid;
            data[1] = name;
            data[2] = position.ToString();
            type = SqlManager.Tables.Layers;
        }

        /// <summary>
        /// Import layer from database
        /// </summary>
        /// <param name="uid"></param>
        public Layers(string uid) : base("0")
        {
            data = Import(table, uid, fields);
            base.uid = uid;
            data[0] = uid;
        }

        public new void New()
        {
            base.New();
            SqlCommand cmd = new SqlCommand(sql.Insert(table, fields, data), conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public string[] Import()
        {
            data = Import(table, uid, fields);
            return data;
        }

        public new void Delete()
        {
            conn.ConnectionString = con;
            SqlCommand cmd = new SqlCommand(sql.Delete(SqlManager.Tables.Layers, fields[0], uid), conn);
            
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            Delete(data[0]);
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
            Delete(uid);
        }


    }

}