using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLY
{
    class Grammer
    {
        public List<Production> Productions=new List<Production>();
        public Dictionary<string, List<Production>> Terminals = new Dictionary<string, List<Production>>();
        public Dictionary<string, List<Production>> NonTerminals = new Dictionary<string, List<Production>>();
        public Dictionary<string, List<Production>> Prodnames = new Dictionary<string, List<Production>>();
        public Dictionary<string, List<string>> First = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> Follow = new Dictionary<string, List<string>>();

        public void AddProduction(string name,List<string> prodnames)
        {
            Productions.Add(new Production(name,prodnames));
        }

        public void BuildLrItems()
        {
            foreach (Production p in Productions)
            {
                Production lastitem = p;
                for (int i = 0; i <= p.prodnames.Count; i++)
                {
                    Production newitem = new Production(p.name, new List<string>(p.prodnames));
                    newitem.prodnames.Insert(i, ".");
                    lastitem.next = newitem;
                    lastitem = newitem;
                }
            }
        }
        public void ComputeFirst()
        {
        }
        public void ComputeFollow()
        {
        }
    }
}
