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
	private TreeNode rootNode;

	public AnalizadorSintactico (string src)
	{
		this.source = src;
		al = new AnalizadorLexico (this.source);
	}
	
	public void parse(){
		token = al.analizador ();
		rootNode = program ();
	}

	public void print(){
		TreeNode t = rootNode;
		Console.Clear ();
		subprint (t);
	}

	private void subprint(TreeNode t){
		if(t != null) {
			Console.WriteLine ("Node -> {0}", t.type);
			if(t.sibling!=null){
				Console.WriteLine ("Sibling -> {0}", t.sibling);
			}
			Console.WriteLine ("Name -> {0}\tValue -> {1}\tToken -> {2}", t.name,t.value,(t.op.tt==AnalizadorLexico.TipoToken.TK_PALABRA_RESERVADA?"":t.op.ToString()));
			Console.WriteLine ("Left -> {0}\tRight -> {1}\tRightRight -> {2}", t.left,t.right,t.rightright);
			subprint (t.left);
			subprint (t.right);
			subprint (t.rightright);
		}
		return;
	}
	/* **** UTILS START **** */
	private TreeNode createNode(TreeNode.NodeType type){
		TreeNode t = new TreeNode (type);
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
		Console.Write (Environment.NewLine);
	}
	/* **** UTILS END **** */

	private TreeNode program(){
		TreeNode t = null;
		t = createNode (TreeNode.NodeType.PROGRAM);
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
		t = variable_type ();
		while ( (t!=null) && (token.tt == AnalizadorLexico.TipoToken.TK_PUNTOYCOMA) ) {
			if (n == null) {
				n = createNode (TreeNode.NodeType.LIST);
				n.left = t;
				n.op = token;
				t = n;
				match (token.tt);
				t.right = declaration_list ();
			}
		}
		return t;
	}

	private TreeNode variable_type(){
		TreeNode t = null;
		switch (token.tt) {
		case AnalizadorLexico.TipoToken.TK_INT:
			t = createNode (TreeNode.NodeType.INT);
			t.left = createNode (TreeNode.NodeType.INT);
			match (token.tt);
			t.right = variable_list ();
			break;
		case AnalizadorLexico.TipoToken.TK_FLOAT:
			t = createNode (TreeNode.NodeType.FLOAT);
			t.left = createNode (TreeNode.NodeType.FLOAT);
			match (token.tt);
			t.right = variable_list ();
			break;
		case AnalizadorLexico.TipoToken.TK_BOOL:
			t = createNode (TreeNode.NodeType.BOOL);
			t.left = createNode (TreeNode.NodeType.BOOL);
			match (token.tt);
			t.right = variable_list ();
			break;
		}
		return t;
	}

	private TreeNode variable_single(){
		TreeNode t = null;
		if(token.tt==AnalizadorLexico.TipoToken.TK_IDENTIFICADOR){
			t = createNode (TreeNode.NodeType.ID);
			t.name = token.lexema;
			match (AnalizadorLexico.TipoToken.TK_IDENTIFICADOR);
		}
		return t;
	}

	private TreeNode variable_list(){
		TreeNode t,n = null;
		t = variable_single ();
		while (token.tt == AnalizadorLexico.TipoToken.TK_COMA) {
			if (n == null) {
				n = createNode (TreeNode.NodeType.LIST);
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
			t = createNode (TreeNode.NodeType.STMT);
			t.left = n;
			n = sentence_list ();
			if (n != null) {
				t.right = n;
			}
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
		case AnalizadorLexico.TipoToken.TK_IDENTIFICADOR:
			t = asign (); break;
		default:
			error ("Unexpected token -> ");
			break;
		}
		return t;
	}

	private TreeNode selection(){
		TreeNode t = null;
		t = createNode (TreeNode.NodeType.SELECT);
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
		t = createNode (TreeNode.NodeType.ITERATION);
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
		t = createNode (TreeNode.NodeType.REPEAT);
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
		t = createNode (TreeNode.NodeType.READ);
		match (AnalizadorLexico.TipoToken.TK_READ);
		if ((t != null) && (token.tt == AnalizadorLexico.TipoToken.TK_IDENTIFICADOR)) {
			t.name = token.lexema;
		}
		match (AnalizadorLexico.TipoToken.TK_IDENTIFICADOR);
		return t;
	}

	private TreeNode sent_write(){
		TreeNode t = null;
		t = createNode (TreeNode.NodeType.WRITE);
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
		t = createNode (TreeNode.NodeType.BLOCK);
		match (AnalizadorLexico.TipoToken.TK_LLAVEI);
		t.left = sentence_list ();
		match (AnalizadorLexico.TipoToken.TK_LLAVED);
		return t;
	}

	private TreeNode asign(){
		TreeNode t = null;
		t = createNode (TreeNode.NodeType.ASIGN);
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
			n = createNode(TreeNode.NodeType.RELATION);
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
			n = createNode (TreeNode.NodeType.SUMAOP);
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
			n = createNode (TreeNode.NodeType.MULTOP);
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
			t = createNode (TreeNode.NodeType.CONST);
			t.value = int.Parse (token.lexema);
			match (AnalizadorLexico.TipoToken.TK_NUMERO);
			break;
		case AnalizadorLexico.TipoToken.TK_IDENTIFICADOR:
			t = createNode (TreeNode.NodeType.ID);
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

