using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Suculentas.Domain;
using Suculentas.Domain.Identity;

namespace Suculentas.Repository
{
    public class SuculentasContext: IdentityDbContext<User, Role, int, 
                                                        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
                                                        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public SuculentasContext(DbContextOptions<SuculentasContext> options): base(options){}

        public DbSet<Category> Categorys { get; set; }
        public DbSet<Address> Adresses { get; set; }
        public DbSet<LogEmail> LogEmails { get; set; }
        public DbSet<LogException> LogExceptions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ProductType> productTypes { get; set; }
        public DbSet<Sale> Sales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Order>()
                .HasKey(PE => new { PE.SaleId, PE.ProductId});

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                {
                    relationship.DeleteBehavior = DeleteBehavior.Restrict;
                }
                base.OnModelCreating(modelBuilder);
        }
    }
}