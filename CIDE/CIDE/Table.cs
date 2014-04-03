using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{
    enum Level
    {
        CONSTANTS, LABEL, GLOBAL, PARAM, LOCAL
    }
    class Table
    {
        public Level level;
        public List<Symbol> symbols=new List<Symbol>();
        public Table(Level level)
        {
            this.level = level;
        }
    }
    class MultiTable
    {
        public LinkedList<Table> tableList=new LinkedList<Table>();
        public Symbol Install(string name, Level level)
        {
            Symbol p = new Symbol(name, level);
            Table last = tableList.Last();
            if (level > last.level)
            {
                tableList.AddFirst(new Table(level));
            }
            last = tableList.Last();
            last.symbols.Add(p);
            return p;
        }
        public Symbol LookUp(string name)
        {
            foreach(Table tbl in tableList)
            {
                //Table tbl = tableList.ElementAt(i);
                foreach (Symbol p in tbl.symbols)
                {
                    if (p.name == name)
                    {
                        return p;
                    }
                }
            }
            return null;
        }
    }

}
