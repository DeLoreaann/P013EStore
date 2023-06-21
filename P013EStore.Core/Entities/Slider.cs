using System.ComponentModel.DataAnnotations;

namespace P013EStore.Core.Entities
{
	public class Slider : IEntity
	{
        public int Id { get; set; }
		[Display(Name = "Slider Adı")]
		public string? Name { get; set; }
		[Display(Name = "Slider Açıklaması")]
		public string? Description { get; set; }
		[Display(Name = "Slider Resmi")]
		public string? Image { get; set; }
        public string? Link { get; set; }

    }
}
