using Microsoft.EntityFrameworkCore;
using Middle0.Domain.Entities;
using System;

namespace Middle0.Persistence.Context
{
    public class EventDbContext: DbContext
    {
		public DbSet<EventEntities> Events { get; set; }

		public EventDbContext(DbContextOptions<EventDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Можно задать Fluent API настройки тут
		}

	}
}
