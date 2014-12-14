using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Game2048
{
    class GameLogic
    {
        static void Main() // Aqui se ejecuta toda la mágia
        {

            do { GameLogic.MainGame(GameInterface.Menu()); } //ejecutamos el juego pasandole como parámetro el metodo menu
            while (GameInterface.PlayAgain()); //luego queda dentro del bucle si se quiere jugar otra partida o salir del juego

        }

        public static void MainGame(int option)  //Este diria que es el metodo principal, donde se invocan los otros métodos
        {
            ScoreTracker.Score = 0; //inicializamos la puntuación. Tambien hace un reset si se juega otra partida.
            int[,] a = new int[option, option]; //inicializamos el vector, el tamaño depende de lo que se devuelva del menu()

            GameInterface.ReDraw(a); // se hace el primer dibujado del grid de juego

            while (true) //o lo que es lo mismo, un bucle infinito
            {
                int mov; //esta variable inicialmente estaba para un debug interno, pero la necesito mas adelante

                ConsoleKeyInfo key = Console.ReadKey(true); //el parametro true de readkey evita que se escriba la letra que se captura
                switch (key.Key) //iniciamos el switchh
                {

                    case ConsoleKey.LeftArrow: //al pulsar el cursor izquierdo del teclado
                        mov = 1; // este valor se usaba en otro método para pintar que movimiento se hacia, se deja para otro uso
                        a = GameLogic.Merge(a); // aqui se hace la mezcla y se mueven los numeros del grid
                        break; //se sale del bucle

                    case ConsoleKey.DownArrow: //no voy a repetirme en decir que hace esto...
                        mov = 2; 
                        a = GameLogic.RotateGrid(a, 1); //bueno, aqui rotamos el array antes de hacer la suma y movimientos del grid
                        a = GameLogic.Merge(a); //suma y movimientos de los numeros del array
                        a = GameLogic.RotateGrid(a, -1); //lo rotamos de nuevo para dejarlo en el sitio 90 grados
                        break;

                    case ConsoleKey.RightArrow: //lo mismo 
                        mov = 3;
                        a = GameLogic.RotateGrid(a, 2); //rotamos 180 grados
                        a = GameLogic.Merge(a); //mezcla
                        a = GameLogic.RotateGrid(a, -2); //deshacemos la rotación
                        break;

                    case ConsoleKey.UpArrow: //idem
                        mov = 4;
                        a = GameLogic.RotateGrid(a, 3); //270 grdos
                        a = GameLogic.Merge(a);
                        a = GameLogic.RotateGrid(a, -3);//volvemos como estaba
                        break;

                    default:
                        mov = 5; //cualquier otra pulsacion de tecla, da el valor 5
                        break;

                }

                Point cp = GameLogic.RandomPoint(a);  // Aqui usando randompoint() creamos un objeto que nos da una posicion aleatoria en el grid
                if (cp != null && mov < 5) //osea, si queda al menos una posición y se pulsó uno de los cursores
                {
                    a[cp.X, cp.Y] = 2; // Añadimos el nº mágico 2 en una posición aleatoria. El juego siempre inserta un 2 asi que no lo considero nº mágico
                    GameInterface.ReDraw(a); //despues de calcular la mezcla y añadir el 2, redibujamos para que se vean los cambios
                }

                if (cp == null && !GameLogic.CanMove(a))  //si no queda ninguna posicion disponible, y además no quedan mas movimientos, con CanMove
                {
                    GameInterface.ReDraw(a, "Game Over"); //usamos redibujar con una sobrecarga con el Game Over
                    break;
                }

                if (GameLogic.Wins(a)) //con el metodo wins() buscamos el 2048 en el vector para saber si hemos ganado el juego
                {
                    GameInterface.ReDraw(a, "Has Ganado!"); //redibujar con la sobrecarga del string 
                    break;
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
            //con 2 for pasamos por todos los elementos del vector
            for (int i = 0; i < a.GetLength(0); i++) //usando el getlenght hacemos que podamos usar cualquier tamaño en e juego
            {
                int lastNum = 0; //inicializamos variables
                int last_j = 0; 
                for (int j = 0; j < a.GetLength(1); j++) //aqui el segundo for con la segunda dimensión
                {
                    if (lastNum != a[i, j] && a[i, j] != 0) //si el nº anterior es distinto de la posición anterior y distinto de 0
                    {
                        lastNum = a[i, j]; //definimos el ultimo numero para la siguiente iteración
                        last_j = j; //definimos la ultima fila para la siguiente iteración
                    }
                    else if (lastNum == a[i, j]) //si el ultimo numero es el mismo que el valor actual, 
                    {
                        a[i, last_j] = 0; //asignamos un 0 o "borramos" uno de los valores que posteriormente vamos a sumar
                        a[i, j] = lastNum + a[i, j]; //sumamos los 2 valores que hemos encontrado juntos
                        ScoreTracker.IncreaseScore(a[i, j]); //damos puntos por los numeros iguales sumados. Usamos un metodo para ello
                    }
                }

                //empuja los numeros por el grid
                last_j = 0; //reseteamos la fila
                for (int j = 0; j < a.GetLength(1); j++) //vamos a desplazar los numeros de la fila, pasamos por todos los valores de la fila de esta iteración
                {
                    if (a[i, j] != 0) //si el valor es distinto de cero
                    {
                        a[i, last_j] = a[i, j]; //cuando encontramos un valor distinto de cero lo desplazamos al inicio(por eso reseteo la fila)
                        if (last_j != j) //con esto borramos los valores desplazados,
                            a[i, j] = 0; //al poner a 0 la posicion j de esta iteracion que no corresponda con el indice del valor movido
                        last_j++; //incrementamos
                    }
                }
            }
            return a; //devolvemos el vector ya mezclado o sumado y movido
        }

        public static int[,] RotateGrid(int[,] a, int rotNum) //este metodo es parecido a un ejercicio hecho en clase
        {
            while (rotNum < 0) //Para obtener las veces que hemos e rotar el array
            {
                rotNum += 4; //el valor que le paso lo sumo a 4, y obtengo las veces que tengo que rotar el vector (2+4=6 veces, -2+4=2 veces)
            }
            for (int rot_i = 0; rot_i < rotNum; rot_i++) //for del numero de rotaciones
            {
                int[,] b = new int[a.GetLength(1), a.GetLength(0)];  //definimos otro vector donde copiamos los valores rotados

                for (int i = 0; i < a.GetLength(0); i++) //for de la primera dimension
                {
                    for (int j = 0; j < a.GetLength(1); j++) //for de la segunda dimensión
                    {
                        b[j, a.GetLength(0) - i - 1] = a[i, j]; //copiamos en b con las posiciones horizontl y vertical cambiadas
                    }
                }

                a = b; //asignamos la referencia del vector b en la del a
            }
            return a; //devolvemos el vector ya rotado
        }

        public static bool CanMove(int[,] a) //este metodo permite saber si quedan movimientos
        {
            bool res = false; //definimos el resultado como false

            int[,] b = ACopy(a); //hacemos una copia del array en b

            //Tenemos que calcular los 4 movimientos posibles, antes de que el jugador haga movimiento, 
            //copiamos el vector en otro y calculamos los movimientos, si al final el vector cambia, es que hay movimientos posibles
            //si el vector no cambia (que se ve con el metodo equals()) es que ya no quedan movimientos posibles

            b = Merge(b); //hacemos el merge hacia la izquierda, ergo no hay rotaciones
            if (!Equals(a, b))
                res = true;

            b = ACopy(a); // resto de movimientos
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


            return res; //devolvemos true o false dependiendo si hay movimientos
        }

        public static bool Equals(int[,] a, int[,] b) //este metodo compara el vector a con el b y devuelve un booleano
        {
            bool res = true; //iniciamos el resultado del metodo
            for (int i = 0; i < a.GetLength(0); i++) //oootra vez mas for para pasar por todos los valores del vector
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (b[i, j] != a[i, j])
                    {
                        res = false;
                        break;//salimos del for
                    }
                }
                if (!res) //si ya encontramos un valor distinto, para que seguir?
                    break;
            }
            return res; //devolvemos resultado
        }

        public static bool Wins(int[,] a) //este método busca el valor 2048 en el vector, 
        {
            bool res = false; //otro boleano
            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    if (a[i, j] == 2048) //si, lo se, otro numero magico, pero el juego es asi, al llegar a 2048, has ganado
                    {
                        res = true;
                        break;//salimos del for
                    }
                }
                if (res) //si ya encontramos un valor distinto, para que seguir?
                    break;
            }
            return res;
        }

        public static int[,] ACopy(int[,] a) //como necesitamos copiar realmente un vector, necesitamos este metodo,
                                             //ya que no es tan facil como poner b=a al ser valores por referencia
        {
            int[,] b = new int[a.GetLength(0), a.GetLength(1)];  //definimos el nuevo vector b
            for (int i = 0; i < a.GetLength(0); i++) //mas for en una dimension
            {
                for (int j = 0; j < a.GetLength(1); j++) //en la otra
                {
                    b[i, j] = a[i, j]; //copiamos valor a valor
                }
            }

            return b; //devolvemos el vector copiado
        }

    }
}
