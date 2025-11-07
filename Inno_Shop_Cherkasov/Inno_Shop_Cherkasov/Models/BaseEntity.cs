namespace Inno_Shop_Cherkasov.Models
{
    public abstract class BaseEntity<T>
    {
        public T? Id { get; set; }
    }
}
