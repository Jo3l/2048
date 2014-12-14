using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Game2048
{
    class ScoreTracker
    {
        public static int Score; //definimos la variable de la puntuación

        public static void IncreaseScore(int Amount) //metodo que suma a la puntuación el valor que le pasamos
        {
            Score += Amount;
        }

    }
}
