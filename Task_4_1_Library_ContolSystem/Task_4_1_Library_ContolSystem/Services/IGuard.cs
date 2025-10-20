namespace Task_4_1_Library_ControlSystem.Services
{
    public interface IGuard
    {
        void AgainstNull<T>(T objectToValidate, string errorMessage);
        void AgainstNullOrEmpty(string stringToValidate, string errorMessage);
    }
}
