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
	{		NONE
,		PROGRAM
,		DECL
,		STMT
,		EXP
,		TYPE
,		LIST
,		INT
,		FLOAT
,		BOOL
,		SELECT
,		ITERATION
,		REPEAT
,		READ
,		WRITE
,		BLOCK
,		ASIGN
,		RELATION
,		SUMAOP
,		MULTOP
,		CONST
,		ID
}
	;

	public NodeType type;

	public TreeNode (NodeType type)
	{
		this.type = type;
		this.op = new Token ();
		switch (type) {
		case NodeType.NONE:
			this.name = "";
			break;
		case NodeType.PROGRAM:
			this.name = "PROGRAM";
			break;
		case NodeType.DECL:
			this.name = "DECLARATION";
			break;
		case NodeType.STMT:
			this.name = "STATEMENT";
			break;
		case NodeType.EXP:
			this.name = "EXPRESION";
			break;
		case NodeType.TYPE:
			this.name = "TYPE";
			break;
		case NodeType.LIST:
			this.name = "TYPE-LIST";
			break;
		case NodeType.INT:
			this.name = "INT";
			break;
		case NodeType.FLOAT:
			this.name = "FLOAT";
			break;
		case NodeType.BOOL:
			this.name = "BOOL";
			break;
		case NodeType.SELECT:
			this.name = "SELECT";
			break;
		case NodeType.ITERATION:
			this.name = "ITERATION";
			break;
		case NodeType.REPEAT:
			this.name = "REPEAT";
			break;
		case NodeType.READ:
			this.name = "READ";
			break;
		case NodeType.WRITE:
			this.name = "WRITE";
			break;
		case NodeType.BLOCK:
			this.name = "BLOCK";
			break;
		case NodeType.ASIGN:
			this.name = "ASIGN";
			break;
		case NodeType.RELATION:
			this.name = "RELATION";
			break;
		case NodeType.SUMAOP:
			this.name = "SUMAOP";
			break;
		case NodeType.MULTOP:
			this.name = "MULTOP";
			break;
		case NodeType.CONST:
			this.name = "CONST";
			break;
		case NodeType.ID:
			this.name = "ID";
			break;
		}
	}
}

