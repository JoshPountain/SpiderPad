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
            //handler.TestDelete();
            handler.TestInitialization();


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
    