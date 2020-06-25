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
        static List<char> word = new List<char>();
        static List<char> trueChar = new List<char>();
        static List<char> falseChar = new List<char>();
        static string enteredChar;
        static string secretWord;
        static int mistakes = 6;
        static void Main(string[] args)
        {
            
            //Начало цикла
            MakeWord(word);
            Console.WriteLine("Thank you");
            Console.Clear();
            bool retry = true;
            while (retry)
            {
                Console.WriteLine("You have {0} attemps. Word length is {1}", mistakes, word.Count);
                Console.WriteLine("You have already entered these letters: {0}", enteredChar);
                retry = selectionLetter(retry);
            }
        }

        static List<char> MakeWord(List<char> word)
        {
            Console.WriteLine("Hello! \nYou are playing in HangMan. \nMake a word, please!");
            string tempWord = Console.ReadLine();
            tempWord = tempWord.ToUpper();
            foreach (char letter in tempWord)
            {
                word.Add(letter);
            }
            return word;
        }

        static bool selectionLetter(bool retry)
        {
            Console.WriteLine(word.Count);
            secretWord = "";

            foreach (var c in word)
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
            if (word.Contains<char>(ch))
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
                foreach (var c in word) rightWord += c.ToString();
                Console.WriteLine(rightWord);
                return retry;
            }
            Console.Clear();
            return retry;
        }
    }
}
