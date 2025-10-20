namespace Task_4_1_Library_ControlSystem.Services
{
    public interface IRepository<T>
        where T : class
    {
        void Insert(T entity);
        void Delete(int id);
        void Update(T patchEntity);
        T Fetch(int id);
        List<T> FetchAll();
    }
}
