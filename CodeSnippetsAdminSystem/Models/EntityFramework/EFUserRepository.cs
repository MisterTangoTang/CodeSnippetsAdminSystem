using AdministrationSystem.Eamv.Models.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class EFUserRepository : IUserRepository
    {
        private UserDbContext context;
        public EFUserRepository(UserDbContext context)
        {
            this.context = context;
        }
        public IQueryable<User> Collection { get { return context.Users; } }

        public void Create(User user)
        {
            user.Password = HashComputer(user.Password);

            context.Add(user);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            User user = GetByID(id);

            if (user != null)
            {
                context.Remove(user);
                context.SaveChanges();
            }
        }

        public User GetByID(int id)
        {
            foreach (User a in context.Users)
            {
                if (id == a.UserID)
                {
                    return a;
                }
            }
            return null;
        }

        public User GetByUsername(string username)
        {
            foreach (User a in context.Users)
            {
                if (username.Equals(a.UserName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return a;
                }
            }
            return null;
        }


        public void Update(User user)
        {
            User user2 = GetByID(user.UserID);

            if (user2 != null)
            {
                user2.UserName = user.UserName;
                user2.UserRole = user.UserRole;
                context.SaveChanges();
            }
        }

        public bool UserChecker(User user)
        {
            user.Password = HashComputer(user.Password);

            foreach (User u in context.Users)
            {
                if (user.UserName.Equals(u.UserName, StringComparison.CurrentCultureIgnoreCase) && user.Password.Equals(u.Password))
                    return true;
            }
            return false;
        }
        public string HashComputer(string password)
        {
            password += "B)Vw9[<kA0)y";
            HashAlgorithm algo = HashAlgorithm.Create("sha512")!;
            byte[] hash = algo.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(Encoding.UTF8.GetBytes(BitConverter.ToString(hash).Replace("-", String.Empty)));
        }
    }
}
