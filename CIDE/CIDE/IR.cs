using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{
    public enum SegmentTag
    {
        CODE,
        DATA
    }
    class IR
    {
        List<Node> linearizedList = new List<Node>();
        public void function(Symbol func,List<Symbol> caller,List<Symbol> callee,int n)
        {
            gencode(caller, callee);
            emitcode();
        }

        private void emit()
        {
            throw new NotImplementedException();
        }

        private void gen(List<Node> forest)
        {
            Node sentinel = new Node();
            List<Node> dummy = new List<Node>();
            foreach(Node node in forest)
            {
                if(Generic(node.op)==TreeTag.CALL)
                {
                    DoCall(node);
                }
                else if (Generic(node.op) == TreeTag.ASGN && Generic(node.right.op) == TreeTag.CALL)
                {
                    DoCall(node.right);
                }
                else if (Generic(node.op) == TreeTag.ARG)
                {
                    DoArg(node);
                }
                rewrite(node);
            }
            foreach(Node node in forest)
            {
                prune(node,dummy);
            }
            foreach(Node node in forest)
            {
                linearize(node);
            }
        }

        private void prune(Node node, List<Node> dummy)
        {
            //throw new NotImplementedException();
            if (node == null)
            {
                return;
            }

        }

        private void linearize(Node node)
        {
            linearize(node.left);
            linearize(node.right);
            linearizedList.Add(node);
            //throw new NotImplementedException();
        }

        private void rewrite(Node node)
        {
            prelabel(node);
            reduce(node);
            //throw new NotImplementedException();
        }

        private void prelabel(Node node)
        {
            if (node == null)
            {
                return;
            } 
            prelabel(node.left);
            prelabel(node.right);
            //throw new NotImplementedException();
        }

        private void reduce(Node node)
        {
            throw new NotImplementedException();
        }
        private TreeTag Generic(TreeTag treeTag)
        {
 	        throw new NotImplementedException();
        }
    } 
}
