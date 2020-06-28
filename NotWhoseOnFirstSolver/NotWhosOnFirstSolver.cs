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
        TOP_LEFT,
        TOP_RIGHT,
        MIDDLE_LEFT,
        MIDDLE_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT,
        OTHER
    }


    class NotWhosOnFirstSolver
    {
        /// <summary>
        /// The correct 26 letter alphabet
        /// </summary>
        private const string alphabetRight = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// For the columns in the Stage 2 Table, they skipped J for some reason.
        /// </summary>
        private const string alphabetMinusJ = "abcdefghiklmnopqrstuvwxyz";

        private string[] positions = {"tl", "tr", "ml", "mr", "bl", "br"};
        #region Stage 1
        private Position stage1Position;

        private string stage1Label;

        private string stage1Display;

        #endregion

        #region Stage 2
        private Position stage2Position;

        private string stage2Label;

        private string stage2Display;

        private int stage2Calculation;
        #endregion

        #region Stage 3
        private Position stage3Position;

        private Position stage3ReferencePosition;

        private string stage3Label;

        private string stage3ReferenceLabel;

        private string stage3Display;
        #endregion

        #region Stage 4
        private Position stage4Position;

        private Position stage4ReferencePosition;

        private string stage4Label;

        private string stage4ReferenceLabel;

        private string stage4Display;
        #endregion

        private string[] stage1Words;

        private char[,] stage2Table;

        private int[] stage2Numbers;

        private Dictionary<string, string> stage3VennDiagram;

        private Dictionary<string, string> stage3Commands;

        private string[] stage4Words;

        private string[] stage5Words;

        private Dictionary<string, int[]> stage5Numbers;

        public NotWhosOnFirstSolver()
        {
            stage1Words = new string[30];
            stage2Table = new char[25, 26];
            stage2Numbers = new int[60];
            stage3VennDiagram = new Dictionary<string, string>();
            stage3Commands = new Dictionary<string, string>();
            stage4Words = new string[28];
            stage5Words = new string[28];
            stage5Numbers = new Dictionary<string, int[]>();

            //loading in the stage 1words file
            Stream inputStream = null;
            StreamReader reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("stage1words.txt");
                reader = new StreamReader(inputStream);

                for(int i = 0; i < stage1Words.Length; i++)
                {
                    stage1Words[i] = reader.ReadLine();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading stage 1 words file!");
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

                string stage2Text = reader.ReadLine();

                int x = 0;
                int y = 0;
                for (int i = 0; i < stage2Text.Length; i++)
                {
                    stage2Table[x, y] = stage2Text[i];
                    x++;
                    if (x >= 25)
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

            //loading in the stage 2 numbers file
            inputStream = null;
            reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("stage2numbers.txt");
                reader = new StreamReader(inputStream);

                for (int i = 0; i < stage2Numbers.Length; i++)
                {
                    stage2Numbers[i] = int.Parse(reader.ReadLine());
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading stage 2 numbers file!");
            }
            finally
            {
                //closin the stream
                if (reader != null)
                {
                    reader.Close();
                }
            }

            //loading in the stage 3 venn diagram file
            inputStream = null;
            reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("stage3venndiagram.txt");
                reader = new StreamReader(inputStream);

                string allCharacters = reader.ReadLine();

                string[] circleCharacters = new string[4];
                circleCharacters[0] = reader.ReadLine();
                circleCharacters[1] = reader.ReadLine();
                circleCharacters[2] = reader.ReadLine();
                circleCharacters[3] = reader.ReadLine();

                for (int i = 0; i < allCharacters.Length; i++)
                {
                    string keyToPutInDictionary = "";
                    for (int j = 0; j < circleCharacters.Length; j++)
                    {
                        if (circleCharacters[j].Contains(allCharacters[i]))
                        {
                            keyToPutInDictionary += j;
                        }
                    }
                    stage3VennDiagram.Add(keyToPutInDictionary, allCharacters[i].ToString());
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading stage 3 venn diagram file!");
            }
            finally
            {
                //closin the stream
                if (reader != null)
                {
                    reader.Close();
                }
            }

            //loading in the stage 3 commands file
            inputStream = null;
            reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("stage3commands.txt");
                reader = new StreamReader(inputStream);

                string allCommandCharacters = reader.ReadLine();

                for (int i = 0; i < allCommandCharacters.Length; i++)
                {

                    stage3Commands.Add(allCommandCharacters[i].ToString(), reader.ReadLine()) ;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading stage 3 commands file!");
            }
            finally
            {
                //closin the stream
                if (reader != null)
                {
                    reader.Close();
                }
            }

            //loading in the stage 4 words file
            inputStream = null;
            reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("stage4words.txt");
                reader = new StreamReader(inputStream);

                for (int i = 0; i < stage4Words.Length; i++)
                {
                    stage4Words[i] = reader.ReadLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading stage 4 words file!");
            }
            finally
            {
                //closin the stream
                if (reader != null)
                {
                    reader.Close();
                }
            }

            //loading in the stage 5 words file
            inputStream = null;
            reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("stage5words.txt");
                reader = new StreamReader(inputStream);

                for (int i = 0; i < stage5Words.Length; i++)
                {
                    stage5Words[i] = reader.ReadLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading stage 5 words file!");
            }
            finally
            {
                //closin the stream
                if (reader != null)
                {
                    reader.Close();
                }
            }

            //loading in the stage 5 numbers file
            inputStream = null;
            reader = null;

            try
            {
                //opening the stream to the binary file, creating a reader
                inputStream = File.OpenRead("stage5numbers.txt");
                reader = new StreamReader(inputStream);

                for (int i = 0; i < stage5Words.Length; i++)
                {
                    stage5Numbers.Add(stage5Words[i], new int[6]);
                    string numbers = reader.ReadLine();
                    for(int j = 0; j < 6; j++)
                    {
                        stage5Numbers[stage5Words[i]][j] = int.Parse(numbers[j].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading stage 5 numbers file!");
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
            #region STAGE 1
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("\n--------- STAGE 1 ---------\n");


            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("1. Enter display: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");

            stage1Display = Console.ReadLine();

            stage1Position = SolveStage1Chart(stage1Display);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n1. Enter the " + stage1Position.ToString() + " label: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");

            stage1Label = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press the button in the " + stage1Position.ToString() + " position.");
            #endregion

            #region STAGE 2

            Console.WriteLine("\n--------- ONTO STAGE 2 ---------\n");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("2. Enter display: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");

            stage2Display = Console.ReadLine();

            stage2Position = SolveStage2();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n2. Enter the " + stage2Position.ToString() + " label: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");

            stage2Label = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press the button in the " + stage2Position.ToString() + " position.");

            #endregion

            #region STAGE 3

            Console.WriteLine("\n--------- ONTO STAGE 3 ---------\n");

            //get display
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("3. Enter display: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");
            stage3Display = Console.ReadLine();

            //figure out the reference position by using Stage 1's chart
            stage3ReferencePosition = SolveStage1Chart(stage3Display);

            //get the label for that reference position
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nYour Stage 3 reference button is in the " + stage3ReferencePosition.ToString() + " position.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("3. Enter the " + stage3ReferencePosition.ToString() + " label: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");
            stage3ReferenceLabel = Console.ReadLine();

            //using this data, solve the venn diagram
            stage3Position = SolveStage3VennDiagram(stage3ReferencePosition, stage3ReferenceLabel, stage3Display);

            //get the label for the final button
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("3. Enter the " + stage3Position.ToString() + " label: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");
            stage3Label = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press the button in the " + stage3Position.ToString() + " position.");

            #endregion

            Console.WriteLine("\n--------- ONTO STAGE 4 ---------\n");

            //get display
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("4. Enter display: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");
            stage4Display = Console.ReadLine();

            //Solve stage 4 using the stage 3 reference position as the starting point
            stage4Position = SolveStage4(stage3ReferencePosition);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("4. Enter the " + stage4Position.ToString() + " label: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" >> ");

            stage4Label = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press the button in the " + stage4Position.ToString() + " position.");

            Console.WriteLine("\n--------- ONTO STAGE 5 ---------\n");

            Console.WriteLine("Press the button in the " + SolveStage5().ToString() + " position!!!!");
        }

        private Position SolveStage1Chart(string display)
        {
            for (int i = 0; i < stage1Words.Length; i++)
            {
                if(display == stage1Words[i])
                {
                    return (Position)(i / 5);
                }
            }
            return Position.OTHER;
        }

        private Position SolveStage2()
        {
            string colWord = stage1Label;
            string rowWord = stage2Display;

            colWord = RemoveInvalidCharacters(colWord);
            rowWord = RemoveInvalidCharacters(rowWord);

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

            string solvedString = SolveStage2Table(colWord, rowWord);
            Console.WriteLine("Solved String: " + solvedString);
            int sumOfAlphaPositions = 0;

            for(int i = 0; i < solvedString.Length; i++)
            {
                sumOfAlphaPositions += GetCorrectedAlphaPosition(solvedString[i]);
            }

            Console.WriteLine("Sum of alphabetic positions: " + sumOfAlphaPositions);

            int moduloAnswerCorrected = (sumOfAlphaPositions % 60) + 1;
            
            stage2Calculation = moduloAnswerCorrected;

            Console.WriteLine("Corrected modulo answer: " + moduloAnswerCorrected);

            for (int i = 0; i < stage2Numbers.Length; i++)
            {
                if (moduloAnswerCorrected == stage2Numbers[i])
                {
                    return (Position)(i / 10);
                }
            }
            return Position.OTHER;
        }

        private Position SolveStage3VennDiagram(Position referencePosition, string referenceLabel, string display)
        {
            Console.WriteLine("\nSolving Venn Diagram");

            string conditionTracker = "";
            //if the button is on the left column
            if(referencePosition == Position.TOP_LEFT || referencePosition == Position.MIDDLE_LEFT || referencePosition == Position.BOTTOM_LEFT)
            {
                conditionTracker += 0;
            }
            //if the label has an even number of letters
            if (RemoveInvalidCharacters(referenceLabel).Length % 2 == 0)
            {
                conditionTracker += 1;
            }
            //if displayed word has an odd number of vowels
            if(CountVowels(display) % 2 == 1)
            {
                conditionTracker += 2;
            }
            //if number calculated from Stage 2 is prime
            if (IsPrime(stage2Calculation))
            {
                conditionTracker += 3;
            }

            //once we have compared all the conditions, get the output of the venn diagram
            string vennDiagramResult = stage3VennDiagram[conditionTracker];

            //if the output is a number, return the button position for that number
            int buttonToPress;
            if (int.TryParse(vennDiagramResult, out buttonToPress))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nVenn diagram result was: " + buttonToPress);
                return (Position)(buttonToPress-1);
            }
            //"C" means press the reference button
            else if(vennDiagramResult == "c")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPick the reference button:");
                return referencePosition;
            }
            //"O" means press the other button in the same row as the reference button
            else if (vennDiagramResult == "o")
            {
                //print out the condition for picking a button
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPick the other button in the same row as the reference button:");
                //so we just move to the left and get the other button
                KeyValuePair<int, int> coordPosition = TranslateToCoordinates(referencePosition);
                return MoveAndTranslateToPosition(coordPosition.Key, coordPosition.Value, 1, false);
            }
            //any other command should let the user pick a button, so prompt to pick a button
            else
            {
                //print out the condition for picking a button
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPick ***" + stage3Commands[vennDiagramResult] + "*** and record info...");

                //then tell user to type in the button position they chose
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Enter button position (tl, tr, ml, mr, bl, br): ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" >> ");

                Position chosenPosition = Position.OTHER;

                //continually loop until the user has typed in a valid position
                while (chosenPosition == Position.OTHER)
                {
                    string positionInput = Console.ReadLine().ToLower();
                    for (int i = 0; i < positions.Length; i++)
                    {
                        if (positionInput == positions[i])
                        {
                            chosenPosition = (Position)i;
                            break;
                        }
                    }
                    if (chosenPosition != Position.OTHER)
                        break;

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Invalid, try again! (tl, tr, ml, mr, bl, br)");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" >> ");
                }

                return chosenPosition;
            }


        }

        private Position SolveStage4(Position currentPosition)
        {
            string[] stage4ButtonLabels = new string[6];

            do
            {
                //get the label of the current position
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n4. Enter the " + currentPosition.ToString() + " label: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" >> ");
                stage4ButtonLabels[(int)currentPosition] = Console.ReadLine();

                //use this label to determine the direction to move
                int direction = 0;
                for (int i = 0; i < stage4Words.Length; i++)
                {
                    if (stage4ButtonLabels[(int)currentPosition] == stage4Words[i])
                    {
                        direction = i / 7;
                        break;
                    }
                }

                KeyValuePair<int, int> coordPosition = TranslateToCoordinates(currentPosition);
                currentPosition = MoveAndTranslateToPosition(coordPosition.Key, coordPosition.Value, direction, true);
            }
            //if the label of the current position is null, that means we have not been there yet, and will need to get the label
            //if it is NOT null, it means it is the repeat, and we use that as the reference button for stage 4
            while (stage4ButtonLabels[(int)currentPosition] == null);

            stage4ReferenceLabel = stage4ButtonLabels[(int)currentPosition];
            stage4ReferencePosition = currentPosition;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nYour Stage 4 reference button is in the " + stage4ReferencePosition.ToString() + " position.");
            Console.WriteLine("Its label is \"" + stage4ReferenceLabel + "\"");

            return SolveStage3VennDiagram(stage4ReferencePosition, stage4ReferenceLabel, stage4Display);
        }

        private Position SolveStage5()
        {
            int[] solvedValues = new int[6];

            solvedValues[0] = stage5Numbers[stage1Label][(int)stage1Position];
            solvedValues[1] = stage5Numbers[stage2Label][(int)stage2Position];
            solvedValues[2] = stage5Numbers[stage3Label][(int)stage3Position];
            solvedValues[3] = stage5Numbers[stage4Label][(int)stage4Position];
            solvedValues[4] = stage5Numbers[stage3ReferenceLabel][(int)stage3ReferencePosition];
            solvedValues[5] = stage5Numbers[stage4ReferenceLabel][(int)stage4ReferencePosition];

            int sum = 0;
            for(int i = 0; i < solvedValues.Length; i++)
            {
                sum += solvedValues[i];
            }

            sum += stage2Calculation;

            int moduloAnswerCorrected = (sum % 60) + 1;

            for (int i = 0; i < stage2Numbers.Length; i++)
            {
                if (moduloAnswerCorrected == stage2Numbers[i])
                {
                    return (Position)(i / 10);
                }
            }
            return Position.OTHER;
        }

        private Position MoveAndTranslateToPosition(int x, int y, int direction, bool verbose)
        {
            switch (direction)
            {
                //up
                case 0:
                    if(verbose)
                        Console.WriteLine("Moving UP ^");
                    y--;
                    break;
                //left
                case 1:
                    if (verbose)
                        Console.WriteLine("Moving LEFT <");
                    x = 1 - x;
                    break;
                //right
                case 2:
                    if (verbose)
                        Console.WriteLine("Moving RIGHT >");
                    x = 1 - x;
                    break;
                //down
                case 3:
                    if (verbose)
                        Console.WriteLine("Moving DOWN v");
                    y++;
                    break;
            }

            if (y < 0)
                y = 2;
            else if (y > 2)
                y = 0;

            //finally, translate it back into a position and return
            if(x == 0)
            {
                if (y == 0)
                    return Position.TOP_LEFT;
                else if (y == 1)
                    return Position.MIDDLE_LEFT;
                else if (y == 2)
                    return Position.BOTTOM_LEFT;
            }
            else
            {
                if (y == 0)
                    return Position.TOP_RIGHT;
                else if (y == 1)
                    return Position.MIDDLE_RIGHT;
                else if (y == 2)
                    return Position.BOTTOM_RIGHT;
            }

            return Position.OTHER;
        }

        private KeyValuePair<int, int> TranslateToCoordinates(Position position)
        {
            if (position == Position.TOP_LEFT)
                return new KeyValuePair<int, int>(0, 0);
            else if (position == Position.MIDDLE_LEFT)
                return new KeyValuePair<int, int>(0, 1);
            else if (position == Position.BOTTOM_LEFT)
                return new KeyValuePair<int, int>(0, 2);
            else if (position == Position.TOP_RIGHT)
                return new KeyValuePair<int, int>(1, 0);
            else if (position == Position.MIDDLE_RIGHT)
                return new KeyValuePair<int, int>(1, 1);
            else
                return new KeyValuePair<int, int>(1, 2);
        }

        private Position CheckIfHitTwice(int[] positionsHit, out Position hitTwice)
        {
            for(int i = 0; i < positionsHit.Length; i++)
            {
                if(positionsHit[i] > 1)
                {
                    hitTwice = (Position)i;
                    return (Position)i;
                }
            }
            hitTwice = Position.OTHER;
            return Position.OTHER;
        }

        private bool IsPrime(int num)
        {
            if (num == 1)
                return false;

            int j = num / 2;
            for(int i = 2; i <= j; i++)
            {
                if(num % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private int CountVowels(string word)
        {
            string vowels = "aeiou";
            int numOfVowels = 0;
            for(int i = 0; i < word.Length; i++)
            {
                if (vowels.Contains(word[i]))
                    numOfVowels++;
            }
            return numOfVowels;
        }

        private string RemoveInvalidCharacters(string word)
        {
            string correctedWord = "";
            for(int i = 0; i < word.Length; i++)
            {
                if (alphabetRight.Contains(word[i]))
                {
                    correctedWord += word[i];
                }
            }

            return correctedWord;
        }

        private string SolveStage2Table(string colWord, string rowWord)
        {
            Console.WriteLine("Col word = " + colWord);
            Console.WriteLine("Row word = " + rowWord);
            string solvedString = "";

            for(int i = 0; i < colWord.Length; i++)
            {
                solvedString += stage2Table[alphabetMinusJ.IndexOf(colWord[i]), alphabetRight.IndexOf(rowWord[i])];
            }

            return solvedString;
        }


        private int GetCorrectedAlphaPosition(char letter)
        {
            return alphabetRight.IndexOf(letter.ToString().ToLower()) + 1;
        }

        private void PrintStage2Table()
        {
            for (int y = 0; y < 26; y++)
            {
                Console.Write(y + ": ");
                for (int x = 0; x < 25; x++)
                {
                    Console.Write(stage2Table[x, y] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
