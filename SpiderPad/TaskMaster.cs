using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderPad
{
    public class TaskMaster
    {
        //overarching class to control behaviors and share data
        ArrayList web = new ArrayList();
        //List<Node> nodes;
        //List<Links> links;
        //List<NLUIDs> nluids;
        //List<Layer> layers;
        //List<UIDs> uids;
        
        public TaskMaster() 
        { 
        }

        public void Run()
        {
            mainPad p = new mainPad();
            Application.Run(p);
        }

        public void Load()
        {

        }
    }
}
