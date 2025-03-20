namespace KPO.Facades;

public interface IFacade<T>
{
    T Create(T item);
    T Update(T item);
    bool Delete(int id);
    T? GetById(int id);
    IEnumerable<T> GetAll();
}