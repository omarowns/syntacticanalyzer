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

	public enum NodeType{PROGRAM,STMT,EXP};
	public enum StmtType{SELECT,ITERATION,REPEAT,READ,WRITE,BLOCK,ASIGN};
	public enum ExpType{RELATION,SUMAOP,MULTOP,CONST,ID};

	public NodeType typeN;
	public StmtType typeS;
	public ExpType typeE;
	
	public TreeNode ()
	{
	}
}

