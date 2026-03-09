
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Signup_Infrastructure.Data.Entities;
[assembly: InternalsVisibleTo("SignupModule.Tests")] // your test project assembly name
namespace Signup_Infrastructure.Data;

internal class SignupDbContext : DbContext
{
    public DbSet<PendingUserCreationEntity> PendingUsers { get; set; }
    public SignupDbContext(DbContextOptions<SignupDbContext> options)
            : base(options) 
    { }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PendingUserCreationEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(40);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(40);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(40);
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.EmailWasSent).IsRequired();
        });
    }
}
