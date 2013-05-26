using System;
/*
		== GRAMATICA == 
programa → program { lista-declaración lista-sentencias }
lista-declaración → declaración ; lista-declaración | vació
declaración → tipo lista-variables
tipo → int | float | bool
lista-variables → identificador, lista-variables | identificador
lista-sentencias → sentencia lista-sentencias | sentencia | vació
sentencia → selección | iteración | repetición | sent-read | 
sent-write | bloque | asignación
selección → if ( expresión ) bloque fi| 
if ( expresión ) bloque else bloque fi
iteración → while ( expresión ) bloque
repetición → do bloque until ( expresión ) ;
sent-read → read identificador ;
sent-write → write expresión ;
bloque → { lista-sentencia }
asignación → identificador = expresión ; 
expresión → expresión-simple relación expresión-simple | 
expresión-simple
relacion → <= | < | > | >= | == | !=
expresión-simple → expresión-simple suma-op termino | termino
suma-op → + | -
termino → termino mult-op factor | factor
mult-op → * | /
factor → ( expresión ) | numero | identificador
 */

public class AnalizadorSintactico
{
	private string source = "";
	private AnalizadorLexico al;
	public Token token;

	public AnalizadorSintactico (string src)
	{
		this.source = src;
		al = new AnalizadorLexico (this.source);
	}
	
	public TreeNode parse(){
		TreeNode t = null;
		token = al.analizador ();
		t = program ();
		return t;
	}

	/* **** UTILS START **** */
	private TreeNode createProgramNode(TreeNode.NodeType type){
		TreeNode t = new TreeNode ();
		t.value	= 0;
		t.left	= null;
		t.right	= null;
		t.typeN = type;
		return t;
	}
	private TreeNode createDeclNode(TreeNode.DeclType type){
		TreeNode t = new TreeNode ();
		t.value	= 0;
		t.left	= null;
		t.right	= null;
		t.typeD = type;
		return t;
	}
	private TreeNode createStmtNode(TreeNode.StmtType type){
		TreeNode t = new TreeNode ();
		t.value	= 0;
		t.left	= null;
		t.right	= null;
		t.typeS = type;
		return t;
	}
	private TreeNode createExprNode(TreeNode.ExpType type){
		TreeNode t = new TreeNode ();
		t.value	= 0;
		t.left	= null;
		t.right	= null;
		t.typeE = type;
		return t;
	}
	private void match(AnalizadorLexico.TipoToken expected){
		if (token.tt == expected) {
			token = al.analizador ();
		} else {
			error ();
		}
	}
	private void error(){
		Console.WriteLine ("Syntax error");
	}
	private void error(string s){
		Console.Write (s);
		Console.Write (token.ToString());
	}
	/* **** UTILS END **** */

	private TreeNode program(){
		TreeNode t = null;
		t = createProgramNode (TreeNode.NodeType.PROGRAM);
		match (AnalizadorLexico.TipoToken.TK_PROGRAM);
		match (AnalizadorLexico.TipoToken.TK_LLAVEI);
		if (t != null) {
			t.left = declaration_list ();
			t.right = sentence_list ();
		}
		match (AnalizadorLexico.TipoToken.TK_LLAVED);
		if (token.tt == AnalizadorLexico.TipoToken.TK_PROGRAM) {
			t.sibling = program ();
		}
		return t;
	}

	private TreeNode declaration_list(){
		TreeNode t, n = null;
		t = variable_single ();
		while ( (t!=null) && (token.tt == AnalizadorLexico.TipoToken.TK_PUNTOYCOMA) ) {
			if (n != null) {
				n.left = t;
				n.op = token;
				t = n;
				match (token.tt);
				t.right = declaration_list ();
			}
		}
		return t;
	}

	private TreeNode variable_single(){
		TreeNode t = null;
		switch (token.tt) {
		case AnalizadorLexico.TipoToken.TK_INT:
			t.left = createDeclNode (TreeNode.DeclType.INT);
			match (token.tt);
			t.right = variable_list ();
			break;
		case AnalizadorLexico.TipoToken.TK_FLOAT:
			t.left = createDeclNode (TreeNode.DeclType.FLOAT);
			match (token.tt);
			t.right = variable_list ();
			break;
		case AnalizadorLexico.TipoToken.TK_BOOL:
			t.left = createDeclNode (TreeNode.DeclType.BOOL);
			match (token.tt);
			t.right = variable_list ();
			break;
		}
		return t;
	}

