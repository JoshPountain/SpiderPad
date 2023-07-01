using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string Insert(Tables table, string[] fields, string[] values)
        {
            ///string interpolation
            ///INSERT INTO [dbo].[#table] (#field1, #field1,...) VALUES (#field1, #field1,...)
            ///INSERT INTO [dbo].[UIDs] ([UID], [type]) VALUES (3, N'Nodes')
            string query = $"INSERT INTO [dbo].[{table.ToString()}] ([{fields[0]}]";
            if (fields.Length != values.Length)
            {
                return $"Error number of fields : {fields.Length}, and number of values : {values.Length}, do not match, fields and data should be 1:1";
            }
            for (int i = 1; i < fields.Length; i++)
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

        public string Delete(Tables table, string condition, string uid)
        {
            string query = $"DELETE FROM [dbo].[{table}] WHERE [{condition}] = {uid}";

            if (table == Tables.Layers)
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

        /// <summary>
        /// 'UPDATE [dbo].[#table] SET [#field1] = #value1, [#field2] = #value2,... WHERE [#condition] = #uid'
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fields"></param>
        /// <param name="values"></param>
        /// <param name="condition"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string Update(Tables table, string[] fields, string[] values, string condition, string accept)
        {

            //Untested copilot generated code
            string query = $"UPDATE [dbo].[{table}] SET {fields[0]} = {values[0]}";

            for (int i = 1; i < fields.Length; i++)
            {
                query += $", {fields[i]} = '{values[i]}'";
            }
            query += $" WHERE {condition} = '{accept}'";
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
            string[] c = { conditions };
            return Read(table, f, c);
        }
        private string Read(Tables table, string[] fields, string[] conditions)
        {
            string query = $"SELECT * FROM [dbo].[{table}] WHERE ";
            try
            {
                query += $"{fields[0]}={conditions[0]}";
                for (int i = 1; i < fields.Length; i++)
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
}
