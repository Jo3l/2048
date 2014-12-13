using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Game_2048
{
    class ScoreTracker
    {
        public static int Score;

        public static void IncreaseScore(int Amount)
        {
            Score += Amount;
        }

    }
}
