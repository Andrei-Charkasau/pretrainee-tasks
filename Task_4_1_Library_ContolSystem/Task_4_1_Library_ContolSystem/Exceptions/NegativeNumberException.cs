namespace Task_4_1_Library_ControlSystem.Exceptions
{
    public class NegativeNumberException : Exception
    {
        public int Number { get; }
        public NegativeNumberException(int number, string message) : base($"{number}, {message}")
        {
            this.Number = number;
        }
    }
}
