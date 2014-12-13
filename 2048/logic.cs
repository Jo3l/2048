using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Logic_2048
{
    class GameLogic
    {
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

    }
}
