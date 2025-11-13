namespace Inno_Shop_Cherkasov.Exceptions

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
