using Middle0.Domain.Entities;
using Middle0.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middle0.Persistence.SeedData
{
	public static class DbInitializer
	{
		public static void Seed(EventDbContext context)
		{
			if (!context.Events.Any())
			{
				context.Events.AddRange(
					new EventEntities { Category = "Music", Name = "Rock Fest", Images = "rock.jpg", Description = "Live rock music", Place = "Stadium", Date = DateTime.Today, Time = new TimeSpan(18, 0, 0), AdditionalInfo = "Free drinks" },
					new EventEntities { Category = "Art", Name = "Gallery Night", Images = "gallery.jpg", Description = "Modern art show", Place = "Gallery Center", Date = DateTime.Today.AddDays(1), Time = new TimeSpan(19, 30, 0), AdditionalInfo = "Dress code" },
					new EventEntities { Category = "Tech", Name = "Hackathon", Images = "hack.jpg", Description = "Coding challenge", Place = "Tech Hub", Date = DateTime.Today.AddDays(2), Time = new TimeSpan(9, 0, 0), AdditionalInfo = "Bring your laptop" },
					new EventEntities { Category = "Food", Name = "Food Fest", Images = "food.jpg", Description = "Street food market", Place = "Main Square", Date = DateTime.Today.AddDays(3), Time = new TimeSpan(12, 0, 0), AdditionalInfo = "Entry free" },
					new EventEntities { Category = "Sports", Name = "Marathon", Images = "run.jpg", Description = "City marathon", Place = "City Center", Date = DateTime.Today.AddDays(4), Time = new TimeSpan(7, 0, 0), AdditionalInfo = "Register online" }
				);
				context.SaveChanges();
			}
		}
	}
}
