using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{
    enum CodeKind
    {
        BLOCKBEG,BLOCKEND,LOCAL,ADDRESS,DEFPOINT,LABEL,
        START,GEN,JUMP,SWITCH
    }
    class Code
    {
        public CodeKind kind;
        public static List<Code> CodeList = new List<Code>();
        public Code(CodeKind kind)
        {
            this.kind = kind;
            CodeList.Add(this);
        }
    }
}
