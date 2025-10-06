namespace Task_1_Console_Calculator
{
    public class Calculator
    {
        //Calculating the result'
        public static decimal DoCalculation(decimal firstNumber, decimal secondNumber, MathOperations operation)
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
    }
}
