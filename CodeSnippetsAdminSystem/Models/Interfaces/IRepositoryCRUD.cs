namespace AdministrationSystem.Eamv.Models.Interfaces
{
    public interface IRepositoryCrud<T>
    {
        public IQueryable<T> Collection { get; }
        public void Create(T t);
        public void Delete(int id);
        public T GetByID(int id);
        public void Update(T t);
    }
}
