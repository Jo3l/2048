using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Game2048
{
    class GameInterface
    {

        public static int Menu()
        {
            do
            {
                Console.Clear();
                Console.WriteLine("Versión para terminal del juego 2048");
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Menu Principal");
                Console.WriteLine("=============");
                Console.WriteLine();
                Console.WriteLine("Por favor, seleccione dificultad:");
                Console.WriteLine("1. Fácil");
                Console.WriteLine("2. Dificil");
                Console.WriteLine("3. Salir");

                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key) //solo permitimos 3 opciones a pulsar
                {

                    case ConsoleKey.D1: //facil
                        return 6; //hete aqui el numero del tamaño del vector que devolvemos, 6 es muy facil, casi estúpido

                    case ConsoleKey.D2:
                        return 4; //aqui devolvemos el tamaño que realmente hace que el juego sea dificil pero no imposible, dificultad ideal

                    case ConsoleKey.D3:
                        Environment.Exit(0); //este apartado del menu, cierra la aplicación
                        return 0; //si no pongo esto, error de compilador :P
                }
            }
            while (true);
        }

        public static bool PlayAgain() //metodo simple que solo pide si quieres volver a jugar
        {

            Console.WriteLine("¿Otra Partida? (s/n)");


            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {

                    case ConsoleKey.S:
                        return true;

                    case ConsoleKey.N:
                        return false;
                }
            }
            
        }

        public static void ReDraw(int[,] a) // el repintado, que incluye el dibujo del grid donde iran los numeros
        {
            Console.Clear(); //este método es el que pinta el grid con sus valores
            for (int j = 0; j < a.GetLength(1); j++) //ooooootro for, este es el de la primera linea que pinta el grid
            {
                if (j == 0) Console.Write("┌────┬"); //si es la primera columna
                else if (j == a.GetLength(1) - 1) Console.Write("────┐"); //columna final
                else Console.Write("────┬"); //las de enmedio
            }
            Console.WriteLine();//nueva linea
            //Aqui se pinta el meollo del juego
            for (int i = 0; i < a.GetLength(0); i++)
            {
                Console.Write("│");
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    string s = ""; //Aqui dependiendo del tamaño del nº, en caracteres, le metemos espacios antes y despues para que se queden centrados. Para ver como quedan hay que cambiar el 2 en la linea 56
                    if (a[i, j] == 0)
                        s = "    ";
                    else if (a[i, j] < 10) //si el nº es menor de 10, es decir, 1 caracter
                        s = "  " + a[i, j] + " ";
                    else if (a[i, j] < 100) // si es menor de 100, 2 caracteres
                        s = " " + a[i, j] + " ";
                    else if (a[i, j] < 1000) //menor de 1000, 3 caracteres
                        s = a[i, j] + " ";
                    else
                        s = a[i, j] + ""; //el resto, 4 caracteres, mas de 2048 no se juega
                    Console.Write(s + "│"); //pinta e separador
                }
                Console.WriteLine();//nueva linea
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (i == a.GetLength(0) - 1) //el pintado de la linea final
                    {
                        if (j == 0) Console.Write("└────┴");//primera columna
                        else if (j == a.GetLength(1) - 1) Console.Write("────┘");//Última columna
                        else Console.Write("────┴");//las de enmedio
                    }
                    else //el pintado de las lineas separadoras de enmedio, las que no sean las ultimas
                    {
                        if (j == 0) Console.Write("├────┼");//primera columna
                        else if (j == a.GetLength(1) - 1) Console.Write("────┤");//Última columna
                        else Console.Write("────┼");//las de enmedio
                    }
                }
                Console.WriteLine();//nueva linea
            }

            Console.WriteLine("Puntuación: " + ScoreTracker.Score); //Escribimos la puntuación


        }

        public static void ReDraw(int[,] a, string s) //el método adminte la sobrecarga de un string para escribir si has ganado o perdido
        {

            ReDraw(a); //invocamos a el redibujado original
            Console.WriteLine("\n\n\t" + s + "\n\n"); //despues de dibujar, añadimos el string

        }

    }

}
