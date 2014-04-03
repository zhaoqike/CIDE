using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{
    public enum TokenTag
    {
        CHAR, SHORT, INT, LONG,
        FLOAT, DOUBLE, STRING,

        ICON,FCON,SCON,

        IDENTIFIER,

        ASSIGN,

        NEG,

        LSHIFT,RSHIFT,

        ANDAND,OROR,AND,OR,

        ADD,SUB,INCR,DECR,

        LP,RP,LSB,RSB,LB,RB,

        SEMI,COMMA,POINTER,QUESTION,COLON, ARRAY,FUNCTION,

        EQ,NE,
        GT,GE, LT,LE,

        SIGNED, UNSIGNED,

        STRUCT, ENUM, UNION,

        STATIC,CONST,VOLITALE,CONVOL,

        IF,ELSE,FOR,DO,WHILE,SWITCH,CASE,DEFAULT,BREAK,CONTINUE,GOTO,RETURN,

        UNKNOWN,
    }
    public struct Union
    {
        public char charvalue;
        public short shortvalue;
        public int intvalue;
        public long longvalue;
        public float floatvalue;
        public double doublevalue;
        public string stringvalue;
    }
    public class Token
    {
        public TokenTag tag;
        public string name;
        public Union union;
        public Symbol tsym;
        
        //public static Token IntToken(int value)
        //{
        //    Token t = new Token(TokenTag.INT);
        //    t.union.intvalue = value;
        //    return t;
        //}
        //public static Token IdentifierToken(string name)
        //{
        //    Token t = new Token(TokenTag.IDENTIFIER,name);
        //    t.name = name;
        //    return t;
        //}
        public Token(TokenTag tag,string name)
        {
            this.tag = tag;
            this.name = name;
        }
    }
}
