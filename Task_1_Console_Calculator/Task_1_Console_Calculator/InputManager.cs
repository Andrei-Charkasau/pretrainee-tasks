using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1_Console_Calculator
{
    public class InputManager
    {
        public static (decimal firstNumber, decimal secondNumber, MathOperations operation) GetInputData()
        {
            //Get first number. | Validating.
            Console.WriteLine("Enter first number: ");
            decimal firstNumber = Validator.GetNumber(Console.ReadLine());

            //Choose what operation is gonna be performed. | Validating.
            Console.WriteLine("\nChoose operation type to proceed:\n1) +\n2) -\n3) *\n4) /");
            MathOperations choice = Validator.GetChoice(Console.ReadLine());

            //Get second number. | Validating.
            Console.WriteLine("\nEnter second number: ");
            decimal secondNumber = Validator.GetNumber(Console.ReadLine());

            //Exception catch (x/0)?
            Validator.CheckForZeros(secondNumber, choice);

            return (firstNumber, secondNumber, choice);
        }
    }
}
