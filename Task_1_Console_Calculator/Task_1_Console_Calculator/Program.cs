using System;
using System.Linq;

namespace Task_1_Console_Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            decimal firstNumber;
            decimal secondNumber;
            MathOperations choice;
            decimal result;

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
            } while (Validator.isPositiveAnswer());
        }
    }
}