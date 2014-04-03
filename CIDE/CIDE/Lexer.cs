using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CIDE
{
    class Lexer
    {
        public string source=null;
        char ch;
        public int index=0;
        static char endOfSource=(char)0;
        public Token t = null;

        //public void Append(int i)
        //{
        //    StreamWriter sw = new StreamWriter("F:/tokens.txt", true);
        //    sw.Write(i);
        //    sw.Flush();
        //    sw.Close();
        //}
        
        public bool ReadSource(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            source = sr.ReadToEnd();
            InitBuffer();
            return true;
        }

        public void InitBuffer()
        {
            index = 0;
            getChar();
        }
        public bool getChar()
        {
            if (index < source.Length)
            {
                ch = source.ElementAt(index);
                index++;
                return true;
            }
            else
            {
                ch=endOfSource;
                return false;
            }
        }
        public bool Skipped(char c)
        {
            if(c==' ' || c=='\t'||c=='\r' ||c=='\n')
            {
                return true;
            }
            return false;
        }
        public void SkipWhite()
        {
            while(Skipped(ch))
            {
                getChar();
            }
        }
        public bool IsEndOfSource()
        {
            return ch==endOfSource;
        }
        bool IsDigit()
        {
            if(ch>='0' && ch<='9')
            {
                return true;
            }
            return false;
        }
        bool IsStartOfIdentifier()
        {
            if(ch>='a'&&ch<='z'||ch>='A'&&ch<='Z'||ch=='_')
            {
                return true;
            }
            return false;
        }
        bool IsDigitOrIdentifier()
        {
            return IsDigit()||IsStartOfIdentifier();
        }
        public Token GetToken()
        {
            t=ReadToken();
            return t;
        }
        public Token ReadToken()
        {
            SkipWhite();
            //Append(index);
            //Append((int)ch);
            if (IsEndOfSource())
            {
                return null;
            }
            else if (IsDigit())
            {
                StringBuilder sb = new StringBuilder();
                while (IsDigit())
                {
                    sb.Append(ch);
                    getChar();
                }
                string intstr = sb.ToString();
                Token t = new Token(TokenTag.INT,intstr);
                return t;
            }
            else if (IsStartOfIdentifier())
            {
                //Append(333);
                StringBuilder sb = new StringBuilder();
                while (IsDigitOrIdentifier())
                {
                    sb.Append(ch);
                    getChar();
                }
                string identifierstr = sb.ToString();
                Token t = new Token(TokenTag.IDENTIFIER,identifierstr);
                return t;
            }
            else if (ch == '=')
            {
                getChar();
                if (ch == '=')
                {
                    getChar();
                    return new Token(TokenTag.EQ,"==");
                }
                else
                {
                    return new Token(TokenTag.ASSIGN,"=");
                }
            }
            else if(ch=='!')
            {
                getChar();
                if (ch == '=')
                {
                    getChar();
                    return new Token(TokenTag.NE,"!=");
                }
                else
                {
                    return new Token(TokenTag.NEG,"!");
                }
            }
            else if (ch == '+')
            {
                getChar();
                if (ch == '+')
                {
                    getChar();
                    return new Token(TokenTag.INCR,"++");
                }
                else
                {
                    return new Token(TokenTag.ADD,"+");
                }
            }
            else if (ch == '-')
            {
                getChar();
                if (ch == '-')
                {
                    getChar();
                    return new Token(TokenTag.DECR,"--");
                }
                else
                {
                    return new Token(TokenTag.SUB,"-");
                }
            }
            else if (ch == '>')
            {
                getChar();
                if (ch == '=')
                {
                    getChar();
                    return new Token(TokenTag.GE,">=");
                }
                else if (ch == '>')
                {
                    getChar();
                    return new Token(TokenTag.RSHIFT,">>");
                }
                else
                {
                    return new Token(TokenTag.GT,">");
                }
            }
            else if (ch == '<')
            {
                getChar();
                if (ch == '=')
                {
                    getChar();
                    return new Token(TokenTag.LE,"<=");
                }
                else if (ch == '<')
                {
                    getChar();
                    return new Token(TokenTag.LSHIFT,"<<");
                }
                else
                {
                    return new Token(TokenTag.LT,"<");
                }
            }
            else if(ch=='&')
            {
                getChar();
                if (ch == '&')
                {
                    getChar();
                    return new Token(TokenTag.ANDAND,"&&");
                }
                else
                {
                    return new Token(TokenTag.AND,"&");
                }
            }
            else if (ch == '|')
            {
                getChar();
                if (ch == '|')
                {
                    getChar();
                    return new Token(TokenTag.OROR,"||");
                }
                else
                {
                    return new Token(TokenTag.OR,"|");
                }
            }
            else if (ch == '(')
            {
                getChar();
                return new Token(TokenTag.LP,"(");
            }
            else if (ch == ')')
            {
                getChar();
                return new Token(TokenTag.RP,")");
            }
            else if (ch == '[')
            {
                getChar();
                return new Token(TokenTag.LSB,"[");
            }
            else if (ch == ']')
            {
                getChar();
                return new Token(TokenTag.RSB,"]");
            }
            else if (ch == '{')
            {
                getChar();
                return new Token(TokenTag.LB,"{");
            }
            else if (ch == '}')
            {
                getChar();
                return new Token(TokenTag.RB,"}");
            }
            else if (ch == ';')
            {
                getChar();
                return new Token(TokenTag.SEMI,";");
            }
            else if (ch == endOfSource)
            {
                return null;
            }
            else
            {
                getChar();
                return new Token(TokenTag.UNKNOWN,"unknown");
            }
        }
    }
}