	private TreeNode variable_list(){
		TreeNode t,n = null;
		t = variable_single ();
		while (token.tt == AnalizadorLexico.TipoToken.TK_COMA) {
			if (n != null) {
				n.left = t;
				n.op = token;
				t = n;
				match (token.tt);
				t.right = variable_list ();
			}
		}
		return t;
	}

	private TreeNode sentence_list(){
		TreeNode t,n = null;
		t = null;
		n = sentence ();
		if (n != null) {
			t = createProgramNode (TreeNode.NodeType.STMT);
			t.left = n;
		}
		n = sentence_list ();
		if (n != null) {
			t = createProgramNode (TreeNode.NodeType.STMT);
			t.right = n;
		}
		return t;
	}

	private TreeNode sentence(){
		TreeNode t = null;
		switch (token.tt) {
		case AnalizadorLexico.TipoToken.TK_IF:
			t = selection (); break;
		case AnalizadorLexico.TipoToken.TK_WHILE:
			t = iteration (); break;
		case AnalizadorLexico.TipoToken.TK_DO:
			t = repeat (); break;
		case AnalizadorLexico.TipoToken.TK_READ:
			t = sent_read (); break;
		case AnalizadorLexico.TipoToken.TK_WRITE:
			t = sent_write (); break;
		case AnalizadorLexico.TipoToken.TK_ASIGNACION:
			t = asign (); break;
		default:
			error ("Unexpected token -> ");
			break;
		}
		return t;
	}

	private TreeNode selection(){
		TreeNode t = null;
		t = createStmtNode (TreeNode.StmtType.SELECT);
		match (AnalizadorLexico.TipoToken.TK_IF);
		match (AnalizadorLexico.TipoToken.TK_PARI);
		if (t != null)
			t.left = expresion ();
		match (AnalizadorLexico.TipoToken.TK_PARD);
		if (t != null)
			t.right = block ();
		if (token.tt == AnalizadorLexico.TipoToken.TK_ELSE) {
			match (AnalizadorLexico.TipoToken.TK_ELSE);
			t.rightright = block ();
		}
		match (AnalizadorLexico.TipoToken.TK_FI);
		return t;
	}

	private TreeNode iteration(){
		TreeNode t = null;
		t = createStmtNode (TreeNode.StmtType.ITERATION);
		match (AnalizadorLexico.TipoToken.TK_WHILE);
		match (AnalizadorLexico.TipoToken.TK_PARI);
		if(t!=null)
			t.left = expresion ();
		match (AnalizadorLexico.TipoToken.TK_PARD);
		if(t!=null)
			t.right = block ();
		return t;
	}

	private TreeNode repeat(){
		TreeNode t = null;
		t = createStmtNode (TreeNode.StmtType.REPEAT);
		match (AnalizadorLexico.TipoToken.TK_DO);
		if (t != null)
			t.left = block ();
		match (AnalizadorLexico.TipoToken.TK_UNTIL);
		match (AnalizadorLexico.TipoToken.TK_PARI);
		if (t != null)
			t.right = expresion ();
		match (AnalizadorLexico.TipoToken.TK_PARD);
		match (AnalizadorLexico.TipoToken.TK_PUNTOYCOMA);
		return t;
	}

	private TreeNode sent_read(){
		TreeNode t = null;
		t = createStmtNode (TreeNode.StmtType.READ);
		match (AnalizadorLexico.TipoToken.TK_READ);
		if ((t != null) && (token.tt == AnalizadorLexico.TipoToken.TK_IDENTIFICADOR)) {
			t.name = token.lexema;
		}
		match (AnalizadorLexico.TipoToken.TK_IDENTIFICADOR);
		return t;
	}

