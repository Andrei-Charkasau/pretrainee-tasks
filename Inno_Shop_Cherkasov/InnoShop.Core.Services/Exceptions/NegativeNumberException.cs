namespace InnoShop.Exceptions

{
    public class NegativeNumberException : Exception
    {
        public decimal Number { get; }
        public NegativeNumberException(decimal number, string message) : base($"{number}, {message}")
        {
            this.Number = number;
        }
    }
}
