using Microsoft.EntityFrameworkCore;
using MyCRM.Domain.Contacts;
using System.Collections.Generic;

namespace MyCRM.Infrastructure.Persistence;

public sealed class MyCrmDbContext : DbContext
{
    public MyCrmDbContext(DbContextOptions<MyCrmDbContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts => Set<Contact>();
}
