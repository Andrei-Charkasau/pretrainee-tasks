using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1_Console_Calculator
{
    public class Validator
    {
        /// <summary>
        /// Validates if operation is not a dividing by zero.
        /// </summary>
        /// <param name="number">Input number.</param>
        /// <param name="operation">Chosen operation.</param>
        /// <exception cref="DivideByZeroException">Exception thrown whether operation is dividing by zero.</exception>
        public static void CheckForZeros(decimal number, MathOperations operation)
        {
            if (number == 0 && operation == MathOperations.Divide)
            {
                throw new DivideByZeroException("You cant divide by 0.");
            }
        }

        /// <summary>
        /// Checks if operation choosed correctly.
        /// </summary>
        /// <param name="operation">Chosen operation</param>
        /// <exception cref="Exception">Exception thrown whether operation choice is invalid.</exception>
        public static void CheckOperations(int operation)
        {
            if (!Enum.IsDefined(typeof(MathOperations), operation))
            {
                throw new Exception("Invalid operation choose option.");
            }
        }

        /// <summary>
        /// Validates if the string represents a valid Y/N continuation response.
        /// </summary>
        /// <param name="answer">Input string to validate</param>
        /// <returns>True if input is valid single character 'Y' or 'N', false otherwise.</returns>
        public static bool IsContinue(string answer)
        {
            const char PositiveAnswer = 'Y';
            const char NegativeAnswer = 'N';

            bool isSuccess = Char.TryParse(answer, out char result);
            if (!isSuccess)
            {
                Console.WriteLine("Invalid input format. (Must be Y or N)");
                return false;
            }
            return result == PositiveAnswer || result == NegativeAnswer;
        }

        /// <summary>
        /// Validates if user's answer is a positive one.
        /// </summary>
        /// <returns>True if answer is positive, false otherwise.</returns>
        public static bool isPositiveAnswer()
        {
            string answer;
            const char PositiveAnswer = 'Y';
            do
            {
                Console.WriteLine("\nDo you want to continue calculations? [Y/N]");
                answer = Console.ReadLine();
            }
            while (!Validator.IsContinue(answer));

            if (answer.Contains(PositiveAnswer))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
