using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace CIDE
{
    public partial class MainForm : Form
    {
        Lexer lexer = new Lexer();
        Level level=Level.GLOBAL;
        static List<string> warnings=new List<string>();
        static List<string> errors=new List<string>();
        int label=1;
        Token token;

        //List<Code> CodeList = new List<Code>();
        
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Type.TypeInit();
            lexer.GetToken();
            //lexer.ReadSource("F:/test.txt");
            //sourceTextBox.Text = lexer.source;

            //Token t=null;
            //List<Token> list=new List<Token>();
            //while ((t = lexer.getToken()) != null)
            //{
            //    list.Add(t);
            //}
            
            //StreamWriter sw = new StreamWriter("F:/tokens.txt",true);
            //sw.Write("count : "+list.Count.ToString());
            //sw.Write(System.Environment.NewLine);
            //foreach (Token tt in list)
            //{
            //    sw.Write((int)tt.tag);

            //    sw.Write("  : "+tt.name);
            //    sw.Write(System.Environment.NewLine);
            //}
            //sw.Flush();
            //sw.Close();
        }

        private void sourceTextBox_TextChanged(object sender, EventArgs e)
        {

        }
        void decl(Level lev)
        {
            StoreClass sclass;
            Type ty,ty1;
            ty=specifier(out sclass);
            if(lexer.t.tag==TokenTag.IDENTIFIER ||lexer.t.tag==TokenTag.POINTER||lexer.t.tag==TokenTag.LP||lexer.t.tag==TokenTag.LSB)
            {
                string id;
                Coordinate pos;
                if(lev==Level.GLOBAL)
                {
                    List<Symbol> parameters=new List<Symbol>();

                    ty1=dclr(ty,out id,parameters,0);
                    if(IsFunction(ty))
                    {
                        functionDef(sclass,id,ty,parameters,pos);
                    }
                }
                else
                {
                    ty1=dclr(ty);
                }
                for(;;)
                {
                    if(lev==Level.GLOBAL)
                    {
                        dclglobal(sclass,id,ty,pos);
                    }
                    if(lexer.t.tag!=TokenTag.COMMA)
                    {
                        break;
                    }
                    ty1=dclr(ty);
                }
            }
            else
            {
                Error("not complete");
            }

        }

        private void functionDef(StoreClass sclass,string id,Type ty,List<Symbol> parameters,Coordinate pos)
        {
            Symbol p=TableManager.identifiers.LookUp(id);
            if(p!=null &&p.type.IsFunction())
            {
                Error("redefinition of function "+id);
            }
            dclglobal(StoreClass.GLOBAL,id,ty,pos);
            Compound(0,null,0);
 	        throw new NotImplementedException();
        }
        void Compound(int loop, Switch swth,int lev)
        {
            Code code=new Code(CodeKind.BLOCKBEG);
            Expect(TokenTag.LB);
            while(kind(lexer.t)==TokenTag.IF ||kind(lexer.t)==TokenTag.IDENTIFIER||
                kind(lexer.t)==TokenTag.CHAR||kind(lexer.t)==TokenTag.STATIC)
            {
                if(kind(lexer.t)==TokenTag.CHAR||kind(lexer.t)==TokenTag.STATIC)
                {
                    decl(Level.LOCAL);
                }
                else
                {
                    statement(loop,swth,lev);
                }
            }
        }
        int genLabel(int n)
        {
            label+=n;
            return label-n;
        }

        private void statement(int loop,Switch swp,int lev)
        {
            switch(lexer.t.tag)
            {
                case TokenTag.IF:
                    ifstmt(genLabel(2),loop,swp,lev);
                    break;
                case TokenTag.FOR:
                    forstmt(genLabel(4),loop,swp,lev);
                    break;
                case TokenTag.DO:
                    dostmt(genLabel(3),loop,swp,lev);
                    break;
                case TokenTag.WHILE:
                    whilestmt(genLabel(3),swp,lev);
                    break;
                case TokenTag.SWITCH:
                    swstmt(loop, genLabel(2), lev);
                    break;
                case TokenTag.BREAK:
                    breakstmt(loop, swp, lev);
                    break;
                case TokenTag.CONTINUE:
                    continuestmt(loop, swp, lev);
                    break;
                case TokenTag.CASE:
                    break;
                case TokenTag.DEFAULT:
                    break;
                case TokenTag.LB:
                    Compound(loop, swp, lev + 1);
                    break;
                case TokenTag.SEMI:
                    lexer.GetToken();
                    break;
                case TokenTag.GOTO:
                    gotostmt(); 
                    break;
                case TokenTag.RETURN:
                    returnstmt();
                    break;
                case TokenTag.IDENTIFIER:
                    if (PeekToken().tag == TokenTag.COLON)
                    {
                        stmtlabel();
                        statement(loop, swp, lev);
                    }
                    else
                    {
                        Tree e = Expr(TokenTag.UNKNOWN);
                        listnodes(e, 0, 0);
                        Expect(TokenTag.SEMI);
                    }
                    break;
                default:
                    break;
            }

 	        //throw new NotImplementedException();
        }

        private void continuestmt(int loop, Switch swp, int lev)
        {
            Walk(null, 0, 0);
            if (loop > 0)
            {
                branch(loop + 2);
            }
            else
            {
                Error("illegal continue statement");
            }
            GetToken();
            Expect(TokenTag.SEMI);
            throw new NotImplementedException();
        }

        private void breakstmt(int loop, Switch swp, int lev)
        {
            Walk(null, 0, 0);
            if (swp != null && swp.lab > loop)
            {
                branch(swp.lab + 1);
            }
            else if (loop > 0)
            {
                branch(loop + 2);
            }
            else
            {
                Error("illegal break statement");
            }
            GetToken();
            Expect(TokenTag.SEMI);
            //throw new NotImplementedException();
        }

        private void gotostmt()
        {
            Walk(null, 0, 0);
            GetToken();
            if (token.tag == TokenTag.IDENTIFIER)
            {
                Symbol p = TableManager.labels.LookUp(token.name);
                if (p == null)
                {
                    p = TableManager.labels.Install(token.name, Level.GLOBAL);
                }
                p.label = genLabel(1);
                branch(p.label);
                GetToken();
            }
            else
            {
                Error("expecting an identifier after goto");
            }
            Expect(TokenTag.SEMI);
            //throw new NotImplementedException();
        }

        private void stmtlabel(string name)
        {
            Symbol p = TableManager.labels.LookUp(name);
            if (p == null)
            {
                p = TableManager.labels.Install(name, Level.GLOBAL);
                p.label = genLabel(1);
            }
            if (p.defined)
            {
                Error("redefinition of label " + name);
            }
            p.defined = true;
            GetToken();
            Expect(TokenTag.COLON);
            //throw new NotImplementedException();
        }

        public void GetToken()
        {
            token = lexer.GetToken();
        }
        private void swstmt(int loop, int lab, int lev)
        {
            Tree e;
            Switch sw=new Switch();
            Code head, tail;
            lexer.GetToken();
            Expect(TokenTag.LP);
            e = Expr(TokenTag.RP);
            if (!isint(e.type))
            {
                Error("wrong type in switch");
                e = retype(e, Type.inttype);
            }
            sw.lab = lab;
            statement(loop, sw, lev);
            if (sw.deflab == null)
            {
                sw.deflab = findlabel(lab);
                definelab(lab);
            }
            swgen(sw);
            branch(lab);
            //throw new NotImplementedException();
        }

        private Symbol findlabel(int lab)
        {
            Table tol = TableManager.labels.tableList.ElementAt(0);
            foreach (Symbol sym in tol.symbols)
            {
                if (sym.label == lab)
                {
                    return sym;
                }
            }
            tol.symbols.Add(new Symbol(lab.ToString(), Level.GLOBAL));
            throw new NotImplementedException();
        }

        private void whilestmt(int lab,Switch swp,int lev)
        {
            lexer.GetToken();
            Expect(TokenTag.LP);
            Tree e = Texpr(Conditional(TokenTag.RP));
            definelab(lab);
            statement(lab, swp, lev);
            definelab(lab + 1);
            Walk(e, lab, 0);
            definelab(lab + 2);
 	        //throw new NotImplementedException();
        }

        private void dostmt(int lab,int loop,Switch swp,int lev)
        {
            lexer.GetToken();
            definelab(lab);
            statement(lab, swp, lev);
            definelab(lab + 1);
            Expect(TokenTag.LP);
            Walk(Conditional(TokenTag.RP),lab,0);
            definelab(lab + 2);
 	        //throw new NotImplementedException();
        }

        private void forstmt(int lab,int loop,Switch swp,int lev)
        {
            Tree e1 = null, e2 = null, e3 = null;
            lexer.GetToken();
            Expect(TokenTag.LP);
            if (lexer.t.Kind() == TokenTag.IDENTIFIER)
            {
                e1 = Texpr(expr0());
            }
            else
            {
                Expect(TokenTag.SEMI);
            }
            Walk(e1, 0, 0);
            if (lexer.t.Kind() == TokenTag.IDENTIFIER)
            {
                e2 = Texpr(Conditional, TokenTag.SEMI);
            }
            else
            {
                Expect(TokenTag.SEMI);
            }
            if (lexer.t.Kind() == TokenTag.IDENTIFIER)
            {
                e3 = Texpr(expr0(), TokenTag.RP);
            }
            else
            {
                Expect(TokenTag.RP);
            }
            branch(lab + 3);
            definelab(lab);
            statement(lab, swp, lev);
            definelab(lab + 1);
            if (e3 != null)
            {
                Walk(e3, 0, 0);
            }
            definelab(lab + 3);
            if (e2 != null)
            {
                Walk(e2, lab, 0);
            }
            else
            {
                branch(lab);
            }
 	        //throw new NotImplementedException();
        }

        private void ifstmt(int lab,int loop,Switch swp,int lev)
        {
            lexer.GetToken();
            Expect(TokenTag.LP);
            Walk(Conditional(TokenTag.RP),0,lab);
            statement(loop,swp,lev);
            if (lexer.t.tag == TokenTag.ELSE)
            {
                branch(lab + 1);
                definelab(lab);
                lexer.GetToken();
                statement(loop, swp, lev);
                definelab(lab + 1);
            }
            else
            {
                definelab(lab);
            }
 	        //throw new NotImplementedException();
        }

        private void definelab(int lab)
        {
            Symbol p = findlabel(lab);
            Code code = new Code(CodeKind.LABEL);
            code.forest = newnode(NodeTag.LABELV, null, null, p);
            //throw new NotImplementedException();
        }

        private void branch(int lab)
        {
            Code code;
            Symbol p = findlabel(lab);
            new Code(CodeKind.JUMP).forest = newnode(NodeTag.JUMP, null, null, p);
            throw new NotImplementedException();
        }

        private void Walk(Tree p,int tlab,int flab)
        {
 	        throw new NotImplementedException();
        }

        private Tree Conditional(TokenTag tag)
        {
            Tree p=Expr(tag);
            return Cond(p);
 	        throw new NotImplementedException();
        }

        private Tree Cond(Tree p)
        {
 	        throw new NotImplementedException();
        }

        private Tree Expr(TokenTag tag)
        {
            Tree p,q;
            p=Expr1(TokenTag.UNKNOWN);
            while(lexer.t.tag==TokenTag.COMMA)
            {
                q=Pointer(Expr1(TokenTag.UNKNOWN));
                p=new Tree(TreeTag.RIGHT,q.type,p,q);
            }
            if(tag!=TokenTag.UNKNOWN)
            {
                Test(tag);
            }
 	        //throw new NotImplementedException();
            return p;
        }

        private void Test(TokenTag tag)
        {
            if (lexer.t.tag == tag)
            {
                lexer.GetToken();
            }
            else
            {
                Expect(tag);
                SkipTo(tag, set);
                if (lexer.t.tag == tag)
                {
                    lexer.GetToken();
                }
            }
            //throw new NotImplementedException();
        }

        private Tree Expr1(TokenTag tag)
        {
 	        //throw new NotImplementedException();
            Tree p,q;
            p=Expr2(TokenTag.UNKNOWN);
            if(lexer.t.tag==TokenTag.EQ)
            {
                q=Expr1(TokenTag.UNKNOWN);
                p=AsgnTree(p,q);
            }
            return p;
        }

        private Tree Expr2(TokenTag tag)
        {
 	        //throw new NotImplementedException();
            Tree p,l,r;
            p=Expr3(4);
            if(lexer.t.tag==TokenTag.QUESTION)
            {
                l=Expr(TokenTag.UNKNOWN);
                Expect(TokenTag.COLON);
                r=Expr(TokenTag.UNKNOWN);
                p=CondTree(p,l,r);
            }
            return p;
        }

        private Tree Expr3(int k)
        {
 	        throw new NotImplementedException();
        }
        Tree unary()
        {
        }
        Tree postfix(Tree p)
        {
        }
        Tree primary()
        {
            Tree p=null;
            switch(lexer.t.tag)
            {
                case TokenTag.ICON:
                case TokenTag.FCON:
                case TokenTag.SCON:
                    p=new Tree(TreeTag.CNST+ttob(lexer.t.tsym.type),lexer.t.tsym.type,null,null);
                    break;
                case TokenTag.IDENTIFIER:
                    p = idtree(lexer.t.tsym);
                    break;
                default:
                    Error("illegal expression");
                    p=consttree(0,Type.inttype);
                    break;
            }
            if(p==null)
            {
                Error("error in primary");
            }
            return p;
        }

        private void Expect(TokenTag tokenTag)
        {
            if(lexer.t.tag==tokenTag)
            {
                lexer.GetToken();
            }
            else
            {
                Error("expect error");
            }
 	        //throw new NotImplementedException();
        }

        public static void Error(string err)
        {
            errors.Add(err);
 	        //throw new NotImplementedException();
        }
        Node listnodes(Tree t,int tlab,int flab)
        {
            Node p,l,r;
            if(t==null)
            {
                return null;
            }
            if(t.node!=null)
            {
                return t.node;
            }
            switch(t.op)
            {
                case TreeTag.AND:
                    if(flab!=0)
                    {
                        listnodes(t.left,0,flab);
                        listnodes(t.right,0,flab);
                    }
                    else
                    {
                        int overlabel=genLabel(1);
                        listnodes(t.left,0,overlabel);
                        listnodes(t.right,tlab,0);
                        labelnode(overlabel);
                    }
                    break;
                case TreeTag.OR:
                    if(tlab!=0)
                    {
                        listnodes(t.left,tlab,0);
                        listnodes(t.right,tlab,0);
                    }
                    else
                    {
                        int overlabel=genLabel(1);
                        listnodes(t.left,overlabel,0);
                        listnodes(t.right,0,flab);
                        labelnode(overlabel);
                    }
                    break;
                case TreeTag.NOT:
                    return listnodes(t.left,flab,tlab);
                    break;
                case TreeTag.BOR:
                case TreeTag.BAND:
                case TreeTag.BXOR:
                case TreeTag.ADD:
                case TreeTag.SUB:
                case TreeTag.LSH:
                case TreeTag.RSH:
                    Debug.Assert(tlab==0&&flab==0,"error in listnode BOR");
                    l=listnodes(t.left,0,0);
                    r=listnodes(t.right,0,0);
                    p = Node.InstallNode(t.op, l, r, null);
                    break;
                case TreeTag.MUL:
                case TreeTag.DIV:
                case TreeTag.MOD:
                    Debug.Assert(tlab == 0 && flab == 0, "error in listnode MUL");
                    l = listnodes(t.left, 0, 0);
                    r = listnodes(t.right, 0, 0);
                    p = Node.InstallNode(t.op, l, r, null);
                    break;
            }
        }


        private Type dclr(Type ty,out string id,List<Symbol> parameters,int isAbstract)
        {
 	        throw new NotImplementedException();
        }

        private Type specifier(out StoreClass sclass)
        {
 	        sclass=StoreClass.AUTO;
            TokenTag cls= TokenTag.UNKNOWN;
            TokenTag vol =TokenTag.UNKNOWN;
            TokenTag cons = TokenTag.UNKNOWN;
            TokenTag sign = TokenTag.UNKNOWN;
            TokenTag size = TokenTag.UNKNOWN;
            TokenTag type = TokenTag.UNKNOWN;
            //List<TokenTag> spe=new List<TokenTag>();
            //for(int i=0;i<6;i++)
            //{
            //    spe.Add(TokenTag.UNKNOWN);
            //}
            Type ty=null;
            for(;;)
            {
                bool flag=true;
                switch(lexer.t.tag)
                {
                    case TokenTag.CHAR: 
                    case TokenTag.INT:
                    case TokenTag.FLOAT:
                    case TokenTag.DOUBLE:
                        type=lexer.t.tag;
                        lexer.GetToken();
                        break;
                    case TokenTag.SHORT:
                    case TokenTag.LONG:
                        size=lexer.t.tag;
                        lexer.GetToken();
                        break;
                    case TokenTag.SIGNED:
                    case TokenTag.UNSIGNED:
                        sign=lexer.t.tag;
                        lexer.GetToken();
                        break;
                    case TokenTag.STRUCT:
                    case TokenTag.UNION:
                        ty=StructDcl(lexer.t.tag);
                        break;
                    case TokenTag.ENUM:
                        ty=EnumDcl();
                        break;
                    case TokenTag.IDENTIFIER:
                        flag=false;
                        break;
                    default:
                        flag=false;
                        break;
                }
                if(flag==false)
                {
                    break;
                }
            }
            if(type==TokenTag.CHAR)
            {
                if(sign==TokenTag.UNSIGNED)
                {
                    ty=Type.uchartype;
                }
                else
                {
                    ty=Type.chartype;
                }
            }
            if(size==TokenTag.SHORT)
            {
                if(sign==TokenTag.UNSIGNED)
                {
                    ty=Type.ushorttype;
                }
                else
                {
                    ty=Type.shorttype;
                }
            }
            if(size==TokenTag.LONG)
            {
                if(type==TokenTag.DOUBLE)
                {
                    ty=Type.longdoubletype;
                }
                else if(sign==TokenTag.UNSIGNED)
                {
                    ty=Type.ulongtype;
                }
                else
                {
                    ty=Type.longtype;
                }
            }

        }
        void dclglobal(StoreClass sclass, string id,Type ty,Coordinate src)
        {
            Symbol p=TableManager.identifiers.LookUp(id);
            if(p!=null&&p.scope==Level.GLOBAL)
            {
                Error("redefinition of symbol "+p.name);
                return;
            }
            if(p==null||p.scope!=Level.GLOBAL)
            {
                p=TableManager.identifiers.Install(id,Level.GLOBAL);
                p.sclass=sclass;
            }
            else if(p.sclass==StoreClass.EXTERN)
            {
                p.sclass=sclass;
            }
        }

        TokenTag kind(Token t)
        {
        }
        void Warning(string warning)
        {
            warnings.Add(warning);
        }
        void program()
        {
            level = Level.GLOBAL;
            while(lexer.t!=null)
            {
                if (kind(lexer.t) == TokenTag.CHAR || kind(lexer.t) == TokenTag.STATIC || lexer.t.tag == TokenTag.IDENTIFIER)
                {
                    decl(Level.GLOBAL);
                }
                else if (lexer.t.tag == TokenTag.SEMI)
                {
                    Warning("empty declaration");
                    lexer.GetToken();
                }
                else
                {
                    Error("unrecognized declaration");
                    lexer.GetToken();
                }
            }
            if(lexer.index==0)
            {
                Warning("empty file");
            }

        }

        void EnterScope()
        {
            level++;
        }
        void ExitScope()
        {
            rmTypes(level);
            if (TableManager.types.tableList.First.Value.level == level)
            {
                TableManager.types.tableList.RemoveFirst();
            }
            if (TableManager.identifiers.tableList.First.Value.level == level)
            {
                TableManager.identifiers.tableList.RemoveFirst();
            }
            --level;
        }

        private void rmTypes(Level level)
        {
            Type.TypeTable.RemoveAll(delegate(Type t)
            {
                return t.level>=level;
            });
            //throw new NotImplementedException();
        }



        public void function(Symbol func,List<Symbol> caller,List<Symbol> callee,int n)
        {
            gencode(caller, callee);
            emitcode();
        }

        private void emitcode()
        {
            throw new NotImplementedException();
        }

        private void gencode(List<Symbol> caller, List<Symbol> callee)
        {
            //throw new NotImplementedException();
            for (int i = 0; i < caller.Count; i++)
            {
                Symbol p = caller.ElementAt(i);
                Symbol q = callee.ElementAt(i);
                Walk(asgn(p, idtree(q)), 0, 0);//need  repair
            }
            SwitchToSegment(SegmentTag.CODE);
            foreach (Code code in Code.CodeList)
            {
                switch (code.kind)
                {
                    case CodeKind.ADDRESS:
                        break;
                    case CodeKind.BLOCKBEG:
                        break;
                    case CodeKind.BLOCKEND:
                        break;
                    case CodeKind.DEFPOINT:
                        break;
                    case CodeKind.GEN:
                        fixup(code.forest);
                        IR.gen(code.forest);
                        break;
                    case CodeKind.LOCAL:
                        break;
                    case CodeKind.START:
                        break;
                    case CodeKind.SWITCH:
                        break;
                    default:
                        MainForm.Error("unrecognized code kind in gencode");
                        break;
                }
            }
        }

        private void emitcode(List<Symbol> caller, List<Symbol> callee)
        {
            //throw new NotImplementedException();
            for (int i = 0; i < caller.Count; i++)
            {
                Symbol p = caller.ElementAt(i);
                Symbol q = callee.ElementAt(i);
                Walk(asgn(p, idtree(q)), 0, 0);//need  repair
            }
            //SwitchToSegment(SegmentTag.CODE);
            foreach (Code code in Code.CodeList)
            {
                switch (code.kind)
                {
                    case CodeKind.ADDRESS:
                        break;
                    case CodeKind.BLOCKBEG:
                        break;
                    case CodeKind.BLOCKEND:
                        break;
                    case CodeKind.DEFPOINT:
                        break;
                    case CodeKind.GEN:
                        fixup(code.forest);
                        IR.gen(code.forest);
                        break;
                    case CodeKind.LOCAL:
                        break;
                    case CodeKind.START:
                        break;
                    case CodeKind.SWITCH:
                        break;
                    default:
                        MainForm.Error("unrecognized code kind in gencode");
                        break;
                }
            }
        }
    }
}
