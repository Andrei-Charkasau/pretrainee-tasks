using Inno_Shop_Cherkasov.Exceptions;

namespace Inno_Shop_Cherkasov.Validators
{
    public static class Guard
    {
        public static void ThrowExceptionIfNull<T>(this T objectToValidate, string errorMessage)
        {
            if (objectToValidate == null)
            {
                throw new ArgumentNullException(errorMessage);
            }
        }

        public static void ThrowExceptionIfNullOrWhiteSpace(this string stringToValidate, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(stringToValidate))
            {
                throw new ArgumentNullException(errorMessage);
            }
        }

        public static void ThrowExceptionIfNegativeYear(int year, string errorMessage)
        {
            if (year <= 0)
            {
                throw new NegativeNumberException(year, errorMessage);
            }
        }
    }
}
