using Task_4_1_Library_ControlSystem.Exceptions;

namespace Task_4_1_Library_ControlSystem.Validators
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
