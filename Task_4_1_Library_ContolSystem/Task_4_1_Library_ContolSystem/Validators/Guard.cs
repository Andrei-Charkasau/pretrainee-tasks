namespace Task_4_1_Library_ControlSystem.Validators
{
    public class Guard
    {
        public void AgainstNull<T>(T objectToValidate, string errorMessage)
        {
            if (objectToValidate == null)
            {
                throw new Exception(errorMessage);
            }
        }

        public void AgainstNullOrEmpty(string stringToValidate, string errorMessage)
        {
            if (string.IsNullOrEmpty(stringToValidate))
            {
                throw new Exception(errorMessage);
            }
        }
    }
}
