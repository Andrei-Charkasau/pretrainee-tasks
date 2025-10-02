using System;

namespace Task_1_Console_Calculator
{
    public class Validator
    {
        //Number EXCEPTIONS
        public static decimal GetNumber(string number)
        {
            bool isSuccess = decimal.TryParse(number, out decimal result);
            if (!isSuccess)
            {
                throw new Exception("Input must be numeric.");
            }
            return result;
        }

        //Number EXCEPTIONS'
        public static void CheckForZeros(decimal number, MathOperations operation)
        {
            if (number == 0 && operation == MathOperations.Divide)
            {
                throw new DivideByZeroException("You cant divide by 0.");
            }
        }

        //Choice EXCEPTIONS
        public static MathOperations GetChoice(string stringOperation)
        {
            bool isSuccess = int.TryParse(stringOperation, out int operation);
            if (!isSuccess)
            {
                throw new Exception("Input must be numeric.");
            }
            CheckOperations(operation);
            return (MathOperations)operation;
        }

        //Choice EXCEPTIONS'
        public static void CheckOperations(int operation)
        {
            if (!Enum.IsDefined(typeof(MathOperations), operation))
            {
                throw new Exception("Invalid operation choose option.");
            }
        }

        public static bool IsContinue(string answer)
        {
            bool isSuccess = Char.TryParse(answer, out char result);
            if (!isSuccess)
            {
                Console.WriteLine("Invalid input format. (Must be Y or N)");
                return false;
            }
            return true;
        }
    }
}
