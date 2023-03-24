using AdministrationSystem.Eamv.Models.Interfaces;

namespace AdministrationSystem.Eamv.Models.EntityFramework
{
    public class EFDepartmentRepository : IRepositoryCrud<Department>
    {
        private MainDbContext context;

        public EFDepartmentRepository(MainDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Department> Collection { get { return context.Departments; } }

        public void Create(Department department)
        {
            context.Add(department);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            Department department = GetByID(id);

            if (department != null)
            {
                context.Remove(department);
                context.SaveChanges();
            }
        }

        public Department GetByID(int id)
        {
            foreach (Department department in context.Departments)
            {
                if (id == department.DepartmentID)
                {
                    return department;
                }
            }
            return null;
        }

        public void Update(Department department)
        {
            context.Update(department);
            context.SaveChanges();
        }
    }
}
