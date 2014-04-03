using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{
    enum NodeTag
    {
        JUMP,
        LABEL,
        LABELV,
    }
    class Node
    {
        public TreeTag op;
        public Node left;
        public Node right;
        public List<Node> kids = new List<Node>();
        public Symbol[] syms = new Symbol[3];
        public static List<Node> DagList = new List<Node>();
        public Node(TreeTag op, Node left, Node right, Symbol sym)
        {
            this.op = op;
            this.left = left;
            this.right = right;
            this.syms[0] = sym;
            DagList.Add(this);
            //Dag dag = new Dag();
            //dag.node = this;
            //dag.Dagnode(op, left, right, sym);
        }
        public static Node InstallNode(TreeTag op,Node left,Node right,Symbol sym)
        {
            foreach( Node n in DagList)
            {
                if(n.op==op&&n.left==left&&n.right==right&&n.syms[0]==sym)
                {
                    return n;
                }
            }
            Node node=new Node(op,left,right,sym);
            return node;
        }
        public static Node CreateNewNode(TreeTag op, Node left, Node right, Symbol sym)
        {
            Node node = new Node(op, left, right, sym);
            return node;
        }
    }
}
