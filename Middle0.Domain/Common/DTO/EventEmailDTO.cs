using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Middle0.Domain.Common.DTO
{
    public class EventEmailDTO
    {
		public int Id { get; set; }
		public string Category { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string Images { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Place { get; set; } = null!;
		public DateTime Date { get; set; }
		public TimeSpan Time { get; set; }
		public string AdditionalInfo { get; set; } = null!;
		public string UserEmail { get; set; }
		public string UserName { get; set; }
	}
}
