using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderPad
{
    public class LocalStorage
    {
        public LocalStorage()
        {

        }
        protected List<Nodes> nodes = new List<Nodes>();
        protected List<Links> links = new List<Links>();
        protected List<Layers> layers = new List<Layers>();
        public List<Nodes> Nodes()
        {
            
            return nodes;
        }

        public void AddNode(Nodes n)
        {
            nodes.Add(n);
        }

        public List<Links> Links()
        {
            
            return links;
        }

        public void AddLink(Links l)
        {
            links.Add(l);
        }

        public List<Layers> Layers()
        {
            return layers;
        }

        public void AddLayers(Layers l)
        {
            layers.Add(l);
        }
    }
}
