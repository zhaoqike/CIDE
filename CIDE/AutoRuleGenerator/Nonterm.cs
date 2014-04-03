using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRuleGenerator
{
    class Nonterm
    {
        Kind kind = Kind.NONTERM;
        string name = "";
        List<Rule> rules = new List<Rule>();
    }
}
