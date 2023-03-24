namespace AdministrationSystem.Eamv.Models.Interfaces
{
    public interface IUserRepository
    {
        public IQueryable<User> Collection { get; }
        public void Create(User user);
        public void Delete(int id);
        public User GetByID(int id);
        public void Update(User user);
        public bool UserChecker(User user);
        public User GetByUsername(string username);
        public string HashComputer(string password);
    }
}
