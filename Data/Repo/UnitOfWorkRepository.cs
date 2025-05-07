using AsadTutorialWebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AsadTutorialWebAPI.Data.Repo
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly DataContext db;

        public UnitOfWorkRepository(DataContext db)
        {
            this.db = db;
        }

        public ICityRepository CityRepository => new CityRepository(db);
        public IUserRepository UserRepository => new UserRepository(db);    

        async Task<bool> IUnitOfWork.SaveAsync()
        {
            return await db.SaveChangesAsync() > 0;
        }
    }
}