namespace DataImport.Data.Entities
{
    public abstract class BaseEntity
    {
        
    }
    
    public abstract class BaseEntity<T> : BaseEntity
    {
        public T Id { get; set; }
    }
}