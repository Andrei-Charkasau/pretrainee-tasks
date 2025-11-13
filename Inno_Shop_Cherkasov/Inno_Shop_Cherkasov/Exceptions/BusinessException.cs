namespace Inno_Shop_Cherkasov.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}