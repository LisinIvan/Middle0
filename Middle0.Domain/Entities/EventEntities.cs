using System.ComponentModel.DataAnnotations;

namespace Middle0.Domain.Entities
{
    public class EventEntities
    {
        [Key]
        public int Id { get; set; }
		[Required]
		public string Category { get; set; } = null!;
		[Required]
		public string Name { get; set; } = null!;
		public string Images { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Place { get; set; } = null!;
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[DataType(DataType.Time)]
		public TimeSpan Time { get; set; }
		public string AdditionalInfo { get; set; } = null!;
	}
}
