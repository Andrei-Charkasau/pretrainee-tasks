using System;
using System.Linq;

namespace Task_1_Console_Calculator
{
    internal class Program
    {
        private const char PositiveAnswer= 'Y';
        static void Main(string[] args)
        {
            decimal firstNumber;
            decimal secondNumber;
            MathOperations choice;
            decimal result;
            string answer;

            do
            {
                try
                {
                    (firstNumber, secondNumber, choice) = InputManager.GetInputData();

                    //Calculating the result
                    result = Calculator.DoCalculation(firstNumber, secondNumber, choice);

                    //Result output in console.
                    Console.WriteLine($"\nCalculation has been completed.\n The result is {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //Asking user does he wants to repeat.
                do
                {
                    Console.WriteLine("\nDo you want to continue calculations? [Y/N]");
                    answer = Console.ReadLine();
                }
                while (!InputManager.IsContinue(answer));
            } while (answer.Contains(PositiveAnswer));
        }
    }
}