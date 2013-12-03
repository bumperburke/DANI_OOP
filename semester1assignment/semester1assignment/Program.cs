using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Runtime.InteropServices;

namespace semester1assignment
{
    class Program
    {
        static List<List<Words>> wordList;
        static List<string> save;
        static string memory = "memory.txt";
        static string book = "quotes.txt";
        static SpeechSynthesizer Speech = new SpeechSynthesizer();
        static string daniReply;

        static void PrintList()
        {
            foreach (var sublist in wordList)
            {
                foreach (Words w in sublist)
                {
                    Console.Write(w.Input + ";" + w.WordCount);
                    Console.Write(" ");
                }
                Console.WriteLine("");
            }
        }

        static void Load(string file)
        {
            string[] lines = System.IO.File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] parse = lines[i].ToLower().Split(' ');
                ListCheck(parse);
            }//end for
        }//end method Load

        static void ListCheck(string[] parsedWords)
        {
            for (int i = 0; i < parsedWords.Length; i++)//Moving through parsedWords array
            {
                bool found = false;
                for (int j = 0; j < wordList.Count; j++)//Moving through wordList
                {
                    if (parsedWords[i] == wordList[j][0].Input)//Comparing parsedWords at position i to the wordList[j] at position 0
                    {
                        found = true;
                        if (i < parsedWords.Length -1)//Checking if i is the last word in parsed array
                        {
                            bool found2 = false;
                            for (int k = 0; k < wordList[j].Count; k++)//Moving through follow words in the wordList at position j
                            {
                                if (parsedWords[i + 1] == wordList[j][k].Input)//Comparing the follow word in parsedWords to the wordList[j] at position k
                                {
                                    found2 = true;
                                    wordList[j][k].WordCount++;
                                }
                            }
                            if(found2 == false)
                            {
                                Words followWord = new Words();
                                followWord.Input = parsedWords[i+1];
                                followWord.WordCount = 1;

                                wordList[j].Add(followWord);
                            }
                        }
                    }
                }

                if (found == false)
                {
                    if (i < parsedWords.Length - 1)
                    {
                        Words newword = new Words();
                        newword.Input = parsedWords[i];
                        newword.WordCount = -1;

                        Words followWord = new Words();
                        followWord.Input = parsedWords[i+1];
                        followWord.WordCount = 1;

                        List<Words> listSub = new List<Words>();
                        listSub.Add(newword);
                        listSub.Add(followWord);

                        wordList.Add(listSub);
                    }
                    else
                    {
                        Words newword = new Words();
                        newword.Input = parsedWords[i];
                        newword.WordCount = -1;

                        List<Words> listSub = new List<Words>();
                        listSub.Add(newword);

                        wordList.Add(listSub);
                    }
                }
            }
        }//End ListCheck

        static string ToString(string[] array)
        {
            string result = string.Join(" ", array);
            return result;
        }//end method

        static void DaniSpeak(string[] sentence)
        {
            string[] output = new string[15];
            Random rnd = new Random();
            int num = rnd.Next(sentence.Length);
            string reply = sentence[num];
            int maxCount = 0;
            int position_x = 0;
            int position_y = 0;

            for ( int k = 0; k < output.Length; k++) //K gives me the position of the word in the output string
            {
                output[k] = reply; //writes a word to the output string
                for (int i = 0; i < wordList.Count; i++) //Goes through the list of lists
                {
                    if (wordList[i][0].Input == reply)//If the word at position[i][0] is the same as the reply word
                    {
                        maxCount = 0;
                        for (int j = 0; j < wordList[i].Count; j++)//Goes through the follow words in the words list at position i
                        {
                            if (wordList[i][j].WordCount > maxCount)//Finds the word with highest word count
                            {
                                maxCount = wordList[i][j].WordCount;
                                position_x = i;//stores i position of word with highest count
                                position_y = j;
                            }
                        }//End j for
                        break;
                    }
                }//End i for
                if (maxCount == 0)
                {
                    break;
                }
                reply = wordList[position_x][position_y].Input;//Puts the follow word with biggest count into reply
            }//End k for


            //string Dreply = ToString(output);
            daniReply = "";
            for (int i = 0; i < output.Length; i++)
            {
                daniReply = daniReply + output[i] + " ";
            }//end for

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("DANI: " + daniReply);
            Speech.SpeakAsync(daniReply);
        }//End DaniSpeak()

        static void Main(string[] args)
        {
            wordList = new List<List<Words>>();
            save = new List<string>();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Welcome to the DANI(Dynamic Artificial Non-Intelligence) program.");
            Console.WriteLine("DANI is a chatbot and will hold a conversation with you.");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1: Load DANI's brain with previous conversations.");
            Console.WriteLine("2: Load DANI's brain with a book.");
            Console.WriteLine("3: Load DANI with no brain and you can teach him.");
            
            bool select = false;
            while (select == false)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("OPTION: ");
                string option = Console.ReadLine().ToLower();
                if (option == "1")
                {
                    Load(memory);
                    Console.WriteLine("DANI's brain is now loaded!");
                    Console.WriteLine("You may start your conversation!");
                    Console.WriteLine("To start simply type what you want to say and hit enter!");
                    Console.WriteLine();
                    Console.WriteLine("Enter \":esc\" and then press enter to exit the program.");
                    select = true;
                }

                else if (option == "2")
                {
                    Load(book);
                    Console.WriteLine("DANI's brain is now loaded!");
                    Console.WriteLine("You may start your conversation!");
                    Console.WriteLine("Start your conversation and press enter!");
                    Console.WriteLine("OR");
                    Console.WriteLine("Enter \":esc\" and then press enter to exit the program.");
                    select = true;
                }

                else if (option == "3")
                {
                    Console.WriteLine("DANI has no words in his memory!");
                    Console.WriteLine("You may start your conversation!");
                    Console.WriteLine("Start your conversation and press enter!");
                    Console.WriteLine("OR");
                    Console.WriteLine("Enter \":esc\" and then press enter to exit the program.");
                    select = true;
                }

                else
                {
                    Console.WriteLine("Incorrect option choosen.");
                    Console.WriteLine();
                }

            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("USER: ");
                string input = Console.ReadLine();
                string[] parsedWords = input.Split(' ');
                save.Add(input);
                if (input == ":esc")
                {
                    break;
                }

                else
                {
                    ListCheck(parsedWords);
                    DaniSpeak(parsedWords);
                    //PrintList();
                }

            }//End while(True)

            Console.WriteLine("To save your conversation to DANI's brain enter \"S\".");
            Console.WriteLine("This will update DANI's memory!");
            Console.WriteLine("Only your input will be saved!");
            Console.WriteLine("If you do not wish to save then enter \"Q\" to exit!");

            bool save_opt = false;
            while (save_opt == false)
            {
                Console.Write("Option: ");
                string option = Console.ReadLine().ToLower();
                if (option == "s")
                {
                    Console.WriteLine("\nDANI is now updating his memory!");
                    Console.WriteLine("Please Wait.....");

                    using (StreamWriter writer = new StreamWriter("memory.txt", true))
                        {
                            foreach(string saveText in save)
                            {
                                writer.WriteLine(saveText);
                            }
                        }
                    Console.WriteLine("Memory Updated. Thank you for using DANI!. Have a nice day :-)");
                    save_opt = true;
                }
                else if (option == "q")
                {
                    Console.WriteLine("Thank you for using DANI!");
                    Console.WriteLine("Have a nice day :-)");
                    save_opt = true;
                }
                else
                {
                    Console.WriteLine("Option not listed. Enter available option!");
                }
            }//End While
            //PrintList();
        }//End Main

    }//End Program Class

}//End Namespace