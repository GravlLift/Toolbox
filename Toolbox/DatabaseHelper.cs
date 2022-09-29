using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Toolbox
{
    public static class DatabaseHelper
    {
        public static void Migrate<TDbContext>(IServiceProvider serviceProvider)
            where TDbContext : DbContext
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = scope.ServiceProvider.GetService<TDbContext>();
            context.Database.Migrate();
        }
    }
}
