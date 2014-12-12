using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Quique_2048
{
    class Game
    {

        static void Main()
        {

            do { MainGame(Menu()); }
            while (PlayAgain());

        }


        static void MainGame(int option)
        {
            ScoreTracker.Score = 0;
            int[,] a = new int[option, option];

            ReDraw(a, 0); 

            while (true)
            {
                int mov = 0;

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {

                    case ConsoleKey.LeftArrow:
                        mov = 1;
                        a = Merge(a);
                        break;

                    case ConsoleKey.DownArrow:
                        mov = 2;
                        a = RotateGrid(a, 1);
                        a = Merge(a);
                        a = RotateGrid(a, -1);
                        break;

                    case ConsoleKey.RightArrow:
                        mov = 3;
                        a = RotateGrid(a, 2);
                        a = Merge(a);
                        a = RotateGrid(a, -2);
                        break;

                    case ConsoleKey.UpArrow:
                        mov = 4;
                        a = RotateGrid(a, 3);
                        a = Merge(a);
                        a = RotateGrid(a, -3);
                        break;

                    default:
                        mov = 5;
                        break;

                }

                Point cp = RandomPoint(a);  // Aqui usamos el objeto tipo point para, usando randompoint() y añadirle el 2 al vector (este 2 seria un nº magico? el juego empieza siempre por 2)
                if (cp != null && mov<5) //osea, si queda al menos una posición
                {
                    a[cp.X, cp.Y] = 2;
                    ReDraw(a, mov);
                }

                if (cp == null && !CanMove(a))  //si no queda ninguna posicion disponible, y además no quedan mas movimientos, con CanMove
                {
                    ReDraw(a, "Game Over"); //usamos redibujar con una sobrecarga con el Game Over
                    break;
                }

                if (Wins(a))
                {
                    ReDraw(a, "Has Ganado!");
                    break;
                }
                
            }

        }

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

                switch (key.Key)
                {

                    case ConsoleKey.D1:
                        return 6;

                    case ConsoleKey.D2:
                        return 4;

                    case ConsoleKey.D3:
                        Environment.Exit(0);
                        return 0;
                }
            }
            while (true);
        }
        public static bool PlayAgain()
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

        public static Point RandomPoint(int[,] a)  //Para insertar el nº en una posicion aleatoria, creamos una lista con las posiciones "libres", donde con 2 For generamos todas las posiciones
        {
            List<Point> listRandom = new List<Point>();
            for (int i = 0; i < a.GetLength(0); i++) //for para la primera dimension
            {
                for (int j = 0; j < a.GetLength(1); j++) //for para la segunda dimension
                {
                    if (a[i, j] == 0) //vemos si en el vector la posicion esta libre para meterlo en la lista
                    {
                        listRandom.Add(new Point(i, j)); //vamos añadiendo a la lista las posiciones del vector libres
                    }
                }
            }

            if (listRandom.Count == 0) //si no meto esto, al no quedar posiciones disponibles, el programa da una excepcion :C
            {
                return null;
            }

            int rnd = new Random().Next(listRandom.Count); //insertamos el nº en una posicion aleatoria de la lista de posibles
            return listRandom[rnd];
        }

        public static int[,] Merge(int[,] a)
        {

            for (int i = 0; i < a.GetLength(0); i++)
            {
                int lastNum = 0;
                int last_j = 0;
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (lastNum != a[i, j] && a[i, j] != 0)
                    {
                        lastNum = a[i, j];
                        last_j = j;
                    }
                    else if (lastNum == a[i, j])
                    {
                        a[i, last_j] = 0;
                        a[i, j] = lastNum + a[i, j];
                        ScoreTracker.IncreaseScore(a[i, j]);
                    }
                }

                //empuja los numeros por el grid
                last_j = 0;
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] != 0)
                    {
                        a[i, last_j] = a[i, j];
                        if (last_j != j)
                            a[i, j] = 0;
                        last_j++;
                    }
                }
            }
            return a;
        }

        public static int[,] RotateGrid(int[,] a, int rotNum)
        {
            while (rotNum < 0)
            {
                rotNum += 4;
            }
            for (int rot_i = 0; rot_i < rotNum; rot_i++)
            {
                int[,] b = new int[a.GetLength(1), a.GetLength(0)];

                for (int i = 0; i < a.GetLength(0); i++)
                {
                    for (int j = 0; j < a.GetLength(1); j++)
                    {
                        b[j, a.GetLength(0) - i - 1] = a[i, j];
                    }
                }

                a = b;
            }
            return a;
        }

        public static bool CanMove(int[,] a)
        {
            bool res = false;

            int[,] b = ACopy(a);
            b = Merge(b);
            if (!Equals(a, b))
                res = true;

            b = ACopy(a);
            b = RotateGrid(b, 1);
            b = Merge(b);
            b = RotateGrid(b, -1);
            if (!Equals(a, b))
                res = true;

            b = ACopy(a);
            b = RotateGrid(b, 2);
            b = Merge(b);
            b = RotateGrid(b, -2);
            if (!Equals(a, b))
                res = true;

            b = ACopy(a);
            b = RotateGrid(b, 3);
            b = Merge(b);
            b = RotateGrid(b, -3);
            if (!Equals(a, b))
                res = true;


            return res;
        }

        public static bool Equals(int[,] a, int[,] b)
        {
            bool res = true;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (b[i, j] != a[i, j])
                    {
                        res = false;
                        break;
                    }
                }
                if (!res)
                    break;
            }
            return res;
        }

        public static bool Wins(int[,] a)
        {
            bool res = false;
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 2048)
                    {
                        res = true;
                    }
                }
            }
            return res;
        }

        public static int[,] ACopy(int[,] a)
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    b[i, j] = a[i, j];
                }
            }

            return b;
        }

        public static void ReDraw(int[,] a, int mov) // el repintado, que incluye el dibujo del grid donde iran los numeros
        {
            Console.Clear();
            for (int j = 0; j < a.GetLength(1); j++)
            {
                if (j == 0) Console.Write("┌────┬");
                else if (j == a.GetLength(1)-1) Console.Write("────┐");
                else Console.Write("────┬");
            }
            Console.Write("\n");
            for (int i = 0; i < a.GetLength(0); i++)
            {
                Console.Write("│");
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    string s = ""; //Aqui dependiendo del tamaño del nº en caracteres, le metemos espacios antes y despues para que se queden centrados. Para ver como quedan hay que cambiar el 2 en la linea 56
                    if (a[i, j] == 0)
                        s = "    ";
                    else if (a[i, j] < 10)
                        s = "  " + a[i, j] + " ";
                    else if (a[i, j] < 100)
                        s = " " + a[i, j] + " ";
                    else if (a[i, j] < 1000)
                        s = a[i, j] + " ";
                    else
                        s = a[i, j]+"";
                    Console.Write(s + "│");
                }
                Console.Write("\n");
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (i == a.GetLength(0)-1) 
                    {
                        if (j == 0) Console.Write("└────┴");
                        else if (j == a.GetLength(1) - 1) Console.Write("────┘");
                        else Console.Write("────┴");
                    }
                    else 
                    {
                        if (j == 0) Console.Write("├────┼");
                        else if (j == a.GetLength(1) - 1) Console.Write("────┤");
                        else Console.Write("────┼");
                    }
                }
                Console.Write("\n");
            }



            //switch (mov) //esto se queda temporalmente para hacer debug cuando resuelva como sumar los elementos :P
            //{
                
            //    case 1:
            //        Console.WriteLine("Izquierda");
            //        break;
            //    case 2:
            //        Console.WriteLine("Abajo");
            //        break;
            //    case 3:
            //        Console.WriteLine("Derecha");
            //        break;
            //    case 4:
            //        Console.WriteLine("Arriba");
            //        break;
            //}

            Console.WriteLine("Puntuación: "+ScoreTracker.Score);


        }

        public static void ReDraw(int[,] a, string s)
        {

                Console.Clear();
                ReDraw(a,0);
                Console.WriteLine("\n\n\t" + s + "\n\n");

        }

    }

    public class ScoreTracker
    {
        public static int Score;

        public static void IncreaseScore(int Amount)
        {
            Score += Amount;
        }

    }

    class Point
    {
        public Point(int x, int y) 
        {
            this.X = x;
            this.Y = y;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }
    }
}
