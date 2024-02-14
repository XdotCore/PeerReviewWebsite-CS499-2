using Microsoft.EntityFrameworkCore;
using PeerReviewWebsite.Classes.Data.Login;

namespace PeerReviewWebsite.Classes.Data {
    /// <summary>
    /// Represents the entirety of the websites database
    /// </summary>
    public class WebsiteDbContext(DbContextOptions options) : DbContext(options) {
        /// <summary>
        /// The users in the database
        /// </summary>
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Add the Users table
            modelBuilder.Entity<User>(entity => {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username);
                entity.Property(e => e.Password);
            });
        }
    }
}
