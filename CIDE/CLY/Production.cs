using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLY
{
    class Production
    {
        public string name=null;
        public List<string> prodnames=new List<string>();
        public Production next=null;
        public Production(string name, List<string> prodnames)
        {
            this.name = name;
            this.prodnames = prodnames;
        }
        public void CreateLrItems()
        {
            Production lastitem = this;
            for (int i = 0; i <= prodnames.Count; i++)
            {
                Production newitem = new Production(this.name, new List<string>(this.prodnames));
                newitem.prodnames.Insert(i, ".");
                lastitem.next = newitem;
                lastitem = newitem;
            }
        }
    }
}
