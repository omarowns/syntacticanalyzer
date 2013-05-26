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

	public enum NodeType{PROGRAM,DECL,STMT,EXP};
	public enum DeclType{TYPE,LIST,INT,FLOAT,BOOL};
	public enum StmtType{SELECT,ITERATION,REPEAT,READ,WRITE,BLOCK,ASIGN};
	public enum ExpType{RELATION,SUMAOP,MULTOP,CONST,ID};

	public NodeType typeN;
	public DeclType typeD;
	public StmtType typeS;
	public ExpType typeE;
	
	public TreeNode ()
	{
	}
}

