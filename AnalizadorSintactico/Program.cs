using System;

class MainClass
{
	public static void Main (string[] args)
	{
		AnalizadorSintactico syntax = new AnalizadorSintactico(args[0]);
		syntax.parse ();
		syntax.print ();
	}
}
