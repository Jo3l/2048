using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Game2048
{

    //objeto que guarda la posicion aleatoria

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
