using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Middle0.Domain.Entities;
using Middle0.Persistence.SeedData;
using System;

namespace Middle0.Persistence.Context
{
    public class EventDbContext: IdentityDbContext<IdentityUser>
	{
		public DbSet<Event> Events { get; set; }

		public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Можно задать Fluent API настройки тут
		}

	}
	public class EventDbContextFactory : IDesignTimeDbContextFactory<EventDbContext>
	{
		public EventDbContext CreateDbContext(string[] args)
		{
			// Загружаем конфигурацию из appsettings.json
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<EventDbContext>();

			// Берём строку подключения
			var connectionString = configuration.GetConnectionString("DefaultConnectionDocker");

			optionsBuilder.UseSqlServer(connectionString);

			return new EventDbContext(optionsBuilder.Options);
		}
	}
}
