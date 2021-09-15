using EfCoreDynamicTable.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EfCoreDynamicTable.Data.Seed
{
    public class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            await InsertStudentSeedData(context);
        }

        static Table[] GetSeedData()
        {
            return new Table[]{
                new Table { Id = Guid.NewGuid(), CreateAt=DateTime.Now, TableName="Tables"}
            };
        }

        static async Task InsertStudentSeedData(ApplicationDbContext context)
        {
            if (context.Tables.Any())
            {
                return;
            }
            await context.Tables.AddRangeAsync(GetSeedData());
            await context.SaveChangesAsync();
        }
    }
}
