

using ConsoleApp1.Main.Effects;
using System;
using Main.Commands;


namespace Main
{
    class Program
    {
        static Effect effect;
        static void Main(string[] args)
        {

            Command command = new Command();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                string pathp = command.IPath;
                if (!string.IsNullOrEmpty(pathp)) Console.WriteLine("{0}>", pathp);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("$ ");

               string input = Console.ReadLine();
                command.Update(input);

            }

        }
    }
}
