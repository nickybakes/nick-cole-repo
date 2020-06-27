using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace NotWhoseOnFirstSolver
{
    public enum Position
    {
        TL,
        TR,
        ML,
        MR,
        BL,
        BR
    }


    class NotWhoseOnFirstSolver
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        private Position stage1Position;

        private string stage1Label;

        private string stage1Display;

        private Position stage2Position;

        private string stage2Label;

        private string stage2Display;

        private string[] words;

        private string stage2Table;

        private char[,] stage2Array;

        public NotWhoseOnFirstSolver()
        {
            words = new string[30];
            stage2Array = new char[26, 26];

            //loading in the words file
            Stream inputStream = null;
            StreamReader reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("words.txt");
                reader = new StreamReader(inputStream);

                for(int i = 0; i < words.Length; i++)
                {
                    words[i] = reader.ReadLine();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading words file!");
            }
            finally
            {
                //closin the stream
                if (reader != null)
                {
                    reader.Close();
                }
            }

            //loading in the stage 2 table file
            inputStream = null;
            reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("stage2table.txt");
                reader = new StreamReader(inputStream);

                stage2Table = reader.ReadLine();

                int x = 0;
                int y = 0;
                for (int i = 0; i < stage2Table.Length; i++)
                {
                    stage2Array[x, y] = stage2Table[i];
                    x++;
                    if (x >= 26)
                    {
                        x = 0;
                        y++;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading stage 2 table file!");
            }
            finally
            {
                //closin the stream
                if (reader != null)
                {
                    reader.Close();
                }
            }

            Solve();
        }


        public void Solve()
        {
            for (int y = 0; y < 25; y++)
            {
                for (int x = 0; x < 25; x++)
                {
                    Console.Write(stage2Array[x, y] + " ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("1. Enter display: ");
            Console.Write(" >> ");

            stage1Display = Console.ReadLine();

            stage1Position = SolveStage1();

            Console.WriteLine("Press button in " + stage1Position.ToString() + " position.");

            Console.WriteLine("1. Enter button's label: ");
            Console.Write(" >> ");

            stage1Label = Console.ReadLine();

            Console.WriteLine("2. Enter display: ");
            Console.Write(" >> ");

            stage2Display = Console.ReadLine();

            SolveStage2();
        }

        private Position SolveStage1()
        {
            for (int i = 0; i < words.Length; i++)
            {
                if(stage1Display == words[i])
                {
                    return (Position)(i / 5);
                }
            }
            return Position.TL;
        }

        private void SolveStage2()
        {
            string colWord = stage1Label;
            string rowWord = stage2Display;

            if (colWord.Length < rowWord.Length)
            {
                int difference = rowWord.Length - colWord.Length;
                for(int i = 0; i < difference; i++)
                {
                    colWord += colWord[i];
                    if (i >= colWord.Length)
                        i = 0;
                }
            }
            else if (colWord.Length > rowWord.Length)
            {
                int difference = colWord.Length - rowWord.Length;
                for (int i = 0; i < difference; i++)
                {
                    rowWord += rowWord[i];
                    if (i >= rowWord.Length)
                        i = 0;
                }
            }
            Console.WriteLine(SolveStage2Table(colWord, rowWord));
        }

        private string SolveStage2Table(string colWord, string rowWord)
        {
            Console.WriteLine("Col word = " + colWord);
            Console.WriteLine("Row word = " + rowWord);
            string solvedString = "";

            for(int i = 0; i < colWord.Length; i++)
            {
                solvedString += stage2Array[alphabet.IndexOf(colWord[i]) -1, alphabet.IndexOf(rowWord[i])];
            }

            return solvedString;
        }


        private int GetAlphaPosition(char letter)
        {
            return alphabet.IndexOf(letter) + 1;
        }


    }
}
