using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HangMan
{
    class Program
    {
        static List<char> _word = new List<char>(); // This variable is a character record.
        static List<char> trueChar = new List<char>(); // This variable is a list of correct character
        static List<char> falseChar = new List<char>(); // This variable is a list of incorrect character
        static string enteredChar; // entered letter
        static string secretWord; // This variable is a string of hidden and correct letters. "A P P _ E" 
        static int mistakes = 6;

        static void Main(string[] args)
        {
            using (HangmanContext db = new HangmanContext())
            {

                while (true){
                    Console.WriteLine("If you don't want to add words then press Backspace, else to continue - any button.");
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Backspace)
                    {
                        Console.Clear();
                        break;
                    }
                    Console.WriteLine("There are words in database:");
                    var words = db.Words.ToList();
                    foreach (Words w in words)
                    {
                        Console.WriteLine($"{w.Id} - {w.Word}");
                    }
                    Console.WriteLine("Enter the words to add to the database.");
                    Words wordAdd = new Words { Word = Console.ReadLine().ToUpper() };
                    db.Words.Add(wordAdd);
                    Console.Clear();
                }
                db.SaveChanges();
                Console.Clear();
                //Random word selection
                Words selectionWord = new Words();
                Random rand = new Random();
                int toSkip = rand.Next(1, db.Words.Count());
                db.Words.Skip(toSkip).Take(1).First();
                selectionWord = db.Words.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(1).First();
                string tempWord = selectionWord.Word;
                tempWord = tempWord.ToUpper();
                foreach (char letter in tempWord)
                {
                    _word.Add(letter);
                }
            }
            Console.Clear();
            //The main cycle of the game. Guessing letters
            bool retry = true;
            while (retry)
            {
                Console.WriteLine("You have {0} attemps. Word length is {1}", mistakes, _word.Count);
                Console.WriteLine("You have already entered these letters: {0}", enteredChar);
                retry = selectionLetter(retry);
            }
        }
        static bool selectionLetter(bool retry)
        {
            Console.WriteLine(_word.Count);
            secretWord = "";

            foreach (var c in _word)
            {

                if (trueChar.Contains<char>(c))
                {
                    secretWord += c.ToString() + " ";
                }
                else
                {
                    secretWord += "_ ";
                }
            }
            if (secretWord.IndexOf("_") == -1)
            {
                retry = false;
                Console.Clear();
                Console.WriteLine("Congratulations! You won! Guessed word:");
                Console.WriteLine(secretWord);
                return retry;
            }
            Console.WriteLine(secretWord);
            char ch = System.Console.ReadKey(true).KeyChar;
            ch = Char.ToUpper(ch);
            if (trueChar.Contains<char>(ch) || falseChar.Contains<char>(ch))
            {
                while (trueChar.Contains<char>(ch) || falseChar.Contains<char>(ch))
                {
                    Console.WriteLine("You have already entered this letter");
                    ch = System.Console.ReadKey(true).KeyChar;
                    ch = Char.ToUpper(ch);
                }
            }
            if (_word.Contains<char>(ch))
            {
                trueChar.Add(ch);
                enteredChar += ch.ToString() + ", ";
            }
            else
            {
                falseChar.Add(ch);
                enteredChar += ch.ToString() + ", ";
                mistakes--;
            }
            if (mistakes == 0) 
            { 
                retry = false;
                Console.Clear();
                Console.WriteLine("I'm sorry... You lost... Right word:");
                string rightWord = "";
                foreach (var c in _word) rightWord += c.ToString();
                Console.WriteLine(rightWord);
                return retry;
            }
            Console.Clear();
            return retry;
        }
    }
}
