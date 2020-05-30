using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
namespace TETA
{
    class Program
    {
        public static int posy = 0;
        public static int posx = 6;

        public static string wll = "▒";
        public static string flr = "▒";
        public static string sqr = "■";//■

        public static bool CanMove = true;

        public static ConsoleKeyInfo chavelol;
        public static bool keyestapressionada = false;

        public static Stopwatch tdelay = new Stopwatch();
        public static int ndelay = 2; //otimização

        public static Stopwatch tdropRate = new Stopwatch();
        public static int ndropRate;

        static Lateta nexttet;
        static Lateta tet;

        public static int hell = 0;
        public static int go = 0;
        public static int TopScore = 0;
        public static bool GameOver = false;

        public static int[,] grid =
            {
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },// ceiling. Se algum numero aqui for 1, Game Over loli
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },//esses 1s orriveis se dão pelo fato que o check CanItR (quando na peça I especificamente), pode dar out of range
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 }, // 22, 16 = 20 + 2, 10 + 6; index = 21, 15
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },// 0 = emp, 1 = wall, 1 = placedteta, 1 = flr
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 },
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },// floor  
            { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
            };
        static void Main()
        {

            DrawBorda();
            if (GameOver == false)
            {
                //esse codigo só vai ser executado uma vez no inicio

                StreamReader sr = new StreamReader("../../highscore.txt");
                try
                {
                    TopScore = Int32.Parse(sr.ReadLine());
                }
                catch (Exception)
                {
                    TopScore = 0;
                }
                sr.Close();//para a leitura do erquivo especificado em sr
                Console.SetCursorPosition(2, 4); Console.WriteLine("Wainting for");
                Console.SetCursorPosition(3, 5); Console.WriteLine("any Input");
                Console.ReadKey(true);
            }
            GameOver = false;
            Clear();
            nexttet = new Lateta();
            tet = nexttet;
            int ClearedLinhas = 0;
            int level = 0;
            int score = 0;
            int combo = 0;
            
            string Resposta;
            DrawShape();
            DrawBorda();

            Console.SetCursorPosition(15, 2); Console.Write("Linhas Cleared : " + ClearedLinhas);
            Console.SetCursorPosition(15, 0); Console.Write("Nível : " + level);
            Console.SetCursorPosition(15, 1); Console.Write("Score : " + score);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Update();
            //aqui VV vai o q acontece ao dar game over, pois o Update() vai parou de ser executado
            if (score > TopScore)
            {
                Console.SetCursorPosition(4, 14); Console.WriteLine("New High");
                Console.SetCursorPosition(5, 15); Console.WriteLine("Score!");
                TopScore = score;
                StreamWriter streamwriter = new StreamWriter("../../highscore.txt");
                streamwriter.WriteLine(TopScore);
                streamwriter.Close();
            }
            Console.SetCursorPosition(6, 18); Console.WriteLine("High");
            Console.SetCursorPosition(5, 19); Console.WriteLine("Score:");
            Console.SetCursorPosition(3, 20); Console.WriteLine(TopScore);
            Console.SetCursorPosition(3, 4); Console.WriteLine("Game Over!");
            Console.SetCursorPosition(3, 5); Console.WriteLine("Try Again?");
            Console.SetCursorPosition(3, 7); Console.WriteLine("'Y' or 'N'");
            Console.SetCursorPosition(7, 9);
            Resposta = Console.ReadLine();
            if (Resposta == "Y" || Resposta == "y")
            {
                Clear();
                Main();
            }
            else
            {
                return; //apicação acaba
            }
            void Clear()
            {
                for (int i = 0; i < 26; i++)
                {
                    Console.SetCursorPosition(0, i); Console.WriteLine("                                  ");
                }
            }//end of void Clear();

            //||=============================||//
            void Update()
            {
                while (GameOver != true)
                {
                    CanMove = true;
                    tdelay.Start();
                    Thread.Sleep(ndelay);
                    tdelay.Stop();
                    DropRate();
                    Input();
                    Logic();
                }
            }
            //||=============================||//         
            void Logic()
            {
                if (combo == 1)
                    score += 40 * level;
                else if (combo == 2)
                    score += 100 * level;
                else if (combo == 3)
                    score += 300 * level;
                else if (combo > 3)
                    score += 300 * combo * level;

                combo = 0;
                ndropRate = 720 - 60 * level;

                if (ClearedLinhas < 5) level = 1;
                else if (ClearedLinhas < 10) level = 2;
                else if (ClearedLinhas < 15) level = 3;
                else if (ClearedLinhas < 25) level = 4;
                else if (ClearedLinhas < 35) level = 5;
                else if (ClearedLinhas < 50) level = 6;
                else if (ClearedLinhas < 70) level = 7;
                else if (ClearedLinhas < 90) level = 8;
                else if (ClearedLinhas < 110) level = 9;
                else if (ClearedLinhas < 150) level = 10;
                Console.SetCursorPosition(15, 0); Console.Write("Level : " + level);
                Console.SetCursorPosition(15, 1); Console.Write("Score : " + score);
                Console.SetCursorPosition(15, 2); Console.Write("Linhas Cleared : " + ClearedLinhas);
                Console.SetCursorPosition(15, 3); Console.Write("DropRate : " + ndropRate);
                Console.SetCursorPosition(14, 21);
            }

            void Input()
            {
                //CHECA SE KEY ESTA PRESSIONADA
                if (Console.KeyAvailable)
                {
                    chavelol = Console.ReadKey();
                    keyestapressionada = true;
                }
                else
                {
                    keyestapressionada = false;
                }

                //SPACE
                if (chavelol.Key == ConsoleKey.Spacebar && keyestapressionada == true)
                {
                    tet.Rotate();
                    //girar a peça
                }
                //LEFT
                else if (chavelol.Key == ConsoleKey.LeftArrow && keyestapressionada == true)
                {
                    ClearShape();
                    posx--;
                    CanItMove();
                    if (CanMove == false)
                    {
                        posx++;
                    }
                    DrawShape();

                }
                //RIGHT
                else if (chavelol.Key == ConsoleKey.RightArrow && keyestapressionada == true)
                {
                    ClearShape();
                    posx++;
                    CanItMove();
                    if (CanMove == false)
                    {
                        posx--;
                    }
                    DrawShape();
                }
                //UP
                else if (chavelol.Key == ConsoleKey.UpArrow && keyestapressionada == true)
                {
                    ClearShape();
                    while (true)
                    {
                        posy++;
                        score++;
                        CanItMove();
                        if (CanMove == false)
                        {
                            posy--;
                            DrawShape();
                            Drop();
                            break;
                        }
                    }
                }
                //DOWN
                else if (chavelol.Key == ConsoleKey.DownArrow && keyestapressionada == true)
                {
                    ClearShape();
                    posy++;
                    score++;
                    CanItMove();
                    if (CanMove)
                    {
                        DrawShape();
                    }
                    else
                    {
                        posy--;
                        DrawShape();
                        Drop();
                    }
                }
            }//end of void INPUT

            void DropRate()
            {
                int ts = stopWatch.Elapsed.Milliseconds;
                if (ts > ndropRate)
                {
                    stopWatch.Reset();
                    ClearShape();
                    posy++;
                    CanItMove();
                    if (CanMove)
                    {
                        DrawShape();
                    }
                    else
                    {
                        posy--;
                        DrawShape();
                        Drop();
                    }
                    stopWatch.Start();
                }
            }//end of void DROPRATE

            void DrawBorda()
            {
                for (int i = 2; i <= 13; i++)
                {
                    Console.SetCursorPosition(i, 0); Console.Write(flr);
                    Console.SetCursorPosition(i, 21); Console.Write(flr);
                }
                for (int i = 0; i <= 20; i++)
                {
                    Console.SetCursorPosition(2, i); Console.Write(wll);
                    Console.SetCursorPosition(13, i); Console.Write(wll);
                }
            }//end of void DrawBorda

            void ClearLinha()
            {
                int i;
                int j;
                for (i = 1; i < 21; i++)
                {
                    for (j = 3; j < 13; j++)
                    {           //y,x
                        if (grid[i, j] == 0)
                        {
                            break;
                        }
                        if (j == 12)
                        {
                            ClearedLinhas++;
                            combo++;

                            for (j = 3; j < 13; j++)//clearlinha
                            {
                                grid[i, j] = 0;        // x,y
                                Console.SetCursorPosition(j, i); Console.Write(" ");
                            }
                            int k;
                            int l;
                            for (k = i; k > 0; k--)
                            {
                                for (l = 3; l < 13; l++)
                                {
                                    if (grid[k, l] == 1)
                                    {
                                        grid[k, l] = 0;
                                        Console.SetCursorPosition(l, k); Console.Write(" ");
                                        if (grid[k + 1, l] == 1)
                                        {
                                            grid[k, l] = 1;
                                            Console.SetCursorPosition(l, k); Console.Write(sqr);
                                        }
                                        else
                                        {
                                            grid[k + 1, l] = 1;
                                            Console.SetCursorPosition(l, k + 1); Console.Write(sqr);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }//end of void ClearLinha

            void CanItMove()
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (tet.shape[i, j] == 1)
                        {
                            if (grid[posy + i, posx + j] == 1)
                            {
                                CanMove = false;
                                break;
                            }
                        }
                    }
                }
            }//end of public static void CanItMove

            void Drop()
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (tet.shape[i, j] == 1)
                        {
                            grid[posy + i, posx + j] = 1;
                        }
                    }
                }
                for (int a = 3; a < 13; a++)
                {
                    if (grid[1, a] == 1)
                    {
                        Console.SetCursorPosition(a, 2); Console.Write("1");
                        GameOver = true;
                        break;
                    }
                }
                if (GameOver == true)
                {
                    go++;
                    Console.SetCursorPosition(19, 19); Console.Write("GO" + go);
                    for (int i = 0; i < 21; i++)
                    {
                        for (int j = 3; j < 13; j++)
                        {
                            Console.SetCursorPosition(j, i); Console.Write(" ");
                            grid[i, j] = 0;
                        }
                    }
                }
                else
                {
                    posy = 0; posx = 6; //resetposition
                    tet = nexttet;
                    nexttet = new Lateta();

                    DrawShape();
                    DrawBorda();
                    ClearLinha();
                }
            }//end of void Drop
        }//end of Main Method


        //==========================================================================================================================================================    
        //public static void pq é chamado pelas duas classes
        public static void ClearShape()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tet.shape[i, j] == 1)
                    {
                        Console.SetCursorPosition(posx + j, posy + i); Console.Write(" ");
                    }
                }
            }
        }//end of void ClearShape
        //o mesmo se aplica a DrawShape. O motivo de colocar eles na classe Program é pq eles tem mais referências nela (só isso)
        public static void DrawShape()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tet.shape[i, j] == 1)
                    {
                        Console.SetCursorPosition(posx + j, posy + i); Console.Write(sqr);
                    }
                }
            }
            Console.SetCursorPosition(14, 21);
        }//end of public static void DrawShape   
    }
    //==========================================================================================================================================================
    public class Lateta
    {
        /*static*/
        int[,] S = { { 0, 0, 0, 0 }, { 0, 0, 1, 1 }, { 0, 1, 1, 0 }, { 0, 0, 0, 0 } };
        /*static*/
        int[,] Z = { { 0, 0, 0, 0 }, { 1, 1, 0, 0 }, { 0, 1, 1, 0 }, { 0, 0, 0, 0 } };
        /*static*/
        int[,] I = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 1, 1, 1, 1 }, { 0, 0, 0, 0 } };
        /*static*/
        int[,] L = { { 0, 0, 0, 0 }, { 0, 0, 0, 1 }, { 0, 1, 1, 1 }, { 0, 0, 0, 0 } };
        /*static*/
        int[,] J = { { 0, 0, 0, 0 }, { 1, 0, 0, 0 }, { 1, 1, 1, 0 }, { 0, 0, 0, 0 } };
        /*static*/
        int[,] T = { { 0, 0, 0, 0 }, { 0, 0, 1, 0 }, { 0, 1, 1, 1 }, { 0, 0, 0, 0 } };
        /*static*/
        int[,] O = { { 0, 0, 0, 0 }, { 0, 1, 1, 0 }, { 0, 1, 1, 0 }, { 0, 0, 0, 0 } };

        public int[,] shape = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        public int[,] Rotatedshape = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
        //List<int[,]> tetrominoes = new List<int[,]>() { S, Z, I, L, J, T, O};
        public static int oilaa = 0;
        public static int oilaaA = 0;
        int x = 0;
        static bool CanItR = true;

        public Lateta()
        {
            oilaaA++;
            Console.SetCursorPosition(18, 27); Console.Write("CreatedLatetas: " + oilaaA);
            Random rnd = new Random();//Pode-se observar que Random é uma classe padrão do C#
            //shape = tetrominoes[rnd.Next(0, 7)];

            //por enquanto essa é a melhor solução lol
            int[] aa = { 1, 2, 3, 4, 5, 6, 7 }; 
            int bb = aa[rnd.Next(0, 7)];
            // maior que esse numero, e menor ou igual a esse. (0 > x <= 7)

            if (bb == 7) shape = S; else if (bb == 1) shape = Z; else if (bb == 2) shape = T; else if (bb == 3) shape = I; else if (bb == 4) shape = O; else if (bb == 5) shape = J; else if (bb == 6) shape = L;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.SetCursorPosition(j + 16, i + 6); Console.Write(" ");
                }
            }
            for (int i = 0; i < shape.GetLength(0); i++)
            {
                for (int j = 0; j < shape.GetLength(1); j++)
                {
                    if (shape[i, j] == 1)
                    {
                        Console.SetCursorPosition(j + 16, i + 6); Console.Write(Program.sqr);
                    }
                }
            }
        }//end of Public Lateta

        public void CanItRotate()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Rotatedshape[i, j] == 1 || shape[i, j] == 1)
                    {
                        if (Program.grid[Program.posy + i, Program.posx + j] == 1)
                        {
                            CanItR = false;
                            break;
                        }
                    }
                }
            }
        }//copypasta lol

        public void Rotate()
        {
            Program.ClearShape();
            //torna todos os valores de rotatedshape para zero
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rotatedshape[i, j] = 0;
                }
            }
            //"algoritmo" que transforma numeros de um array 2D para girarem em 90 graus sentido horário, usando verdades matemáticas simples que eu observei (coisa realmente simples)
            //pega valores de shape e tranforma (giro de 90g) em rotatedshape
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (shape[i, j] == 1)
                    {
                        while (true)
                        {
                            if (x + i == 3)
                            {
                                break;
                            }
                            else
                            {
                                x++;
                            }
                        }
                        Rotatedshape[j, x] = 1;
                        x = 0;
                    }
                }
            }
            CanItRotate();
            if (CanItR == false)
            {
                oilaa++;
                CanItR = true;
                Console.SetCursorPosition(18, 26); Console.Write("CN Rotate: " + oilaa);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (shape[i, j] == 1) { Rotatedshape[i, j] = 1; }
                        else { Rotatedshape[i, j] = 0; }
                    }
                }
            }
            //pega valores de rotatedshape e transfere para shape
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Rotatedshape[i, j] == 1) { shape[i, j] = 1; }
                    else { shape[i, j] = 0; }
                }
            }
            Program.DrawShape();
        }//end of void Rotate
    }
}