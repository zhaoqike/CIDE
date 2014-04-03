using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRuleGenerator
{
    public enum Kind
    {
        TERM,
        NONTERM
    }
    class Term
    {
        Kind kind=Kind.TERM;
        string name="";
        List<Rule> rules = new List<Rule>();
    }
}
