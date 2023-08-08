using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Writed.Models;

namespace Writed.Data;

public class ApplicationContext : IdentityDbContext<User>
{
    public DbSet<Post> Posts { get; set; }

    public DbSet<Community> Communities { get; set; }

    public DbSet<Comment> Comments { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Community>()
            .HasMany(c => c.Posts)
            .WithOne(p => p.Community)
            .HasForeignKey(p => p.CommunityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
