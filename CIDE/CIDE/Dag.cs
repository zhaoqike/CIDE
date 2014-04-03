using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{
    class Dag
    {
        public Node node;
        public Node Dagnode(TreeTag op,Node left,Node right,Symbol sym)
        {
            node.op=op;
            node.left=left;
            node.right=right;
            node.syms[0]=sym;
        }
    }
}
