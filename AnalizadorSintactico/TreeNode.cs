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

	public enum Type{NONE,PROGRAM,DECL,STMT,EXP,TYPE,LIST,INT,FLOAT,BOOL,SELECT,ITERATION,REPEAT,READ,WRITE,BLOCK,ASIGN,RELATION,SUMAOP,MULTOP,CONST,ID};
	public enum NodeType{NONE,PROGRAM,DECL,STMT,EXP};
	public enum DeclType{NONE,TYPE,LIST,INT,FLOAT,BOOL};
	public enum StmtType{NONE,SELECT,ITERATION,REPEAT,READ,WRITE,BLOCK,ASIGN};
	public enum ExpType{NONE,RELATION,SUMAOP,MULTOP,CONST,ID};

	public NodeType typeN;
	public DeclType typeD;
	public StmtType typeS;
	public ExpType typeE;
	
	public TreeNode ()
	{
	}
}

