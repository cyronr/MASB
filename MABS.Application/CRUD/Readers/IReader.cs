namespace MABS.Application.CRUD.Readers
{
    public interface IReader<T>
    {
        Task<T> GetByUUIDAsync(Guid uuid);
        Task<List<T>> GetAllAsync();
    }
}
