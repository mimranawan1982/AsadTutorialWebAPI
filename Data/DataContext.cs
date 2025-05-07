using AsadTutorialWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AsadTutorialWebAPI.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<User> Users { get; set; }

    }
}
