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

	/* **** UTILS START **** */
	private TreeNode createNode(string s){
		TreeNode t = new TreeNode ();
		t.value	= 0;
		t.name	= s;
		t.left	= null;
		t.right	= null;
		return t;
	}
	private void equal(Token tk){
		if (token.lexema.Equals (tk.lexema)) {
			token = al.analizador ();
		} else {
			error ();
		}
	}
	private void error(){
		Console.WriteLine ("Syntax error");
		Console.ReadKey ();
	}
	/* **** UTILS END **** */

	private TreeNode program(){
		TreeNode temp,nuevo;
		//temp = listadeclaracion ();
		while( (token.tt==AnalizadorLexico.TipoToken.TK_PROGRAM) || (token.tt==AnalizadorLexico.TipoToken.TK_LLAVEI) || (token.tt==AnalizadorLexico.TipoToken.TK_LLAVED) ){
			switch (token.tt) {
			case AnalizadorLexico.TipoToken.TK_PROGRAM:
				equal (token);
				nuevo = createNode ("program");
				nuevo.left = temp;
				//nuevo.right = listadeclaracion ();
				temp = nuevo;
				break;
			case AnalizadorLexico.TipoToken.TK_LLAVEI: break;
			case AnalizadorLexico.TipoToken.TK_LLAVED: break;
			}
		}
		return null;
	}

	public TreeNode parse(){
		TreeNode t = null;
		token = al.analizador ();
		t = program ();
		return t;
	}
}

