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
            FileManager f = new FileManager();
            //  string s = f.TestDatabase();
            f.Debug();
        }
    }
}
