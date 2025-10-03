using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1_Console_Calculator
{
    public class InputManager
    {
        /// <summary>
        /// Gets Input Data from user.
        /// </summary>
        /// <returns>
        /// <param name="firstNumber">First number.</param>
        /// <param name="secondNumber">Seconds number.</param>
        /// <param name="choice">Operation to perform.</param>
        /// </returns>
        public static (decimal firstNumber, decimal secondNumber, MathOperations operation) GetInputData()
        {
            //Get first number. | Validating.
            Console.WriteLine("Enter first number: ");
            decimal firstNumber = InputConverter.GetNumber(Console.ReadLine());

            //Choose what operation is gonna be performed. | Validating.
            Console.WriteLine("\nChoose operation type to proceed:\n1) +\n2) -\n3) *\n4) /");
            MathOperations choice = InputConverter.GetChoice(Console.ReadLine());

            //Get second number. | Validating.
            Console.WriteLine("\nEnter second number: ");
            decimal secondNumber = InputConverter.GetNumber(Console.ReadLine());

            //Exception catch (x/0)?
            Validator.CheckForZeros(secondNumber, choice);

            return (firstNumber, secondNumber, choice);
        }
    }
}
