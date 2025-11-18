using System.ComponentModel.DataAnnotations;

namespace Middle0.UI.Models
{
	public class Event
	{
		[Key]
		public int Id { get; set; }
		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string Category { get; set; } = null!;
		[Required]
		[StringLength(100, MinimumLength = 3)]
		public string Name { get; set; } = null!;
		public string Images { get; set; } = null!;
		public string Description { get; set; } = null!;
		public string Place { get; set; } = null!;
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[DataType(DataType.Time)]
		public TimeSpan Time { get; set; }
		public string AdditionalInfo { get; set; } = null!;
		public string JobId { get; set; }
	}
}
