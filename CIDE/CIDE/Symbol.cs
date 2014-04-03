using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{
    enum StoreClass
    {
        AUTO,
        GLOBAL,
        EXTERN,
        STATIC,
        LOCAL
    }
    class Symbol
    {
        public string name;
        public Level scope;
        public StoreClass sclass;
        public Type type;
        public float reference;
        public Coordinate src;
        public int label;
        public bool defined = false;
        public static Symbol pointersym = new Symbol("*", Level.GLOBAL);
        public Symbol(string name, Level scope)
        {
            this.name = name;
            this.scope = scope;
        }
    }
}
