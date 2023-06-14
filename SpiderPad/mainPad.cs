using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SpiderPad
{
    public partial class mainPad : Form
    {
        int buffer = 10;

        LocalStorage storage = new LocalStorage();
        List<Layers> layers = new List<Layers>();
        public mainPad()
        {
            InitializeComponent();
            
        }

        private void Pad_Load(object sender, EventArgs e)
        {
            SetWeb();
        }

        private void Pad_Resize(object sender, EventArgs e)
        {

            SetWeb();
        }

        private void SetWeb()
        {
            //Height comes out 40 more than actual form area
            int h = Height - 40;
            h -= buffer * 2;
            Point webP = new Point(buffer, buffer);
            //Point webP = new Point(1,1);
            pbWeb.Width = Convert.ToInt16((Width - buffer * 2) * 0.66);

            pbWeb.Height = Convert.ToInt16(h);
            pbWeb.Location = webP;
        }

        private void tblPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mainPad_Load(object sender, EventArgs e)
        {
            //   Type t = new Type("Nod");
            //   Console.WriteLine(t.Val());
            cbComponent.SelectedIndex = 0;
            FileManager f = new FileManager();
            
            //  string s = f.TestDatabase();
            f.Debug();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            DatabaseHandler handler = new DatabaseHandler();
            storage = handler.Import();
            layers = storage.Layers();

        }

        private void Redraw()
        {
            List<Nodes> nodes = new List<Nodes>();
            nodes = storage.Nodes();
            List<Links> links = new List<Links>();
            links = storage.Links();

            foreach (Layers layer in layers)
            {

            }
            

            foreach (Nodes node in nodes)
            {
                int[] pos = node.GetPos();
                DrawNode(pos[0], pos[1]);
            }

        }

        private void DrawNode(int posx, int posy)
        {
            Pen pen = new Pen(Color.Red);
            Graphics g = pbWeb.CreateGraphics();
            
            g.DrawEllipse(pen, posx, posy, 10, 10);
        }
    }
}
