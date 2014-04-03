using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{
    public class TypeElement
    {
        public int op;
        public Type nexttype;
        public int align;
        public int size;
        public Symbol sym;
    }
    public class Type
    {
        public TokenTag op;
        public Level level;
        public Type nexttype;
        public int align;
        public int size;
        public Symbol sym;

        public static List<Type> TypeTable=new List<Type>();

        public static Type chartype;
        public static Type uchartype;
        public static Type shorttype;
        public static Type ushorttype;
        public static Type inttype;
        public static Type uinttype;
        public static Type longtype;
        public static Type ulongtype;
        public static Type floattype;
        public static Type doubletype;
        public static Type longdoubletype;
        public Type(TokenTag op, Type type, int size, int align,Symbol sym)
        {

            this.op = op;
            this.nexttype = type;
            this.align = align;
            this.size = size;
            this.sym=sym;
            TypeTable.Add(this);
        }
        public Type InstallBasicType(TokenTag op,string name,int size,int align) 
        {
            Symbol p=TableManager.types.Install(name,Level.GLOBAL);
            Type type=new Type(op,null,align,size,p);
            return type;
        }
        public static void TypeInit()
        {
            chartype.InstallBasicType(0, "char", 1, 4);
            uchartype.InstallBasicType(0, "unsigned char", 1, 4);
            shorttype.InstallBasicType(0, "short", 2, 4);
            ushorttype.InstallBasicType(0, "unsigned short", 2, 4);
            inttype.InstallBasicType(0, "int", 4, 4);
            uinttype.InstallBasicType(0, "unsigned int", 4, 4);
            floattype.InstallBasicType(0, "float", 4, 4);
            doubletype.InstallBasicType(0, "double", 4, 4);
            longdoubletype.InstallBasicType(0, "long double", 4, 4);
        }
        public static bool IsPtr(Type type)
        {
            return;
        }
        public static Type Ptr(Type type)
        {
            return new Type(TokenTag.POINTER, type, 4, 4, Symbol.pointersym);
        }
        public static Type AtoP(Type type)
        {
        }
        public static bool IsArray(Type type)
        {
            return type.op == TokenTag.ARRAY;
        }
        public static bool IsQual(Type type)
        {
            return type.op==TokenTag.CONST||type.op==TokenTag.VOLITALE||type.op==TokenTag.CONVOL;
        }
        public static Type UnQual(Type type)
        {
            if (IsQual(type))
            {
                return type.nexttype;
            }
            return type;
        }
        public static Type Qual(Type type, TokenTag op)
        {
            if (IsArray(type))
            {
                return new Type(TokenTag.ARRAY, Qual(type.nexttype, op), type.size, type.align, type.sym);
            }
            if (IsConst(type) && op == TokenTag.CONST ||
                IsVolatile(type) && op == TokenTag.VOLITALE)
            {
                MainForm.Error("qual error");
                return type;
            }
            else
            {
                if (IsConst(type) && op == TokenTag.VOLITALE ||
                    IsVolatile(type) && op == TokenTag.CONST)
                {
                    op = TokenTag.CONVOL;
                    type = type.nexttype;
                }
                type = new Type(op,type,type.size,type.align,type.sym);
            }
            return type;
        }
        public static bool IsConst(Type type)
        {
            return type.op==TokenTag.CONST||type.op==TokenTag.CONVOL;
        }
        public static bool IsVolatile(Type type)
        {
            return type.op==TokenTag.VOLITALE||type.op==TokenTag.CONVOL;
        }
        public static bool IsStruct(Type type)
        {
            return Type.UnQual(type).op == TokenTag.STRUCT || Type.UnQual(type).op == TokenTag.UNION;
        }
    }
}
