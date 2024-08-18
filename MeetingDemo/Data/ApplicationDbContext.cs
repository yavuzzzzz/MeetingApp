using MeetingDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingDemo.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Meeting> Meetings { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// User ve Meeting arasındaki ilişkiyi tanımlayın
			modelBuilder.Entity<User>()
				.HasMany(u => u.Meetings)
				.WithOne(m => m.User)
				.HasForeignKey(m => m.UserId);

			// Email alanı için Unique constraint
			modelBuilder.Entity<User>()
				.HasIndex(u => u.Email)
				.IsUnique();
		}
	}
}
