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

        public NotWhoseOnFirstSolver()
        {
            words = new string[30];

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

            }
            else if (colWord.Length > rowWord.Length)
            {

            }
            else
            {
                Console.WriteLine(SolveStage2Table(colWord, rowWord));
            }
        }

        private string SolveStage2Table(string colWord, string rowWord)
        {
            string solvedString = "";

            for(int i = 0; i < colWord.Length; i++)
            {
                int position = alphabet.IndexOf(colWord[i]) + (26 * alphabet.IndexOf(rowWord[i]));
                solvedString += stage2Table[position];
            }

            return solvedString;
        }


        private int GetAlphaPosition(char letter)
        {
            return alphabet.IndexOf(letter) + 1;
        }


    }
}
