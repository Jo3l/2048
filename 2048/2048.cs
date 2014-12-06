﻿using System;
using System.Collections.Generic;
using System.Text;

     //."".    ."",
     //|  |   /  /
     //|  |  /  /
     //|  | /  /
     //|  |/  ;-._ 
     //}  ` _/  / ;
     //|  /` ) /  /
     //| /  /_/\_/\
     //|/  /      |
     //(  ' \ '-  |
     // \    `.  /
     //  |      |
     //  |      | me guardo esto en caso de ganar :D

namespace Quique_2048
{
    class Game
    {
        static void Main()
        {
            int[,] a = new int[6, 6]; //si, ya se, evitar numeros magicos, cuando lo acabe, pedira una dificultad, cuanto mas grande, mas facil

            ReDraw(a, 0); 

            while (true)
            {
                int mov = 0;

                ConsoleKeyInfo key = Console.ReadKey();
                switch (key.Key)
                {

                    case ConsoleKey.LeftArrow:
                        mov = 3;
                        a = Merge(a);
                        break;

                    case ConsoleKey.DownArrow:
                        mov = 2;
                        a = RotateGrid(a, 1);
                        a = Merge(a);
                        a = RotateGrid(a, -1);
                        break;

                    case ConsoleKey.RightArrow:
                        a = RotateGrid(a, 2);
                        a = Merge(a);
                        a = RotateGrid(a, -2);
                        mov = 4;
                        break;

                    case ConsoleKey.UpArrow:
                        mov = 1;
                        a = RotateGrid(a, 3);
                        a = Merge(a);
                        a = RotateGrid(a, -3);
                        break;
                }

                Point cp = RandomPoint(a);  // Aqui usamos el objeto tipo point para, usando randompoint() y añadirle el 2 al vector (este 2 seria un nº magico? el juego empieza siempre por 2)
                if (cp != null)
                {
                    a[cp.X, cp.Y] = 2;
                    ReDraw(a, mov);
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



            switch (mov) //esto se queda temporalmente para hacer debug cuando resuelva como sumar los elementos :P
            {
                
                case 1:
                    Console.WriteLine("Arriba");
                    break;
                case 2:
                    Console.WriteLine("Abajo");
                    break;
                case 3:
                    Console.WriteLine("Izquierda");
                    break;
                case 4:
                    Console.WriteLine("Derecha");
                    break;
            }


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
