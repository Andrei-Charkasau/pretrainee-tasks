namespace InnoShop.Core.Models
{
    public abstract class BaseEntity<T>
    {
        public T? Id { get; set; }
    }
}