	private TreeNode sent_write(){
		TreeNode t = null;
		t = createStmtNode (TreeNode.StmtType.WRITE);
		match (AnalizadorLexico.TipoToken.TK_WRITE);
		if ((t != null)) {
			//TODO Check if right or left
			t.left = expresion ();
		}
		match (AnalizadorLexico.TipoToken.TK_PUNTOYCOMA);
		return t;
	}

	private TreeNode block(){
		TreeNode t = null;
		t = createStmtNode (TreeNode.StmtType.BLOCK);
		match (AnalizadorLexico.TipoToken.TK_LLAVEI);
		t.left = sentence_list ();
		match (AnalizadorLexico.TipoToken.TK_LLAVED);
		return t;
	}

	private TreeNode asign(){
		TreeNode t = null;
		t = createStmtNode (TreeNode.StmtType.ASIGN);
		if ((t != null) && (token.tt == AnalizadorLexico.TipoToken.TK_IDENTIFICADOR)) {
			t.name = token.lexema;
		}
		match (AnalizadorLexico.TipoToken.TK_IDENTIFICADOR);
		match (AnalizadorLexico.TipoToken.TK_ASIGNACION);
		if (t != null) {
			//TODO Check if its right or left
			t.left = expresion ();
		}
		match (AnalizadorLexico.TipoToken.TK_PUNTOYCOMA);
		return t;
	}

	private TreeNode expresion(){
		TreeNode t,n = null;
		t = expresion_simple ();
		while
		(	(token.tt==AnalizadorLexico.TipoToken.TK_MENORIGUAL) ||
			(token.tt==AnalizadorLexico.TipoToken.TK_MENOR) ||
			(token.tt==AnalizadorLexico.TipoToken.TK_MAYOR) ||
			(token.tt==AnalizadorLexico.TipoToken.TK_MAYORIGUAL) ||
			(token.tt==AnalizadorLexico.TipoToken.TK_CIGUALDAD) ||
			(token.tt==AnalizadorLexico.TipoToken.TK_CDIFERENTE) ){
			n = createExprNode(TreeNode.ExpType.RELATION);
			if(n!=null){
				n.left = t;
				n.op = token;
				t = n;
				match (token.tt);
				t.right = expresion_simple();
			}
		}

		return t;
	}

	private TreeNode expresion_simple(){
		TreeNode t, n = null;
		t = termino ();
		while( (token.tt==AnalizadorLexico.TipoToken.TK_SUMA) || (token.tt==AnalizadorLexico.TipoToken.TK_RESTA) ){
			n = createExprNode (TreeNode.ExpType.SUMAOP);
			if(n!=null){
				n.left = t;
				n.op = token;
				t = n;
				match (token.tt);
				t.right = expresion_simple ();
			}
		}
		return t;
	}

	private TreeNode termino(){
		TreeNode t,n = null;
		t = factor ();
		while ((token.tt==AnalizadorLexico.TipoToken.TK_MULTIPLICACION) || (token.tt==AnalizadorLexico.TipoToken.TK_DIVISION)) {
			n = createExprNode (TreeNode.ExpType.MULTOP);
			if(n!= null){
				n.left = t;
				n.op = token;
				t = n;
				match (token.tt);
				n.right = termino ();
			}
		}
		return t;
	}


	private TreeNode factor(){
		TreeNode t = null;
		switch (token.tt) {
		case AnalizadorLexico.TipoToken.TK_PARI:
			match (AnalizadorLexico.TipoToken.TK_PARI);
			t = expresion ();
			match (AnalizadorLexico.TipoToken.TK_PARD);
			break;
		case AnalizadorLexico.TipoToken.TK_NUMERO:
			t = createExprNode (TreeNode.ExpType.CONST);
			t.value = int.Parse (token.lexema);
			match (AnalizadorLexico.TipoToken.TK_NUMERO);
			break;
		case AnalizadorLexico.TipoToken.TK_IDENTIFICADOR:
			t = createExprNode (TreeNode.ExpType.ID);
			t.name = token.lexema;
			match (AnalizadorLexico.TipoToken.TK_IDENTIFICADOR);
			break;
		default:
			error ("unexpected token -> ");
			break;
		}
		return t;
	}
}

