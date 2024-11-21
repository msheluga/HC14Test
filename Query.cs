using HC14Test.Models;
using Microsoft.EntityFrameworkCore;

namespace HC14Test
{
    public class Query
    {
        private readonly IDbContextFactory<AdventureWorks2022Context> _dbContextFactory;

        public Query(IDbContextFactory<AdventureWorks2022Context> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        [UsePaging]
        public async Task<IQueryable<Address>> GetAddress()
        {
            var context = await _dbContextFactory.CreateDbContextAsync();
            return context.Addresses;
        }
    }
}
