using System;

public class TreeNode
{
	public int value;
	public Token op;
	public string name;
	public TreeNode sibling;
	public TreeNode left;
	public TreeNode right;
	public TreeNode rightright;

	public enum NodeType
	{NONE,
	PROGRAM,
	DECLARATION_LIST,
		DECL,
		TYPE,
			INT,
			FLOAT,
			BOOL,
		VARIABLES_LIST,
	STATEMENT_LIST,
		STATEMENT,
			SELECT,
				IF,
				FI,
			ITERATION,
				WHILE,
			REPEAT,
				DO,
				UNTIL,
			READ,
			WRITE,
			BLOCK,
			ASIGN,
			RELATION,
				LET,LT,GT,GET,REL,DIF,
			SIMPLE_EXP,
				SUMAOP,
					PLUS,
					MINUS,
				MULTOP,
					TIMES,
					DIV,
				CONST,ID,EXP,
	COMMA, SEMI_COLON}
	;

	public NodeType type;

	public TreeNode (NodeType type)
	{
		this.type = type;
		this.op = new Token ();
		switch (type) {
		case NodeType.NONE: this.name = ""; break;
		case NodeType.PROGRAM: this.name = "PROGRAM"; break;
		case NodeType.DECLARATION_LIST: this.name = "DECLARATION_LIST"; break;
		case NodeType.DECL: this.name = "DECLARATION"; break;
		case NodeType.TYPE: this.name = "TYPE"; break;
		case NodeType.INT: this.name = "INT"; break;
		case NodeType.FLOAT: this.name = "FLOAT"; break;
		case NodeType.BOOL: this.name = "BOOL"; break;
		case NodeType.VARIABLES_LIST: this.name = "VARIABLES_LIST"; break;
		case NodeType.STATEMENT_LIST: this.name = "STATEMENT_LIST"; break;
		case NodeType.STATEMENT: this.name = "STATEMENT"; break;
		case NodeType.SELECT: this.name = "SELECT"; break;
		case NodeType.IF: this.name = "IF"; break;
		case NodeType.FI: this.name = "FI"; break;
		case NodeType.ITERATION: this.name = "ITERATION"; break;
		case NodeType.REPEAT: this.name = "REPEAT"; break;
		case NodeType.DO: this.name = "DO"; break;
		case NodeType.UNTIL: this.name = "UNTIL"; break;
		case NodeType.READ: this.name = "READ"; break;
		case NodeType.WRITE: this.name = "WRITE"; break;
		case NodeType.BLOCK: this.name = "BLOCK"; break;
		case NodeType.ASIGN: this.name = "ASIGN"; break;
		case NodeType.RELATION: this.name = "RELATION"; break;
		case NodeType.LET: this.name = "LESS EQUAL THAN"; break;
		case NodeType.GET: this.name = "GREATER EQUAL THAN"; break;
		case NodeType.LT: this.name = "LESS THAN"; break;
		case NodeType.GT: this.name = "GREATER THAN"; break;
		case NodeType.REL: this.name = "REL"; break;
		case NodeType.DIF: this.name = "DIFERENT"; break;
		case NodeType.SIMPLE_EXP: this.name = "SIMPLE EXPRESSION"; break;
		case NodeType.SUMAOP: this.name = "SUMA_OP"; break;
		case NodeType.PLUS: this.name = "+"; break;
		case NodeType.MINUS: this.name = "-"; break;
		case NodeType.MULTOP: this.name = "MULT_OP"; break;
		case NodeType.TIMES: this.name = "*"; break;
		case NodeType.DIV: this.name = "/"; break;
		case NodeType.CONST: this.name = "CONST"; break;
		case NodeType.ID: this.name = "ID"; break;
		case NodeType.EXP: this.name = "EXPRESSION"; break;
		case NodeType.COMMA: this.name = ","; break;
		case NodeType.SEMI_COLON: this.name = ";"; break;
		}
	}

	public override string ToString ()
	{
		return string.Format ("[TreeNode].{0}",this.name);
	}
}

