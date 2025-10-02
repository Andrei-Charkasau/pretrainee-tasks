using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            char answer;

            do
            {
                try
                {
                    (firstNumber, secondNumber, choice) = GetInputData();

                    //Calculating the result
                    result = DoCalculation(firstNumber, secondNumber, choice);

                    //Result output in console.
                    Console.WriteLine($"\nCalculation has been completed.\n The result is {result}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    //Asking user does he wants to repeat.
                    Console.WriteLine("\nDo you want to continue calculations? [Y/N]");
                    answer = Convert.ToChar(Console.ReadLine());
                }
            } while (answer.Equals(PositiveAnswer));
        }

        //Number EXCEPTIONS
        private static decimal GetNumber(string number)
        {
            bool isSuccess = decimal.TryParse(number, out decimal result);
            if (!isSuccess)
            {
                throw new Exception("Input must be numeric.");
            }
            return result;
        }

        //Number EXCEPTIONS'
        private static void CheckForZeros(decimal number, MathOperations operation)
        {
            if (number == 0 && operation == MathOperations.Divide)
            {
                throw new DivideByZeroException("You cant divide by 0.");
            }
        }

        //Choice EXCEPTIONS
        private static MathOperations GetChoice(string stringOperation)
        {
            bool isSuccess = int.TryParse(stringOperation, out int operation);
            if (!isSuccess)
            {
                throw new Exception("Input must be numeric.");
            }
            CheckForOptions(operation);
            return (MathOperations)operation;
        }

        //Choice EXCEPTIONS'
        private static void CheckForOptions(int operation)
        {
            if (!Enum.IsDefined(typeof(MathOperations), operation))
            {
                throw new Exception("Invalid operation choose option.");
            }
        }

        //Calculating the result'
        private static decimal DoCalculation(decimal firstNumber, decimal secondNumber, MathOperations operation)
        {
            switch (operation)
            {
                case MathOperations.Sum:
                    return firstNumber + secondNumber;
                case MathOperations.Minus:
                    return firstNumber - secondNumber;
                case MathOperations.Multiply:
                    return firstNumber * secondNumber;
                case MathOperations.Divide:
                    return firstNumber / secondNumber;
                default: return 0;
            }
        }

        private static (decimal firstNumber, decimal secondNumber, MathOperations operation) GetInputData()
        {
            //Get first number. | Validating.
            Console.WriteLine("Enter first number: ");
            decimal firstNumber = GetNumber(Console.ReadLine());

            //Choose what operation is gonna be performed. | Validating.
            Console.WriteLine("\nChoose operation type to proceed:\n1) +\n2) -\n3) *\n4) /");
            MathOperations choice = GetChoice(Console.ReadLine());

            //Get second number. | Validating.
            Console.WriteLine("\nEnter second number: ");
            decimal secondNumber = GetNumber(Console.ReadLine());

            //Exception catch (x/0)?
            CheckForZeros(secondNumber, choice);

            return (firstNumber, secondNumber, choice);
        }
    }
}
