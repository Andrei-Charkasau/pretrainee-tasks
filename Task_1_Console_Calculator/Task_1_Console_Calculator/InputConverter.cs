using System;

namespace Task_1_Console_Calculator
{
    public class InputConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">Input number.</param>
        /// <returns>
        /// <param name="result">Converted input.
        /// </returns>
        /// <exception cref="Exception"></exception>
        public static decimal GetNumber(string number)
        {
            bool isSuccess = decimal.TryParse(number, out decimal result);
            if (!isSuccess)
            {
                throw new Exception("Input must be numeric.");
            }
            return result;
        }

        //Choice EXCEPTIONS
        public static MathOperations GetChoice(string stringOperation)
        {
            bool isSuccess = int.TryParse(stringOperation, out int operation);
            if (!isSuccess)
            {
                throw new Exception("Input must be numeric.");
            }
            Validator.CheckOperations(operation);
            return (MathOperations)operation;
        }


    }
}
