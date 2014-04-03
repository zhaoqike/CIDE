﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIDE
{

	

    public enum TreeTag
    {
        F=1,
	    D=2,
	    C=3,
	    S=4,
	    I=5,
	    U=6,
	    P=7,
	    V=8,
	    B=9,
        CNST=1<<4,
       	CNSTC=CNST+C,
       	CNSTD=CNST+D,
       	CNSTF=CNST+F,
       	CNSTI=CNST+I,
       	CNSTP=CNST+P,
       	CNSTS=CNST+S,
       	CNSTU=CNST+U,
       ARG=2<<4,
       	ARGB=ARG+B,
       	ARGD=ARG+D,
       	ARGF=ARG+F,
       	ARGI=ARG+I,
       	ARGP=ARG+P,
       ASGN=3<<4,
       	ASGNB=ASGN+B,
       	ASGNC=ASGN+C,
       	ASGND=ASGN+D,
       	ASGNF=ASGN+F,
       	ASGNI=ASGN+I,
       	ASGNS=ASGN+S,
       	ASGNP=ASGN+P,
       INDIR=4<<4,
       	INDIRB=INDIR+B,
       	INDIRC=INDIR+C,
       	INDIRD=INDIR+D,
       	INDIRF=INDIR+F,
       	INDIRI=INDIR+I,
       	INDIRS=INDIR+S,
       	INDIRP=INDIR+P,
       CVC=5<<4,
       	CVCI=CVC+I,
       	CVCU=CVC+U,
       CVD=6<<4,
       	CVDF=CVD+F,
       	CVDI=CVD+I,
       CVF=7<<4,
       	CVFD=CVF+D,
       CVI=8<<4,
       	CVIC=CVI+C,
       	CVID=CVI+D,
       	CVIS=CVI+S,
       	CVIU=CVI+U,
       CVP=9<<4,
       	CVPU=CVP+U,
       CVS=10<<4,
       	CVSI=CVS+I,
       	CVSU=CVS+U,
       CVU=11<<4,
       	CVUC=CVU+C,
       	CVUI=CVU+I,
       	CVUP=CVU+P,
       	CVUS=CVU+S,
       NEG=12<<4,
       	NEGD=NEG+D,
       	NEGF=NEG+F,
       	NEGI=NEG+I,
       CALL=13<<4,
       	CALLB=CALL+B,
       	CALLD=CALL+D,
       	CALLF=CALL+F,
       	CALLI=CALL+I,
       	CALLV=CALL+V,
       LOAD=14<<4,
       	LOADB=LOAD+B,
       	LOADC=LOAD+C,
       	LOADD=LOAD+D,
       	LOADF=LOAD+F,
       	LOADI=LOAD+I,
       	LOADP=LOAD+P,
       	LOADS=LOAD+S,
       	LOADU=LOAD+U,
       RET=15<<4,
       	RETD=RET+D,
       	RETF=RET+F,
       	RETI=RET+I,
       ADDRG=16<<4,
       	ADDRGP=ADDRG+P,
       ADDRF=17<<4,
       	ADDRFP=ADDRF+P,
       ADDRL=18<<4,
       	ADDRLP=ADDRL+P,
       ADD=19<<4,
       	ADDD=ADD+D,
       	ADDF=ADD+F,
       	ADDI=ADD+I,
       	ADDP=ADD+P,
       	ADDU=ADD+U,
       SUB=20<<4,
       	SUBD=SUB+D,
       	SUBF=SUB+F,
       	SUBI=SUB+I,
       	SUBP=SUB+P,
       	SUBU=SUB+U,
       LSH=21<<4,
       	LSHI=LSH+I,
       	LSHU=LSH+U,
       MOD=22<<4,
       	MODI=MOD+I,
       	MODU=MOD+U,
       RSH=23<<4,
       	RSHI=RSH+I,
       	RSHU=RSH+U,
       BAND=24<<4,
       	BANDU=BAND+U,
       BCOM=25<<4,
       	BCOMU=BCOM+U,
       BOR=26<<4,
       	BORU=BOR+U,
       BXOR=27<<4,
       	BXORU=BXOR+U,
       DIV=28<<4,
       	DIVD=DIV+D,
       	DIVF=DIV+F,
       	DIVI=DIV+I,
       	DIVU=DIV+U,
       MUL=29<<4,
       	MULD=MUL+D,
       	MULF=MUL+F,
       	MULI=MUL+I,
       	MULU=MUL+U,
       EQ=30<<4,
       	EQD=EQ+D,
       	EQF=EQ+F,
       	EQI=EQ+I,
       GE=31<<4,
       	GED=GE+D,
       	GEF=GE+F,
       	GEI=GE+I,
       	GEU=GE+U,
       GT=32<<4,
       	GTD=GT+D,
       	GTF=GT+F,
       	GTI=GT+I,
       	GTU=GT+U,
       LE=33<<4,
       	LED=LE+D,
       	LEF=LE+F,
       	LEI=LE+I,
       	LEU=LE+U,
       LT=34<<4,
       	LTD=LT+D,
       	LTF=LT+F,
       	LTI=LT+I,
       	LTU=LT+U,
       NE=35<<4,
       	NED=NE+D,
       	NEF=NE+F,
       	NEI=NE+I,
       JUMP=36<<4,
       	JUMPV=JUMP+V,
       LABEL=37<<4,
       	LABELV=LABEL+V,
        AND=38<<4,
        NOT=39<<4,
        OR=40<<4,
        COND=41<<4,
        RIGHT=42<<4,
    }
    class Tree
    {
        public TreeTag op;
        public Type type;
        public Tree left;
        public Tree right;
        public Node node;
        public Tree(TreeTag op, Type type, Tree left, Tree right)
        {
            this.op = op;
            this.type = type;
            this.left = left;
            this.right = right;
        }
    }
}
