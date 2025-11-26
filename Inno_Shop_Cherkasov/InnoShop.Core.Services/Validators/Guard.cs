using InnoShop.Exceptions;

namespace InnoShop.Core.Services.Validators
{
    public static class Guard
    {
        public static T ThrowExceptionIfNull<T>(this T objectToValidate, string errorMessage) // На null
        {
            if (objectToValidate == null)
                throw new ArgumentNullException(errorMessage);
            return objectToValidate;
        }

        public static string ThrowExceptionIfNullOrWhiteSpace(this string stringToValidate, string errorMessage) // На пустую строку
        {
            if (string.IsNullOrWhiteSpace(stringToValidate))
                throw new ArgumentException(errorMessage);
            return stringToValidate;
        }

        public static decimal ThrowExceptionIfNegative(this decimal number, string errorMessage) // На негативное число
        {
            if (number < 0)
                throw new NegativeNumberException(number, errorMessage);
            return number;
        }

        public static void ThrowBusinessExceptionIf(bool condition, string errorMessage) // Проверки через if 
        {
            if (condition)
                throw new BusinessException(errorMessage);
        }

        public static T ThrowNotFoundExceptionIfNull<T>(this T entity, string entityName, object key = null)
        {
            if (entity == null)
            {
                string message;
                if (key != null)
                {
                    message = $"Entity '{entityName}' with key '{key}' was not found.";
                }
                else
                {
                    message = $"{entityName} was not found.";
                }
                throw new NotFoundException(message);
            }
            return entity;
        }
    }
}